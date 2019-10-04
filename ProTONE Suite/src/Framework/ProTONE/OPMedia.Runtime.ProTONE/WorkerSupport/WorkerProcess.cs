using OPMedia.Core;
using OPMedia.Core.Configuration;
using OPMedia.Core.Logging;
using OPMedia.Core.Utilities;
using OPMedia.DeezerInterop.PlayerApi;
using OPMedia.Runtime.ProTONE.Rendering;
using OPMedia.Runtime.ProTONE.Rendering.DS;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace OPMedia.Runtime.ProTONE.WorkerSupport
{
    public delegate void WorkerTerminatedHandler(int pid);
    public delegate void StateEventHandler(string state);
    public delegate void RenderEventHandler(int pos);

    public enum WorkerType
    {
        /// <summary>
        /// Unsupported media
        /// </summary>
        Unsupported = 0,

        /// <summary>
        /// OPMedia.DeezerWorker.exe
        /// </summary>
        Deezer,
        /// <summary>
        /// OPMedia.ShoutcastWorker.exe
        /// </summary>
        Shoutcast,

        /// <summary>
        /// OPMedia.AudioWorker.exe
        /// </summary>
        Audio,

        /// <summary>
        /// OPMedia.AudioCdWorker.exe
        /// </summary>
        AudioCd,
        /// <summary>
        /// OPMedia.VideoDvdWorker.exe
        /// </summary>
        VideoDvd,
      
        /// <summary>
        /// OPMedia.VideoWorker.exe
        /// </summary>
        Video,
    }

    public class WorkerProcess : IDisposable, IWorkerPlayer
    {
        public const char InnerArrayDelim = ';';

        Process _wp = null;

        public event WorkerTerminatedHandler WorkerTerminated = null;
        public event StateEventHandler StateChanged = null;
        public event RenderEventHandler RenderEvent = null;

        public int Pid { get; private set; }

        WorkerType _wt = WorkerType.Deezer;

        public WorkerProcess(WorkerType workerType)
        {
            _wt = workerType;

            ProcessStartInfo psi = new ProcessStartInfo($".\\OPMedia.{workerType}Worker.exe");
            psi.CreateNoWindow = true;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.WorkingDirectory = LiteAppConfig.InstallationPath;
            psi.UseShellExecute = false;

            _wp = Process.Start(psi);
            _wp.Exited += _wp_Exited;

            this.Pid = _wp.Id;
        }

        private void _wp_Exited(object sender, EventArgs e)
        {
            WorkerTerminated?.Invoke(Pid);
        }

        public void Dispose()
        {
            if (_wp != null && _wp.HasExited == false)
                _wp.Kill();
        }

        public int GetLength()
        {
            return GetCommand<int>(WorkerCommandType.GetLenReq);
        }

        public int GetMediaPosition()
        {
            return GetCommand<int>(WorkerCommandType.GetPosReq);
        }

        public int GetVolume()
        {
            return GetCommand<int>(WorkerCommandType.GetVolReq);
        }

        public void Pause()
        {
            SetCommand(WorkerCommandType.PauseReq);
        }

        public void Play(string url, string userId, int delayStart, long renderHwnd, long notifyHwnd)
        {
            SetCommand(WorkerCommandType.PlayReq, url, userId, delayStart, renderHwnd, notifyHwnd);

            ThreadPool.QueueUserWorkItem((c) => ReadEvents());
        }

        void ReadEvents()
        {
            try
            {
                while (true)
                {
                    var evt = WorkerCommandHelper.ReadCommand(_wp?.StandardError);
                    if (evt == null)
                    {
                        // process exited ??
                        WorkerTerminated?.Invoke(Pid);
                        return;
                    }
                    //else if (evt.IsValid)
                    //{
                    //    switch (evt.Type)
                    //    {
                    //        case WorkerCommandType.StateEvt:
                    //            StateChanged?.Invoke(evt.Args<string>(0));
                    //            break;

                    //        case WorkerCommandType.RenderEvt:
                    //            RenderEvent?.Invoke(evt.Args<int>(0));
                    //            break;
                    //    }
                    //}
                }
            }
            catch
            { }

            return;
        }

        public void Resume(int pos)
        {
            SetCommand(WorkerCommandType.ResumeReq, pos);
        }

        public void SetMediaPosition(int pos)
        {
            SetCommand(WorkerCommandType.SetPosReq, pos);
        }

        public void SetVolume(int vol)
        {
            SetCommand(WorkerCommandType.SetVolReq, vol);
        }

        public void Stop()
        {
            SetCommand(WorkerCommandType.StopReq);
        }

        public FilterState GetFilterState()
        {
            return GetCommand<FilterState>(WorkerCommandType.GetStateReq);
        }

        public void ResizeRenderRegion()
        {
            SetCommand(WorkerCommandType.ResizeRenderRegionReq);
        }

        private T GetCommand<T>(WorkerCommandType wct)
        {
            try
            {
                var cmd = new WorkerCommand(wct);
                if (WorkerCommandHelper.WriteCommand(_wp?.StandardInput, cmd))
                {
                    var replyCmd = WorkerCommandHelper.ReadCommand(_wp?.StandardOutput);
                    if (replyCmd != null && replyCmd.Type == (WorkerCommandType)(wct + 1))
                        return DecodeReply<T>(wct.ToString(), replyCmd);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return default(T);
        }

        private void SetCommand(WorkerCommandType wct, params object[] args)
        {
            WorkerCommand cmd = new WorkerCommand(wct);
            WorkerCommand replyCmd = null;

            try
            {
                if (args != null && args.Length > 0)
                {
                    foreach (object arg in args)
                        cmd.AddParameter(arg.ToString()); ;
                }

                if (WorkerCommandHelper.WriteCommand(_wp?.StandardInput, cmd))
                {
                    replyCmd = WorkerCommandHelper.ReadCommand(_wp?.StandardOutput);
                    if (replyCmd != null && replyCmd.Type == (WorkerCommandType)(wct + 1))
                    {
                        CheckReply(wct.ToString(), replyCmd);
                        return;
                    }
                }

                if (wct != WorkerCommandType.StopReq)
                    // The worker process will die when StopReq is sent so don't bother processing the response.
                    WorkerException.Throw(WorkerError.Generic, dz_error_t.DZ_ERROR_BAD_OPERATION_RSP.ToString());
            }
            catch (Exception ex)
            {
                if (_wp != null && _wp.HasExited == false)
                {
                    if (replyCmd != null)
                        Logger.LogError($"Killing worker process after exception, last command: {cmd}, reply: {replyCmd}");
                    else
                        Logger.LogError($"Killing worker process after exception, last command: {cmd}, no reply");

                    _wp.Kill();
                    _wp = null;
                }

                throw ex;
            }
        }

        private T DecodeReply<T>(string operation, WorkerCommand cmd)
        {
            CheckReply(operation, cmd);

            // Since it is OK, decode reply as expected type
            return cmd.Args<T>(0);
        }

        private void CheckReply(string operation, WorkerCommand cmd)
        {
            // Decode reply code as string
            string replyCode = cmd.Args<string>(0);
            if (replyCode.StartsWith("err_"))
            {
                // The reply command wraps an error code
                WorkerError errorType = WorkerError.Generic;
                Enum.TryParse<WorkerError>(replyCode.Replace("err_", ""), out errorType);

                string errorCode = cmd.Args<string>(1);

                WorkerException.Throw(errorType, errorCode);
            }
        }

        public void SetCommandProcessor(CommandProcessor proc)
        {
            // Not needed here ...
        }
    }
}
