using OPMedia.Core;
using OPMedia.Core.Configuration;
using OPMedia.Core.Logging;
using System.Windows.Forms;

namespace OPMedia.UI.HelpSupport
{
    public static class HelpTarget
    {
        static HelpViewer _helpViewer = null;

        public static void HelpRequest(string sectionName, string topicName = "")
        {
            string helpUri = string.Empty;

            // Help base URI should be something like:
            // https://raw.githubusercontent.com/octavian-paraschiv/protone-suite-docs/master/#VERSION#/

            if (string.IsNullOrEmpty(topicName))
            {
                helpUri = string.Format("{0}/{1}/{2}.htm",
                    AppConfig.HelpUriBase, ApplicationInfo.ApplicationName,
                    sectionName);
            }
            else
            {
                helpUri = string.Format("{0}/{1}/{2}/{3}.htm",
                    AppConfig.HelpUriBase, ApplicationInfo.ApplicationName,
                       sectionName, topicName);
            }

            Logger.LogHelpTrace(helpUri);

            if (_helpViewer == null)
            {
                _helpViewer = new HelpViewer();
                _helpViewer.Show();
                _helpViewer.FormClosed += new FormClosedEventHandler(_helpViewer_FormClosed);
            }

            _helpViewer.OpenURL(helpUri);
        }

        static void _helpViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            _helpViewer.Dispose();
            _helpViewer = null;
        }

    }
}
