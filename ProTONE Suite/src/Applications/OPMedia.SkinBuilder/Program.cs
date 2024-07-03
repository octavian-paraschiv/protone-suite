using OPMedia.Core.Logging;
using OPMedia.Core.TranslationSupport;
using System;
using System.Windows.Forms;

namespace OPMedia.SkinBuilder
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
                LoggedApplication.Start("SkinBuilder", false);

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

                Translator.SetInterfaceLanguage("en"); // Only English

                Application.Run(new SkinBuilderForm());


            }
            catch (Exception ex)
            {
                ErrorDispatcher.DispatchError(ex, false);
            }
            finally
            {
                LoggedApplication.Stop();
            }
        }
    }
}
