using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using System.Threading;

namespace OPMedia.ShellSupport
{
    public static class PlayerRemoteControl
    {

        const int MaxStartupAttempts = 20;

        const string PlayerName = "OPMedia.ProTONE.exe";
        const string MutexName = "Global\\opmedia.protone.mutex";

        static readonly string _deployPath = "";
        static PlayerRemoteControl()
        {
            _deployPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public static void SendPlayerCommand(string cmdType, string[] args)
        {
            try
            {
                CheckPlayerRunning();

                string data = $"{cmdType}?{string.Join("?", args)}";
                WmCopyDataSender.SendData(PlayerName, data);
            }
            catch { }
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

        public static bool IsPlayerRunning()
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
