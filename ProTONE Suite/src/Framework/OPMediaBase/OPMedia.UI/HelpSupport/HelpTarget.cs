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
            // Help base URI should be something like:
            // http://ocpa.ro/api/wiki/protone/{version}/{appName}/{section}/{topic}.md

            if (string.IsNullOrEmpty(sectionName))
                sectionName = "default";

            string helpUri = $"{AppConfig.HelpUriBase}/{ApplicationInfo.ApplicationName}/{sectionName}";

            if (topicName?.Length > 0)
                helpUri += $"/{topicName}";

            if (!helpUri.EndsWith(".md", System.StringComparison.OrdinalIgnoreCase))
                helpUri += ".md";

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
