using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using OPMedia.Core;


namespace OPMedia.GalaxyService
{
    [RunInstaller(true)]
    public partial class GalaxyServiceInstaller : System.Configuration.Install.Installer
    {
        public GalaxyServiceInstaller()
        {
            InitializeComponent();

            ServiceInstaller serviceInstaller = new ServiceInstaller();
            serviceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = Constants.GalaxyServiceLongName;
            serviceInstaller.DisplayName = Constants.GalaxyServiceLongName;
            serviceInstaller.Description = Constants.GalaxyServiceDescription;

            Installers.Add(serviceInstaller);

            ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;

            Installers.Add(serviceProcessInstaller);
        }
    }
}
