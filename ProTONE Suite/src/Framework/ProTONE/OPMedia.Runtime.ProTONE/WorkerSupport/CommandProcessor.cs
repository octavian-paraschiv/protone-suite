using OPMedia.Core.Logging;
using OPMedia.Runtime.ProTONE;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using OPMedia.Runtime.ProTONE.WorkerSupport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OPMedia.Runtime.ProTONE
{
    public class CommandProcessor
    {
        private IWorkerPlayer _player = null;
        private StreamWriter _evtStream = null;

        public CommandProcessor(IWorkerPlayer player)
        {
            _player = player;
            _player.SetCommandProcessor(this);
        }

        public void SetEventStream(StreamWriter evtStream)
        {
            _evtStream = evtStream;
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
            catch (WorkerException ex)
            {
                Logger.LogException(ex);
                long code = WorkerProcess.GenericErrorCode + (long)ex.WorkerErrorCode;
                replyArg = code.ToString();

                if (ex.WorkerErrorCode == WorkerError.RenderingError)
                    replyArg2 = ex.HResult.ToString();
            }
            catch(Exception ex)
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
