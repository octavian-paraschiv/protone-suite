using OPMedia.Core.Logging;
using OPMedia.Runtime.ProTONE.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace OPMedia.Runtime.ProTONE.WorkerSupport
{
    public class Worker
    {
        private string _appName;

        private System.Timers.Timer _tmrCheckWorkerLoop = null;
        private ManualResetEvent _evtCmdReceived = new ManualResetEvent(false);

        public static void Run(IWorkerPlayer player)
        {
            CommandProcessor cmdProcessor = new CommandProcessor(player);
            string appName = player.GetType().Namespace;
            Worker worker = new Worker(appName);
            worker.Run(cmdProcessor);
        }

        private Worker(string appName)
        {
            _appName = appName;

            int interval = 5000;

            if (ProTONEConfig.XFade)
            {
                interval = Math.Max(1000 * ProTONEConfig.XFadeLength, interval);
            }

            _tmrCheckWorkerLoop = new System.Timers.Timer();
            _tmrCheckWorkerLoop.Interval = interval;
            _tmrCheckWorkerLoop.Elapsed += _tmrCheckWorkerLoop_Elapsed;
            _tmrCheckWorkerLoop.Start();
        }

        private void _tmrCheckWorkerLoop_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _tmrCheckWorkerLoop.Stop();

                if (_evtCmdReceived.WaitOne(0) == false)
                {
                    Logger.LogTrace($"Worker loop has not received any command in {_tmrCheckWorkerLoop.Interval / 1000 } sec ... Exiting worker process.");

                    // Kill worker process
                    Process.GetCurrentProcess().Kill();
                    return;
                }

                _evtCmdReceived.Reset();
            }
            catch { }
            finally
            {
                _tmrCheckWorkerLoop.Start();
            }
        }

        private void Run(CommandProcessor proc)
        {
            LoggedApplication.Start(_appName, false);

            try
            {
                Logger.LogInfo("Worker process starting");

                using (StreamReader stdin = new StreamReader(Console.OpenStandardInput()))
                using (StreamWriter stdout = new StreamWriter(Console.OpenStandardOutput()))
                using (StreamWriter stderr = new StreamWriter(Console.OpenStandardError()))
                {
                    proc.SetEventStream(stderr);

                    while (true)
                    {
                        WorkerCommand cmd = WorkerCommandHelper.ReadCommand(stdin);
                        if (cmd == null)
                        {
                            Logger.LogTrace("Worker loop broken after reading null command.");
                            break;
                        }
                        if (cmd.IsValid == false)
                        {
                            Logger.LogTrace("Worker loop broken after reading invalid command.");
                            break;
                        }

                        // Signal that the worker loop received a command
                        _evtCmdReceived.Set();

                        Logger.LogToConsole($"Worker loop processing command: {cmd}");

                        WorkerCommand replyCmd = proc.Process(cmd);
                        if (replyCmd == null)
                        {
                            Logger.LogTrace("Worker loop broken after responding with null command");
                            break;
                        }
                        if (replyCmd.IsValid == false)
                        {
                            Logger.LogTrace("Worker loop broken after responding with invalid command.");
                            break;
                        }

                        if (WorkerCommandHelper.WriteCommand(stdout, replyCmd) == false)
                        {
                            Logger.LogTrace("Worker loop broken after failing to write response command.");
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            finally
            {
                Logger.LogInfo("Worker process exiting");
                LoggedApplication.Stop();
            }
        }
    }
}
