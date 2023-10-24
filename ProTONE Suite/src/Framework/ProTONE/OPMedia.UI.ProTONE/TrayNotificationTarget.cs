
using OPMedia.Core;
using OPMedia.Core.GlobalEvents;
using OPMedia.Core.Logging;
using OPMedia.UI.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OPMedia.UI.ProTONE
{
    public class TrayNotificationTarget : SelfRegisteredEventSinkObject
    {
        private NotifyIcon _notifyIcon = null;
        private bool _tipVisible = false;
        private string _dispMessage = string.Empty;

        private TrayNotificationBox _box = new TrayNotificationBox();

        public TrayNotificationTarget(NotifyIcon notifyIcon, Form hostForm) : base()
        {
            _notifyIcon = notifyIcon;
        }

        private Image GetTrayToolTipIcon(int icon)
        {
            Image dispIcon = null;

            switch (icon)
            {
                case (int)ToolTipIcon.Warning:
                case (int)MessageBoxIcon.Warning:
                    dispIcon = ImageProvider.GetUser32Icon(User32Icon.Warning, true);
                    break;

                case (int)ToolTipIcon.Error:
                case (int)MessageBoxIcon.Error:
                    dispIcon = ImageProvider.GetUser32Icon(User32Icon.Error, true);
                    break;

                case (int)ToolTipIcon.Info:
                case (int)MessageBoxIcon.Information:
                    dispIcon = ImageProvider.GetUser32Icon(User32Icon.Information, true);
                    break;

                default:
                    dispIcon = ImageProvider.ApplicationIconLarge;
                    break;
            }

            return dispIcon;
        }

        [EventSink(EventNames.ShowTrayMessage)]
        public void ShowTrayMessage(string message, string title, int icon)
        {
            string dispMsg = string.Format("{0}_{1}_{2}", message, title, icon);
            Logger.LogTrace("ShowTrayMessage: oldMsg={0}, msg={1}", _dispMessage ?? "<null>", dispMsg);

            Image dispIcon = GetTrayToolTipIcon(icon);
            if (dispMsg != _dispMessage)
            {
                _dispMessage = dispMsg;

                Dictionary<string, string> d = null;
                if (message != null)
                {
                    d = new Dictionary<string, string>();
                    d.Add(message, string.Empty);
                }

                TrayNotificationBox box = new TrayNotificationBox();
                box.HideDelay = 5000;
                box.FormClosed += Box_FormClosed;
                box.Shown += Box_Shown;

                box.Show(title, d, dispIcon);
            }
        }

        private void Box_Shown(object sender, EventArgs e)
        {
            Logger.LogTrace("Tray message box shown");
        }

        private void Box_FormClosed(object sender, FormClosedEventArgs e)
        {
            Logger.LogTrace("Tray message box closed");
            _dispMessage = null;

            TrayNotificationBox box = sender as TrayNotificationBox;
            if (box != null)
            {
                box.FormClosed -= Box_FormClosed;
                box.Shown -= Box_Shown;
            }
        }
    }
}
