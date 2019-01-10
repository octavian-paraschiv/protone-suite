using OPMedia.Core.Configuration;
using OPMedia.Core.Logging;
using OPMedia.DeezerInterop.PlayerApi;
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
        /// OPMedia.DeezerWorker.exe
        /// </summary>
        Deezer,
        /// <summary>
        /// OPMedia.AudioCdWorker.exe
        /// </summary>
        AudioCd,
        /// <summary>
        /// OPMedia.VideoDvdWorker.exe
        /// </summary>
        VideoDvd,
        /// <summary>
        /// OPMedia.AudioWorker.exe
        /// </summary>
        Audio,
        /// <summary>
        /// OPMedia.VideoWorker.exe
        /// </summary>
        Video,
        /// <summary>
        /// OPMedia.ShoutcastWorker.exe
        /// </summary>
        Shoutcast,
    }

    public class WorkerProcess : IDisposable, IWorkerPlayer
    {
        Process _wp = null;

        public event WorkerTerminatedHandler WorkerTerminated = null;
        public event StateEventHandler StateChanged = null;
        public event RenderEventHandler RenderEvent = null;

        public int Pid { get; private set; }

        public WorkerProcess(WorkerType workerType)
        {
            ProcessStartInfo psi = new ProcessStartInfo($".\\OPMedia.{workerType}Worker.exe");
            psi.CreateNoWindow = true;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.WorkingDirectory = AppConfig.InstallationPath;
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

        public void Play(string url)
        {
            SetCommand(WorkerCommandType.PlayReq, url);

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

        public string GetFilterState()
        {
            return GetCommand<string>(WorkerCommandType.GetStateReq);
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
            try
            {
                var cmd = new WorkerCommand(wct);
                if (args != null && args.Length > 0)
                {
                    foreach (object arg in args)
                        cmd.AddParameter(arg.ToString()); ;
                }

                if (WorkerCommandHelper.WriteCommand(_wp?.StandardInput, cmd))
                {
                    var replyCmd = WorkerCommandHelper.ReadCommand(_wp?.StandardOutput);
                    if (replyCmd != null && replyCmd.Type == (WorkerCommandType)(wct + 1))
                    {
                        CheckReply(wct.ToString(), replyCmd);
                        return;
                    }
                }

                DeezerApi.HandleDzErrorCode(wct.ToString(), dz_error_t.DZ_ERROR_BAD_OPERATION_RSP);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
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
            // Decode reply code as integer
            int replyCode = cmd.Args<int>(0);
            if (replyCode >= 0x10000000)
            {
                // The reply command wraps an error code
                dz_error_t dzError = (dz_error_t)(replyCode - 0x10000000);
                DeezerApi.HandleDzErrorCode(operation, dzError);
            }
        }
    }
}
