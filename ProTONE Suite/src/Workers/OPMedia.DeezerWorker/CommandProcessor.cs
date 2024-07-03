using OPMedia.Core.Logging;
using OPMedia.DeezerInterop.PlayerApi;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using OPMedia.Runtime.ProTONE.WorkerSupport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OPMedia.DeezerWorker
{
    public class CommandProcessor : ICommandProcessor
    {
        private DeezerPlayer _player = null;
        private StreamWriter _evtStream = null;

        public CommandProcessor()
        {
            _player = new DeezerPlayer(this);
        }

        public void SetEventStream(StreamWriter evtStream)
        {
            _evtStream = evtStream;
        }

        public WorkerCommand Process(WorkerCommand cmd)
        {
            WorkerCommand replyCmd = null;

            string replyArg = "0";

            try
            {
                replyCmd = new WorkerCommand(cmd.Type + 1);

                switch (cmd.Type)
                {
                    case WorkerCommandType.PlayReq:
                        _player.Play(cmd.Args<string>(0));
                        break;

                    case WorkerCommandType.PauseReq:
                        _player.Pause();
                        break;

                    case WorkerCommandType.ResumeReq:
                        _player.Resume(cmd.Args<int>(0));
                        break;

                    case WorkerCommandType.StopReq:
                        // No reply cmd needed since we need to die anyways on Stop
                        replyCmd = null;
                        _player.Stop();
                        break;

                    case WorkerCommandType.SetVolReq:
                        _player.SetVolume(cmd.Args<int>(0));
                        break;

                    case WorkerCommandType.GetPosReq:
                        replyArg = ((int)_player.GetMediaPosition()).ToString();
                        break;

                    case WorkerCommandType.SetPosReq:
                        _player.SetMediaPosition(cmd.Args<int>(0));
                        break;

                    case WorkerCommandType.GetStateReq:
                        replyArg = _player.FilterState.ToString();
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
            catch (DeezerPlayerException dpe)
            {
                Logger.LogException(dpe);
                long code = WorkerProcess.GenericErrorCode + (long)dpe.DzErrorCode;
                replyArg = code.ToString();
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }

            replyCmd?.AddParameter(replyArg);

            return replyCmd;
        }

        public void StateEvt(FilterState state)
        {
            var cmd = new WorkerCommand(WorkerCommandType.StateEvt);
            cmd.AddParameter(cmd.ToString());
            WorkerCommandHelper.WriteCommand(_evtStream, cmd);
        }

        public void RenderEvt(int pos)
        {
            var cmd = new WorkerCommand(WorkerCommandType.RenderEvt);
            cmd.AddParameter(pos.ToString());
            WorkerCommandHelper.WriteCommand(_evtStream, cmd);
        }
    }
}
