using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPMedia.Runtime.ServiceHelpers;
using System.ServiceModel;
using OPMedia.Core;

using OPMedia.Core.Configuration;
using System.ServiceModel.Security;
using System.IdentityModel.Selectors;
using System.Security.Cryptography.X509Certificates;

namespace OPMedia.PersistenceService
{
    internal class ServiceHelper : ServiceHelperBase
    {
        ServiceHost _host = null;

        public ServiceHelper()
        {

        }

        protected override void StartInternal()
        {
            Environment.CurrentDirectory = AppConfig.InstallationPath;

            var binding = new NetTcpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;

            binding.OpenTimeout = TimeSpan.FromSeconds(4);
            binding.CloseTimeout = TimeSpan.FromSeconds(4);
            binding.SendTimeout = TimeSpan.FromMilliseconds(500);
            binding.ReceiveTimeout = TimeSpan.FromSeconds(4);

            _host = new ServiceHost(typeof(PersistenceServiceImpl));
            _host.AddServiceEndpoint(typeof(IPersistenceService), binding, PersistenceConstants.PersistenceServiceAddress);

            _host.Open();
        }

        protected override void StopInternal()
        {
            _host.Close();
            _host = null;
        }
    }
}
