using OPMedia.Core;
using OPMedia.Core.InstanceManagement;
using OPMedia.Core.Logging;
using System;

namespace OPMedia.PersistenceService
{
    static class Program
    {
        static PersistenceServiceImpl _svc = null;
        static readonly string _appName = Constants.PersistenceServiceShortName;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Start();

            Console.WriteLine("Press any key to exit ...");
            Console.ReadKey();

            Stop();
        }

        private static void Start()
        {
            SingleInstanceApplication.Start(_appName, false);
            Logger.LogInfo($"{_appName} preparing to start ...");

            try
            {
                _svc = new PersistenceServiceImpl();
                Logger.LogInfo($"{_appName} started with success.");
            }
            catch (Exception ex)
            {
                Logger.LogError($"{_appName} failed to start correctly. {ex.Message}");
                Stop();
            }
        }

        private static void Stop()
        {
            Logger.LogInfo($"{_appName} preparing to stop ...");

            try
            {
                _svc?.Dispose();
                _svc = null;

                Logger.LogInfo($"{_appName} stopped with success.");
            }
            catch (Exception ex)
            {
                Logger.LogError($"{_appName} failed to stop correctly. {ex.Message}");
            }

            SingleInstanceApplication.Stop();
        }
    }
}
