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
        public const long GenericErrorCode = 0x10000000;
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

        public double[] GetLevels()
        {
            if (GetSupportedMeteringData().HasFlag(SupportedMeteringData.Levels))
            {
                string rsp = GetCommand<string>(WorkerCommandType.GetLevelsReq);
                return StringUtils.CoerceToVectorOf<double>(rsp, InnerArrayDelim);
            }

            return null;
        }

        public double[] GetWaveform()
        {
            if (GetSupportedMeteringData().HasFlag(SupportedMeteringData.Waveform))
            {
                string rsp = GetCommand<string>(WorkerCommandType.GetWaveformReq);
                return StringUtils.CoerceToVectorOf<double>(rsp, InnerArrayDelim);
            }

            return null;
        }

        public double[] GetSpectrogram()
        {
            if (GetSupportedMeteringData().HasFlag(SupportedMeteringData.Spectrogram))
            {
                string rsp = GetCommand<string>(WorkerCommandType.GetSpectrogramReq);
                return StringUtils.CoerceToVectorOf<double>(rsp, InnerArrayDelim);
            }

            return null;
        }

        SupportedMeteringData? _sup = null;

        public SupportedMeteringData GetSupportedMeteringData()
        {
            if (_sup == null)
                _sup = GetCommand<SupportedMeteringData>(WorkerCommandType.GetSupportedMeteringDataReq);

            return _sup.GetValueOrDefault();
        }

        public void Pause()
        {
            SetCommand(WorkerCommandType.PauseReq);
        }

        public void Play(string url, string userId, int delayStart)
        {
            SetCommand(WorkerCommandType.PlayReq, url, userId, delayStart);

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
                    HandleErrorCode(wct.ToString(), (WorkerError)dz_error_t.DZ_ERROR_BAD_OPERATION_RSP);
            }
            catch(Exception ex)
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

                throw;
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
            if (replyCode >= GenericErrorCode)
            {
                // The reply command wraps an error code
                WorkerError error = (WorkerError)(replyCode - GenericErrorCode);

                int hr = 0;
                if (error == WorkerError.RenderingError)
                {
                    // Decode HR from the second argument
                    hr = cmd.Args<int>(1);
                }

                HandleErrorCode(operation, error, hr);
            }
        }

        private void HandleErrorCode(string operation, WorkerError errCode, int hr = 0)
        {
            switch (_wt)
            {
                case WorkerType.Deezer:
                    DeezerApi.HandleDzErrorCode(operation, (dz_error_t)errCode);
                    break;

                default:
                    {
                        if (errCode == (WorkerError)dz_error_t.DZ_ERROR_BAD_OPERATION_RSP)
                            errCode = WorkerError.Generic;

                        // Try to recompose the original exception
                        WorkerException.ThrowForHResult(hr);
                        WorkerException.ThrowForErrorCode(errCode);
                    }
                    break;
            }
        }

        public void SetCommandProcessor(CommandProcessor proc)
        {
            // Not needed here ...
        }
    }
}
