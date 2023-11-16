using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Security.AccessControl;
using System.Threading;

namespace OPMedia.ShellSupport
{
    public static class PlayerSupport
    {

        const int MaxStartupAttempts = 20;

        const string PlayerName = "OPMedia.ProTONE.exe";
        const string MutexName = "Global\\opmedia.protone.mutex";

        static readonly string _deployPath = "";
        static PlayerSupport()
        {
            _deployPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public static void SendPlayerCommand(CommandType cmdType, string[] args)
        {
            try
            {
                CheckPlayerRunning();

                var data = new
                {
                    ChangeType = 3,
                    Id = Guid.NewGuid().ToString(),
                    PersistenceId = "IpcEvent",
                    PersistenceContext = Environment.UserName,
                    ObjectContent = $"BasicCommand>{cmdType}?{string.Join("?", args)}"
                };

                // The format of the "Enqueue/Play files" command sent as IpcEvent is:
                // {"ChangeType":3,"Id":"(guid)","PersistenceId":"IpcEvent","PersistenceContext":"(win user)","ObjectContent":"BasicCommand>(cmdType)?path_1?path_2..."}

                SendSimpleMessage(JsonConvert.SerializeObject(data, Formatting.None));
            }
            catch { }
        }

        private static void SendSimpleMessage(string dataToSend)
        {
            // Connect to the Persistence Service which also acts as message dispatcher
            var client = new TcpClient("localhost", 10200);
            if (client.Connected)
            {
                using (NetworkStream ns = client.GetStream())
                using (StreamWriter sw = new StreamWriter(ns))
                {
                    sw.WriteLine(dataToSend);
                    sw.Flush();
                }
                client.Close();
            }
        }

        private static void CheckPlayerRunning()
        {
            int i = 0;

            bool playerRunning = IsPlayerRunning();
            if (playerRunning)
            {
                Thread.Sleep(1000);
                return;
            }

            // See if player is started; if not - start it
            while (!playerRunning && i < MaxStartupAttempts)
            {
                try
                {
                    Process.Start($"{_deployPath}/{PlayerName}");
                    Thread.Sleep(2000);
                    playerRunning = IsPlayerRunning();
                }
                catch
                {
                    playerRunning = false;
                }

                i++;
            }

            if (playerRunning)
            {
                Thread.Sleep(1000);
                return;
            }

            throw new InvalidOperationException();
        }

        private static bool IsPlayerRunning()
        {
            try
            {
                using (Mutex m = Mutex.OpenExisting(MutexName, MutexRights.ReadPermissions))
                {
                }
                return true;
            }
            catch
            {
            }

            return false;
        }
    }
}
