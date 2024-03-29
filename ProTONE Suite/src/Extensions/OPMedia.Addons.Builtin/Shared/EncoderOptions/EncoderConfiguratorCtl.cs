﻿using OPMedia.UI.Controls;
using System;

namespace OPMedia.Addons.Builtin.Shared.EncoderOptions
{
    public partial class EncoderConfiguratorCtl : OPMBaseControl
    {
        public bool UsedForCdRipper { get; set; }

        public EncoderSettings EncoderSettings { get; set; }

        public event EventHandler SettingsChanged = null;
        public void FireSettingsChanged()
        {
            if (SettingsChanged != null)
                SettingsChanged(this, EventArgs.Empty);
        }

        public EncoderConfiguratorCtl()
        {
            InitializeComponent();
        }

        public EncoderConfiguratorCtl(EncoderSettings settings)
        {
            InitializeComponent();
            this.EncoderSettings = settings;
        }

        internal virtual void Reload()
        {
        }

    }
}
