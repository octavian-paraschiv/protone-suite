using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPMedia.Core.TranslationSupport;
using OPMedia.UI.Themes;
using OPMedia.Runtime.Addons;
using OPMedia.UI.ProTONE.Configuration;
using System.Windows.Forms;
using OPMedia.Core.Configuration;
using OPMedia.Core.Logging;
using OPMedia.Core;
using OPMedia.UI.ProTONE.Controls.MediaPlayer;
using OPMedia.Runtime.Addons.Configuration;
using OPMedia.Runtime.Addons.AddonsBase;
using System.Diagnostics;
using OPMedia.Runtime.ProTONE.SubtitleDownload;
using OPMedia.Runtime.ProTONE;

using OPMedia.Runtime.ProTONE.Rendering;

namespace OPMedia.MediaLibrary
{
    static class Program
    {
        public static string LaunchPath { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            LaunchPath = string.Empty;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                LoggedApplication.Start(Constants.LibraryName, true);

                string[] cmdLineArgs = Environment.GetCommandLineArgs();

                if (cmdLineArgs.Length > 2)
                {
                    switch (cmdLineArgs.Length)
                    {
                        case 3:
                            switch (cmdLineArgs[1].ToLowerInvariant())
                            {
                                case "launch":
                                    LaunchPath = cmdLineArgs[2];
                                    break;
                            }
                            break;
                    }
                }

                string cmdLine = Environment.CommandLine.ToLowerInvariant();
                if (cmdLineArgs != null && cmdLineArgs.Length > 2 && cmdLine.Contains("configaddons"))
                {
                    Translator.SetInterfaceLanguage(cmdLineArgs[2]);

                    AddonsConfig.IsInitialConfig = true;
                    AppConfig.SkinType = string.Empty;

                    try
                    {
                        AddonAppSettingsForm.Show("TXT_S_ADDONSETTINGS");
                    }
                    finally
                    {
                    }
                }
                else
                {
                    Translator.SetInterfaceLanguage(AppConfig.LanguageID);

                    // Eager detection of multimedia audio devices
                    RenderingEngine.DefaultInstance.CheckMMDevice();

                    Application.Run(new MediaLibraryForm());
                }
            }
            catch (Exception ex)
            {
                ErrorDispatcher.DispatchFatalError(ex);
            }
            finally
            {
                LoggedApplication.Stop();
            }
        }
    }
}
