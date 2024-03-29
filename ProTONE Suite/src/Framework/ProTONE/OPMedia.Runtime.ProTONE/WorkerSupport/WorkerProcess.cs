﻿using OPMedia.Core.Configuration;
using OPMedia.Core.Logging;
using OPMedia.DeezerInterop.PlayerApi;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using System;
using System.Diagnostics;
using System.Threading;

namespace OPMedia.Runtime.ProTONE.WorkerSupport
{
    public delegate void WorkerTerminatedHandler(int pid);
    public delegate void WorkerEventHandler(WorkerEvent evt);

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
        WorkerClientStream _wcs = null;

        public event WorkerTerminatedHandler WorkerTerminated = null;
        public event WorkerEventHandler WorkerEvent = null;

        public int Pid { get; private set; }

        WorkerType _wt = WorkerType.Deezer;
        string _fileName = null;

        public WorkerProcess(WorkerType workerType, string args = null)
        {
            _wt = workerType;
            _fileName = $"OPMedia.{workerType}Worker.exe";

            ProcessStartInfo psi = new ProcessStartInfo($".\\{_fileName}");
            psi.CreateNoWindow = true;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.WorkingDirectory = AppConfig.InstallationPath;
            psi.UseShellExecute = false;
            psi.Arguments = args;

            _wp = Process.Start(psi);

            this.Pid = _wp.Id;

            _wcs = new WorkerClientStream(this.Pid);

        }

        public void Dispose()
        {
            if (_wp != null && _wp.HasExited == false)
                _wp.Kill();

            _wcs.Dispose();
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

            ThreadPool.QueueUserWorkItem(_ =>
            {
                while (true)
                {
                    var evt = ReadEvent();
                    if (evt != null)
                        WorkerEvent?.Invoke(evt);
                    else
                        break;

                    Thread.Sleep(5);
                }
            });

            ThreadPool.QueueUserWorkItem(_ => MonitorWorkerProcess());
        }

        void MonitorWorkerProcess()
        {
            try
            {
                while (true)
                {
                    var proc = Process.GetProcessById(Pid);
                    if (proc != null && proc.MainModule != null)
                    {
                        if (string.Compare(proc.MainModule.ModuleName, _fileName, true) == 0)
                        {
                            // worker process still running
                            Thread.Sleep(500);
                            continue;
                        }
                    }

                    Logger.LogTrace($"The worker process spawned as {_fileName} with PID={Pid} has exited.");

                    // worker process exited
                    WorkerTerminated?.Invoke(Pid);
                    return;
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
            Logger.LogToConsole($"WorkerProcess::SetVolume: pid={Pid}, vol={vol}");
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
                if (_wcs?.WriteCommand(cmd) == true)
                {
                    var replyCmd = _wcs?.ReadCommand();
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
                        cmd.AddParameter(arg.ToString());
                }

                if (_wcs?.WriteCommand(cmd) == true)
                {
                    replyCmd = _wcs?.ReadCommand();
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
                        Logger.LogWarning($"Killing worker process after exception, last command: {cmd}, reply: {replyCmd}");
                    else
                        Logger.LogWarning($"Killing worker process after exception, last command: {cmd}, no reply");

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
            return cmd.Arg<T>(0);
        }

        private void CheckReply(string operation, WorkerCommand cmd)
        {
            // Decode reply code as string
            string replyCode = cmd.Arg<string>(0);
            if (replyCode.StartsWith("err_"))
            {
                // The reply command wraps an error code
                WorkerError errorType = WorkerError.Generic;
                Enum.TryParse<WorkerError>(replyCode.Replace("err_", ""), out errorType);

                string errorCode = cmd.Arg<string>(1);

                WorkerException.Throw(errorType, errorCode);
            }
        }

        public void SetCommandProcessor(CommandProcessor proc)
        {
            // Not needed here ...
        }

        public WorkerEvent ReadEvent()
        {
            try
            {
                var cmd = _wcs?.ReadEvent();
                if (cmd != null)
                    return new WorkerEvent(cmd);
            }
            catch
            {
            }

            return null;
        }
    }
}
