using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OPMedia.Core.Logging;
using OPMedia.Core.InstanceManagement;
using OPMedia.Runtime.ProTONE.RemoteControl;
using OPMedia.Core.TranslationSupport;

using OPMedia.Runtime.Shortcuts;
using OPMedia.Core.Configuration;
using OPMedia.UI.ProTONE.Controls.MediaPlayer;
using OPMedia.Runtime.ProTONE;
using OPMedia.Runtime.ProTONE.Configuration;

using OPMedia.Runtime.ProTONE.Rendering;

namespace OPMedia.ProTONE
{
    static class Program
    {
        static MainForm mainFrm;

        static List<BasicCommand> _commandQueue = new List<BasicCommand>();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                if (!ProcessCommandLine(true))
                {
                    try
                    {
                        RemoteControllableApplication.Start(Core.Constants.PlayerName, true);

                        ProcessCommandLine(false);

                        Translator.SetInterfaceLanguage(AppConfig.LanguageID);
                        Translator.RegisterTranslationAssembly(typeof(MediaPlayer).Assembly);
                        Translator.RegisterTranslationAssembly(typeof(MainForm).Assembly);

                        ShortcutMapper.IsPlayer = true;

                        // Eager detection of multimedia audio devices
                        RenderingEngine.DefaultInstance.CheckMMDevice();

                        mainFrm = new MainForm();

                        foreach (BasicCommand cmd in _commandQueue)
                        {
                            mainFrm.EnqueueCommand(cmd);
                        }

                        Application.Run(mainFrm);
                        mainFrm.Dispose();

                        ShortcutMapper.Save();
                    }
                    catch (MultipleInstancesException ex)
                    {
                        Logger.LogWarning(ex.Message);

                        // Send an activate command to the main instance
                        PlayerRemoteControl.SendPlayerCommand(CommandType.Activate, null);
                    }
                    catch (Exception ex)
                    {
                        ErrorDispatcher.DispatchFatalError(ex);
                    }
                    finally
                    {
                        RemoteControllableApplication.Stop();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorDispatcher.DispatchFatalError(ex);
            }
        }

        public static bool ProcessCommandLine(bool testForShellExec)
        {
            string[] cmdLineArgs = Environment.GetCommandLineArgs();

            if (testForShellExec && cmdLineArgs.Length > 1 && cmdLineArgs[1].ToLowerInvariant() == "launch")
            {
                List<string> files = new List<string>();
                for (int i = 2; i < cmdLineArgs.Length; i++)
                {
                    files.Add(cmdLineArgs[i]);
                }

                try
                {
                    CommandType cmdType = (CommandType)Enum.Parse(typeof(CommandType),
                        ProTONEConfig.ExplorerLaunchType);

                    if (cmdType == CommandType.PlayFiles || cmdType == CommandType.EnqueueFiles)
                    {
                        if (PlayerRemoteControl.IsPlayerRunning())
                        {
                            // There is another player instance that is running.
                            // Just pass the command to that instance and exit.
                            PlayerRemoteControl.SendPlayerCommand(cmdType, files.ToArray());
                        }
                        else
                        {
                            // There is no other player instance. 
                            // This instance needs to process the command itself.
                            
                            // Note: when player is launched like this - clear previous playlist first.
                            _commandQueue.Add(BasicCommand.Create(CommandType.ClearPlaylist));
                            
                            _commandQueue.Add(BasicCommand.Create(cmdType, files.ToArray()));

                            return false; // Don't exit
                        }
                    }
                }
                catch(Exception ex)
                {
                    ErrorDispatcher.DispatchError(ex, false);
                }

                return true;
            }

            return false;
        }

    }
}