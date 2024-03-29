using OPMedia.Core;
using OPMedia.Core.Logging;
using OPMedia.Runtime.ProTONE.Rendering;
using System;
using System.Text;
using System.Windows.Forms;

namespace OPMedia.Runtime.ProTONE.RemoteControl
{
    [Serializable]
    public class BrowseRemoteFilesCommand : BasicCommand
    {
        public string DirectoryPath
        { get { return (args.Length >= 1) ? args[0] : string.Empty; } }

        public string Filter
        { get { return (args.Length >= 2) ? args[1] : "*.*"; } }

        public BrowseRemoteFilesCommand(string[] args)
            : base(CommandType.BrowseRemoteFiles, args)
        {
            this.requiresAnswer = true;
        }

        public string Execute()
        {
            StringBuilder ans = new StringBuilder();
            ans.AppendLine("ACK");
            foreach (string dir in PathUtils.EnumDirectories(DirectoryPath))
            {
                ans.AppendLine(string.Format("[DIR] {0}", dir));
            }
            foreach (string file in PathUtils.EnumFiles(DirectoryPath, Filter))
            {
                ans.AppendLine(string.Format("[FIL] {0}", file));
            }
            return ans.ToString();
        }
    }

    [Serializable]
    public class GetDriveListCommand : BasicCommand
    {
        public GetDriveListCommand()
            : base(CommandType.GetDriveList, null)
        {
            this.requiresAnswer = true;
        }

        public string Execute()
        {
            StringBuilder ans = new StringBuilder();
            ans.AppendLine("ACK");
            foreach (string drive in Environment.GetLogicalDrives())
            {
                ans.AppendLine(string.Format("[DRV] {0}", drive));
            }
            return ans.ToString();
        }
    }

    [Serializable]
    public class QueryMediaRendererCommand : BasicCommand
    {
        public QueryMediaRendererCommand()
            : base(CommandType.QueryMediaRenderer, null)
        {
            this.requiresAnswer = true;
        }

        public string Execute()
        {
            return "ACK\r\n" + RenderingEngine.DefaultInstance.GetStateDescription();
        }
    }

    [Serializable]
    public class KeyPressCommand : BasicCommand
    {
        string key = string.Empty;

        public KeyPressCommand(string[] args)
            : base(CommandType.KeyPress, args)
        {
            if (args != null && args.Length > 0)
            {
                key = args[0];
            }
        }

        public string Execute()
        {
            try
            {
                IntPtr hWnd = IntPtr.Zero;

                if (MainThread.ModalForm != null)
                    hWnd = MainThread.ModalForm.Handle;
                else
                    hWnd = MainThread.MainWindow.Handle;

                if (User32.SetWindowOnTop(hWnd, true, false))
                {
                    SendKeys.SendWait(key);
                    return "ACK\r\n";
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return "NAK\r\n";
        }
    }
}
