using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPMedia.Runtime.ServiceHelpers;
using System.ServiceModel;
using OPMedia.Core;
using OPMedia.Core.Configuration;
using OPMedia.Runtime.ProTONE.Galaxy;
using System.ServiceModel.Description;

namespace OPMedia.GalaxyService
{
    internal class ServiceHelper : ServiceHelperBase
    {
        ServiceHost _host = null;

        public ServiceHelper()
        {

        }

        protected override void StartInternal()
        {
            Environment.CurrentDirectory = LiteAppConfig.InstallationPath;

            var binding = new WebHttpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;

            binding.OpenTimeout = TimeSpan.FromSeconds(4);
            binding.CloseTimeout = TimeSpan.FromSeconds(4);
            binding.SendTimeout = TimeSpan.FromMilliseconds(500);
            binding.ReceiveTimeout = TimeSpan.FromSeconds(30);

            _host = new ServiceHost(typeof(GalaxyServiceImpl));
            
            var endpoint = _host.AddServiceEndpoint(typeof(IGalaxyService), binding, GalaxyConstants.GalaxyServiceAddress);
            endpoint.Behaviors.Add(new WebHttpBehavior());

            _host.Open();
        }

        protected override void StopInternal()
        {
            _host.Close();
            _host = null;
        }
    }
}
