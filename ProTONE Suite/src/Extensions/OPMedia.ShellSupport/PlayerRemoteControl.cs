using OPMedia.ShellSupport;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;

namespace OPMedia.Runtime.ProTONE
{
    public enum CommandType
    {
        Activate = 0,
        Terminate,
        PlayFiles,
        EnqueueFiles,
        ClearPlaylist,
        Playback,

        BrowseRemoteFiles,
        GetDriveList,

        QueryMediaRenderer,

        KeyPress,
    }

    public static class PlayerRemoteControl
    {

        const int MaxStartupAttempts = 20;

        public static void SendPlayerCommand(CommandType cmdType, string[] args)
        {
            int i = 0;

            bool playerWasAlreadyRunning = IsPlayerRunning();
            {
                // See if player is started; if not - start it
                while ((!IsPlayerRunning() && i < MaxStartupAttempts))
                {
                    Process.Start(ShellProTONEConfig.PlayerInstallationPath);
                    i++;
                    Thread.Sleep(2000);
                }
            }

            if (IsPlayerRunning())
            {
                if (!playerWasAlreadyRunning)
                {
                    // The player has just been launched. Give it some time to boot up.
                    Thread.Sleep(2000);
                }
            }


            List<string> fields = new List<string>();
            fields.Add(cmdType.ToString());

            if (args != null && args.Length > 0)
            {
                fields.AddRange(args);
            }

            string command = FromStringArray(fields.ToArray(), '?');
        }

        private static string FromStringArray(string[] array, char delim)
        {
            string retVal = string.Empty;

            if (array != null && array.Length > 0)
            {
                foreach (string field in array)
                {
                    retVal += field;
                    retVal += delim;
                }
            }

            return retVal.TrimEnd(new char[] { delim });
        }

        public static bool IsPlayerRunning()
        {
            string mutexName = OPMedia.ShellSupport.ShellConstants.PlayerName.Replace(" ", "").ToLowerInvariant() + @".mutex";
            try
            {
                using (Mutex m = Mutex.OpenExisting("Global\\" + mutexName, MutexRights.ReadPermissions))
                {
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
