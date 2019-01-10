﻿using OPMedia.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OPMedia.Runtime.ProTONE.WorkerSupport
{
    public class Worker
    {
        private string _appName;

        public Worker(string appName)
        {
            _appName = appName;
        }

        public void Run(ICommandProcessor proc)
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

                        WorkerCommand replyCmd = proc.Process(cmd);
                        if (replyCmd == null)
                        {
                            Logger.LogTrace("Worker loop broken after responding with null command.");
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
