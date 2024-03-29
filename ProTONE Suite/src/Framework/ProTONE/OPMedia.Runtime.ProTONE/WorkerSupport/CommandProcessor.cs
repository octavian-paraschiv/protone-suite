﻿using OPMedia.Core.Logging;
using OPMedia.Runtime.ProTONE.WorkerSupport;
using System;

namespace OPMedia.Runtime.ProTONE
{
    public class CommandProcessor
    {
        private IWorkerPlayer _player = null;

        public CommandProcessor(IWorkerPlayer player)
        {
            _player = player;
            _player.SetCommandProcessor(this);
        }

        public WorkerCommand Process(WorkerCommand cmd)
        {
            WorkerCommand replyCmd = null;

            string replyArg = "0";
            string replyArg2 = null;

            try
            {
                replyCmd = new WorkerCommand(cmd.Type + 1);

                switch (cmd.Type)
                {
                    case WorkerCommandType.PlayReq:
                        {
                            int i = 0;
                            string path = cmd.Arg<string>(i++);
                            string userId = cmd.Arg<string>(i++);
                            int delayStart = cmd.Arg<int>(i++);
                            long renderHwnd = cmd.Arg<long>(i++);
                            long notifyHwnd = cmd.Arg<long>(i++);
                            _player.Play(path, userId, delayStart, renderHwnd, notifyHwnd);
                        }
                        break;

                    case WorkerCommandType.PauseReq:
                        _player.Pause();
                        break;

                    case WorkerCommandType.ResumeReq:
                        _player.Resume(cmd.Arg<int>(0));
                        break;

                    case WorkerCommandType.StopReq:
                        // No reply cmd needed since we need to die anyways on Stop
                        replyCmd = null;
                        _player.Stop();
                        break;

                    case WorkerCommandType.SetVolReq:
                        _player.SetVolume(cmd.Arg<int>(0));
                        break;

                    case WorkerCommandType.GetPosReq:
                        replyArg = ((int)_player.GetMediaPosition()).ToString();
                        break;

                    case WorkerCommandType.SetPosReq:
                        _player.SetMediaPosition(cmd.Arg<int>(0));
                        break;

                    case WorkerCommandType.GetStateReq:
                        replyArg = _player.GetFilterState().ToString();
                        break;

                    case WorkerCommandType.GetVolReq:
                        replyArg = _player.GetVolume().ToString();
                        break;

                    case WorkerCommandType.GetLenReq:
                        replyArg = _player.GetLength().ToString();
                        break;

                    default:
                        break;
                }
            }
            catch (WorkerException ex)
            {
                Logger.LogException(ex);
                replyArg = $"err_{ex.ErrorType}";
                replyArg2 = ex.Message;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }

            if (replyCmd != null)
            {
                replyCmd.AddParameter(replyArg);

                if (string.IsNullOrEmpty(replyArg2) == false)
                    replyCmd.AddParameter(replyArg2);
            }

            return replyCmd;
        }
    }
}
