using OPMedia.Core;
using OPMedia.Core.Configuration;
using OPMedia.Core.GlobalEvents;
using OPMedia.UI.Properties;
using System;
using System.Drawing;

namespace OPMedia.UI.Configuration
{
    public partial class NetworkCfgPanel : BaseCfgPanel
    {
        public override Image Image
        {
            get
            {
                return Resources.Network;
            }
        }

        public NetworkCfgPanel() : base()
        {
            this.Title = "TXT_S_NETWORK_SETTINGS";
            InitializeComponent();

            ctlProxy.ProxySettings = ProxySettings.Empty;

            this.HandleCreated += new EventHandler(InternetSettingsPanel_Load);
            ctlProxy.OnDataChanged += new EventHandler(ctlProxy_OnDataChanged);
        }

        void ctlProxy_OnDataChanged(object sender, EventArgs e)
        {
            this.Modified = true;
        }

        private void InternetSettingsPanel_Load(object sender, EventArgs e)
        {
            ctlProxy.ProxySettings = AppConfig.ProxySettings;
        }

        protected override void SaveInternal()
        {
            AppConfig.ProxySettings = ctlProxy.ProxySettings;

        }

        [EventSink(EventNames.PerformTranslation)]
        public void PerformTranslation()
        {
            ctlProxy.ProxySettings.PerformTranslation();
        }

    }
}