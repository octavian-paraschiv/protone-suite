using OPMedia.Core;
using OPMedia.Core.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace OPMedia.Runtime.ProTONE.FfdShowApi
{
    public static class FfdShowConfig
    {
        const string CLSID = "{4DB2B5D9-4556-4340-B189-AD20110D953F}";
        const int MSG_TRAYICON = 32777;

        public static string InstallLocation { get; } = RegistrationSupport.GetInstalledLocationForCLSID(CLSID);

        public static void DoConfigureVideo()
        {
            IntPtr hWnd = WindowHelper.FindWindow("ffdshow_tray");
            if (hWnd != IntPtr.Zero)
                User32.SendMessage(hWnd, MSG_TRAYICON, 1, (int)Messages.WM_LBUTTONDBLCLK);
            else
                ThreadPool.QueueUserWorkItem(_ => RunDll("configure"));
        }

        public static void DoConfigureAudio()
        {
            IntPtr hWnd = WindowHelper.FindWindow("ffdshowaudio_tray");
            if (hWnd != IntPtr.Zero)
                User32.SendMessage(hWnd, MSG_TRAYICON, 1, (int)Messages.WM_LBUTTONDBLCLK);
            else
                ThreadPool.QueueUserWorkItem(_ => RunDll("configureAudio"));
        }

        private static void RunDll(string fncName)
        {
            try
            {
                var runDllPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "rundll32.exe");
                if (!File.Exists(runDllPath))
                    throw new FileNotFoundException(runDllPath);

                Process.Start(new ProcessStartInfo
                {
                    Arguments = $"\"{InstallLocation}\", {fncName}",
                    CreateNoWindow = false,
                    UseShellExecute = false,
                    FileName = runDllPath,
                });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
