using OPMedia.Core;
using OPMedia.Core.Logging;
using System;
using System.Diagnostics;
using System.ServiceProcess;

namespace OPMedia.PersistenceService
{
    public partial class PersistenceService : ServiceBase
    {
        PersistenceServiceImpl _svc = null;

        public PersistenceService()
        {
            InitializeComponent();
            this.ServiceName = Constants.PersistenceServiceShortName;
        }

        [STAThread]
        static void Main()
        {
            if (Environment.UserInteractive)
            {
                Debug.Listeners.Add(new ConsoleTraceListener(false));

                // Start as stand-alone app
                new PersistenceService().RunStandAlone(Environment.GetCommandLineArgs());
            }
            else
            {
                // Start as Windows Service
                Run(new PersistenceService());
            }
        }

        public void RunStandAlone(string[] args)
        {
            OnStart(args);

            Console.WriteLine("Press any key to exit ...");
            Console.ReadKey();

            OnStop();
        }


        protected override void OnStart(string[] args)
        {
            LoggedApplication.Start(this.ServiceName, false);
            Logger.LogInfo("Service preparing to start ...");

            try
            {
                _svc = new PersistenceServiceImpl();
                Logger.LogInfo("Service started with success.");
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Service failed to start correctly. {0}", ex.Message);
                Stop();
            }
        }

        protected override void OnStop()
        {
            Logger.LogInfo("Service preparing to stop ...");

            try
            {
                _svc?.Dispose();
                _svc = null;
                Logger.LogInfo("Service stopped with success.");
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Service failed to stop correctly. {0}", ex.Message);
            }

            LoggedApplication.Stop();
        }
    }
}
