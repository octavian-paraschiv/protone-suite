﻿using OPMedia.Core;
using OPMedia.Core.Utilities;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.Rendering;
using OPMedia.UI.Configuration;
using OPMedia.UI.Controls;
using OPMedia.UI.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using LocalEventNames = OPMedia.UI.ProTONE.GlobalEvents.EventNames;

namespace OPMedia.UI.ProTONE.Configuration.MiscConfig
{
    public partial class PlaylistOptionsPage : BaseCfgPanel
    {
        readonly string[] DefaultFormats = new string[]
        {
            "<A> - <T>",
            "<#> <A> - <T>"
        };

        public PlaylistOptionsPage()
        {
            InitializeComponent();

            lblDisplayFileName.OverrideBackColor = ThemeManager.ColorValidationFailed;

            chkFileNameFormat.Checked = ProTONEConfig.UseFileNameFormat;
            chkUseMetadata.Checked = ProTONEConfig.UseMetadata;

            cmbProfile.DataSource = Enum.GetValues(typeof(XFadeProfiles)).OfType<XFadeProfiles>().ToList();
            cmbProfile.SelectedItem = RenderingEngine.DefaultInstance.XFade_OperationalProfile;

            PopulatePlaylistEntryFormats(true);
            PopulateFileNameFormats(true);
            PopulatePlaylistOptions();

            EnableDisableFields();

            this.chkUseMetadata.CheckedChanged += new System.EventHandler(this.chkUseMetadata_CheckedChanged);
            this.cmbPlaylistEntryFormat.SelectedIndexChanged += new System.EventHandler(this.OnPlaylistEntryFormatChanged);
            this.cmbPlaylistEntryFormat.TextUpdate += new System.EventHandler(this.OnPlaylistEntryFormatChanged);
            this.cmbFileNameFormat.SelectedIndexChanged += new System.EventHandler(this.OnFileNameFormatChanged);
            this.cmbFileNameFormat.TextChanged += new System.EventHandler(this.OnFileNameFormatChanged);
            this.chkFileNameFormat.CheckedChanged += new System.EventHandler(this.chkFileNameFormat_CheckedChanged);

            this.cmbProfile.SelectedIndexChanged += OnPlaylistSettingsChanged;

            this.chkLoopPlay.CheckedChanged += OnPlaylistSettingsChanged;
            this.chkXFade.CheckedChanged += OnPlaylistSettingsChanged;
            this.nudAnticipatedEnd.ValueChanged += OnPlaylistSettingsChanged;
            this.nudXFadeLength.ValueChanged += OnPlaylistSettingsChanged;
        }

        private void OnPlaylistSettingsChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        private void chkUseMetadata_CheckedChanged(object sender, EventArgs e)
        {
            EnableDisableFields();
            Modified = true;
        }

        private void EnableDisableFields()
        {
            //cmbPlaylistEntryFormat.Enabled = chkUseMetadata.Checked;
            cmbFileNameFormat.Enabled = chkFileNameFormat.Checked;

            bool formatting = chkUseMetadata.Checked || chkFileNameFormat.Checked;

            lblDisplayFileName.Visible = !formatting;
            cmbPlaylistEntryFormat.Visible = formatting;
            lblPlaylistFormat.Visible = formatting;
            txtHints.Visible = formatting;
        }

        private void chkFileNameFormat_CheckedChanged(object sender, EventArgs e)
        {
            EnableDisableFields();
            Modified = true;
        }

        private void OnClearMetadataHistory(object sender, EventArgs e)
        {
            PopulatePlaylistEntryFormats(false);
            Modified = true;
        }

        private void OnClearFileNameFormat(object sender, EventArgs e)
        {
            PopulateFileNameFormats(false);
            Modified = true;
        }

        private void OnPlaylistEntryFormatChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        private void OnFileNameFormatChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        private void PopulateFileNameFormats(bool fillCustomFormats)
        {
            cmbFileNameFormat.Items.Clear();
            cmbFileNameFormat.Items.AddRange(DefaultFormats);

            if (fillCustomFormats)
            {
                string[] customFormats = StringUtils.ToStringArray(ProTONEConfig.CustomFileNameFormats, '?');
                if (customFormats != null)
                {
                    cmbFileNameFormat.Items.AddRange(customFormats);
                }

                cmbFileNameFormat.SelectedIndex =
                    cmbFileNameFormat.FindStringExact(ProTONEConfig.FileNameFormat);
            }
            else
            {
                cmbFileNameFormat.SelectedIndex = 0;
            }
        }

        private void PopulatePlaylistOptions()
        {
            chkLoopPlay.Checked = ProTONEConfig.LoopPlay;
            chkXFade.Checked = ProTONEConfig.XFade;
            nudXFadeLength.Value = ProTONEConfig.XFadeLength;
            nudAnticipatedEnd.Value = ProTONEConfig.XFadeAnticipationPercentage;
        }

        private void PopulatePlaylistEntryFormats(bool fillCustomFormats)
        {
            cmbPlaylistEntryFormat.Items.Clear();
            cmbPlaylistEntryFormat.Items.AddRange(DefaultFormats);

            if (fillCustomFormats)
            {
                string[] customFormats = StringUtils.ToStringArray(ProTONEConfig.CustomPlaylistEntryFormats, '?');
                if (customFormats != null)
                {
                    cmbPlaylistEntryFormat.Items.AddRange(customFormats);
                }

                cmbPlaylistEntryFormat.SelectedIndex =
                    cmbPlaylistEntryFormat.FindStringExact(ProTONEConfig.PlaylistEntryFormat);
            }
            else
            {
                cmbPlaylistEntryFormat.SelectedIndex = 0;
            }
        }

        private void AddToPlaylistEntryFormatHistory(string format)
        {
            if (!cmbPlaylistEntryFormat.Items.Contains(format))
            {
                cmbPlaylistEntryFormat.Items.Add(format);
            }
        }

        private void AddToFileNameFormatHistory(string format)
        {
            if (!cmbFileNameFormat.Items.Contains(format))
            {
                cmbFileNameFormat.Items.Add(format);
            }
        }

        protected override void SaveInternal()
        {
            AddToPlaylistEntryFormatHistory(cmbPlaylistEntryFormat.Text);
            AddToFileNameFormatHistory(cmbFileNameFormat.Text);

            ProTONEConfig.UseFileNameFormat = chkFileNameFormat.Checked;
            ProTONEConfig.UseMetadata = chkUseMetadata.Checked;

            ProTONEConfig.PlaylistEntryFormat = cmbPlaylistEntryFormat.Text;
            ProTONEConfig.FileNameFormat = cmbFileNameFormat.Text;

            string[] customPlaylistEntryFormats = GetCustomFormats(cmbPlaylistEntryFormat);
            string[] customFileNameFormats = GetCustomFormats(cmbFileNameFormat);

            ProTONEConfig.CustomPlaylistEntryFormats =
                StringUtils.FromStringArray(customPlaylistEntryFormats, '?');
            ProTONEConfig.CustomFileNameFormats =
                StringUtils.FromStringArray(customFileNameFormats, '?');

            ProTONEConfig.LoopPlay = chkLoopPlay.Checked;
            ProTONEConfig.XFade = chkXFade.Checked;
            ProTONEConfig.XFadeLength = (int)nudXFadeLength.Value;
            ProTONEConfig.XFadeAnticipationPercentage = (int)nudAnticipatedEnd.Value;
            RenderingEngine.DefaultInstance.XFade_OperationalProfile = (XFadeProfiles)cmbProfile.SelectedItem;

            EventDispatch.DispatchEvent(LocalEventNames.UpdatePlaylistNames, false);
            EventDispatch.DispatchEvent(LocalEventNames.UpdateStateButtons);
        }

        private string[] GetCustomFormats(OPMEditableComboBox cmb)
        {
            List<String> formats = new List<string>();
            foreach (string item in cmb.Items)
            {
                if (!DefaultFormats.Contains(item))
                {
                    formats.Add(item);
                }
            }

            return formats.ToArray();
        }
    }
}
