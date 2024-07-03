using OPMedia.Core;
using System.ComponentModel;
using System.ServiceProcess;


namespace OPMedia.PersistenceService
{
    [RunInstaller(true)]
    public partial class PersistenceServiceInstaller : System.Configuration.Install.Installer
    {
        public PersistenceServiceInstaller()
        {
            InitializeComponent();

            ServiceInstaller serviceInstaller = new ServiceInstaller();
            serviceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = Constants.PersistenceServiceShortName;
            serviceInstaller.DisplayName = Constants.PersistenceServiceLongName;
            serviceInstaller.Description = Constants.PersistenceServiceDescription;

            Installers.Add(serviceInstaller);

            ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;

            Installers.Add(serviceProcessInstaller);
        }
    }
}
