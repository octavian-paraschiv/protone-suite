﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OPMedia.UI.Configuration;
using OPMedia.UI.ProTONE.Properties;
using System.Diagnostics;
using OPMedia.Core.Configuration;
using OPMedia.Runtime.ProTONE.Configuration;

namespace OPMedia.UI.ProTONE.Configuration.InternetConfig
{
    public partial class DeezerConfigPage : BaseCfgPanel
    {
        public override Image Image
        {
            get
            {
                return Resources.deezer16;
            }
        }

        public DeezerConfigPage()
        {
            InitializeComponent();
            this.Title = "Deezer";
            this.HandleCreated += new EventHandler(DeezerConfigPage_HandleCreated);

            txtDeezerAppID.InnerTextBox.TextChanged += new EventHandler(OnTextChanged);
            txtDeezerToken.InnerTextBox.TextChanged += new EventHandler(OnTextChanged);
            txtDeezerUserId.InnerTextBox.TextChanged += new EventHandler(OnTextChanged);
            txtDeezerEndpoint.InnerTextBox.TextChanged += new EventHandler(OnTextChanged);
        }

        void OnTextChanged(object sender, EventArgs e)
        {
            base.Modified = true;
        }

        void DeezerConfigPage_HandleCreated(object sender, EventArgs e)
        {
            txtDeezerAppID.Text = ProTONEConfig.DeezerApplicationId;
            txtDeezerToken.Text = ProTONEConfig.DeezerUserAccessToken;
            txtDeezerUserId.Text = ProTONEConfig.DeezerUserId;
            txtDeezerEndpoint.Text = ProTONEConfig.DeezerApiEndpoint;
        }

        protected override void SaveInternal()
        {
            ProTONEConfig.DeezerApplicationId = txtDeezerAppID.Text;
            ProTONEConfig.DeezerUserAccessToken = txtDeezerToken.Text;
            ProTONEConfig.DeezerUserId = txtDeezerUserId.Text;
            ProTONEConfig.DeezerApiEndpoint = txtDeezerEndpoint.Text;
        }

        private void opmLinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://developers.deezer.com/sdk/native#_main_features_and_changes");
        }
    }
}