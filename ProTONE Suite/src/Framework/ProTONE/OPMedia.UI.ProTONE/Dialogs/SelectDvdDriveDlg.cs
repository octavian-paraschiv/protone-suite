using System;
using System.Collections.Generic;
using System.Text;
using OPMedia.UI.Themes;
using OPMedia.Runtime.ProTONE.FileInformation;
using System.Windows.Forms;
using OPMedia.Core.TranslationSupport;
using OPMedia.UI.Controls;
using OPMedia.UI.Dialogs;
using System.IO;
using OPMedia.Core;
using System.Drawing;
using OPMedia.Core.Logging;

namespace OPMedia.UI.ProTONE.Dialogs
{
    public class SelectDvdMediaDlg : ToolForm
    {
        private System.Windows.Forms.ListBox lbDvdMedias;
        private OPMButton btnCancel;
        private OPMButton btnOk;
        private System.ComponentModel.IContainer components;
        private OPMLabel label2;
        private OPMButton btnOpenDvdFolder;
        private OPMLabel label1;
        private OPMTableLayoutPanel opmLayoutPanel1;
        private DvdMedia _selectedMedia = null;

        private System.Windows.Forms.Timer tmrUpdateUi;

        public DvdMedia SelectedMedia
        {
            get
            {
                return _selectedMedia;
            }
        }
    
        public SelectDvdMediaDlg()
            : base("TXT_SELECT_DVD")
        {
            InitializeComponent();

            this.AllowResize = false;
            this.InheritAppIcon = false;
            this.Icon = OPMedia.Core.Properties.Resources.DVD.ToIcon();

            this.HandleDestroyed += new EventHandler(OnHandleDestroyed);
            this.Shown += new EventHandler(OnFormShown);
        }

        void OnHandleDestroyed(object sender, EventArgs e)
        {
            if (tmrUpdateUi != null)
            {
                tmrUpdateUi.Stop();
                tmrUpdateUi.Dispose();
                tmrUpdateUi = null;
            }
        }

        void OnFormShown(object sender, EventArgs e)
        {
            RefreshDvdMedias();

            if (tmrUpdateUi == null)
            {
                tmrUpdateUi = new System.Windows.Forms.Timer();
                tmrUpdateUi.Interval = 5000;
                tmrUpdateUi.Tick += new System.EventHandler(this.tmrUpdateUi_Tick);
            }

            tmrUpdateUi.Start();
        }

        private void RefreshDvdMedias()
        {
            List<DvdMedia> dvdDrives = DvdMedia.GetAllDvdMedias();

            do
            {
                if (dvdDrives.Count > lbDvdMedias.Items.Count)
                    lbDvdMedias.Items.Add(new object());

                if (dvdDrives.Count < lbDvdMedias.Items.Count)
                    lbDvdMedias.Items.RemoveAt(lbDvdMedias.Items.Count - 1);
            }
            while (dvdDrives.Count != lbDvdMedias.Items.Count);

            for (int i = 0; i < lbDvdMedias.Items.Count; i++)
            {
                if (lbDvdMedias.Items[i] as DvdMedia != dvdDrives[i])
                    lbDvdMedias.Items[i] = dvdDrives[i];
            }
        }

        private void InitializeComponent()
        {
            this.label1 = new OPMedia.UI.Controls.OPMLabel();
            this.lbDvdMedias = new System.Windows.Forms.ListBox();
            this.btnCancel = new OPMedia.UI.Controls.OPMButton();
            this.btnOk = new OPMedia.UI.Controls.OPMButton();
            this.label2 = new OPMedia.UI.Controls.OPMLabel();
            this.btnOpenDvdFolder = new OPMedia.UI.Controls.OPMButton();
            this.opmLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.pnlContent.SuspendLayout();
            this.opmLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.opmLayoutPanel1);
            this.pnlContent.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.opmLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Location = new System.Drawing.Point(3, 48);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.label1.Name = "label1";
            this.label1.OverrideBackColor = System.Drawing.Color.Empty;
            this.label1.OverrideForeColor = System.Drawing.Color.Empty;
            this.label1.Size = new System.Drawing.Size(462, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "TXT_AVAILABLE_DVD_MEDIA";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbDvdMedias
            // 
            this.lbDvdMedias.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opmLayoutPanel1.SetColumnSpan(this.lbDvdMedias, 2);
            this.lbDvdMedias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDvdMedias.FormattingEnabled = true;
            this.lbDvdMedias.Location = new System.Drawing.Point(3, 66);
            this.lbDvdMedias.Name = "lbDvdMedias";
            this.lbDvdMedias.Size = new System.Drawing.Size(462, 125);
            this.lbDvdMedias.TabIndex = 3;
            this.lbDvdMedias.SelectedIndexChanged += new System.EventHandler(this.lbDvdMedias_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(374, 197);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnCancel.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnCancel.ShowDropDown = false;
            this.btnCancel.Size = new System.Drawing.Size(91, 27);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "TXT_CANCEL";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.AutoSize = true;
            this.btnOk.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Location = new System.Drawing.Point(307, 197);
            this.btnOk.Name = "btnOk";
            this.btnOk.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnOk.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnOk.ShowDropDown = false;
            this.btnOk.Size = new System.Drawing.Size(61, 27);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "TXT_OK";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.OverrideBackColor = System.Drawing.Color.Empty;
            this.label2.OverrideForeColor = System.Drawing.Color.Empty;
            this.label2.Size = new System.Drawing.Size(365, 33);
            this.label2.TabIndex = 0;
            this.label2.Text = "TXT_LOAD_DVD_FOLDER";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnOpenDvdFolder
            // 
            this.btnOpenDvdFolder.AutoSize = true;
            this.btnOpenDvdFolder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOpenDvdFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenDvdFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenDvdFolder.Location = new System.Drawing.Point(374, 3);
            this.btnOpenDvdFolder.Name = "btnOpenDvdFolder";
            this.btnOpenDvdFolder.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnOpenDvdFolder.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnOpenDvdFolder.ShowDropDown = false;
            this.btnOpenDvdFolder.Size = new System.Drawing.Size(91, 27);
            this.btnOpenDvdFolder.TabIndex = 1;
            this.btnOpenDvdFolder.Text = "TXT_BROWSE";
            this.btnOpenDvdFolder.Click += new System.EventHandler(this.btnOpenDvdFolder_Click);
            // 
            // opmLayoutPanel1
            // 
            this.opmLayoutPanel1.ColumnCount = 2;
            this.opmLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmLayoutPanel1.Controls.Add(this.btnOpenDvdFolder, 1, 0);
            this.opmLayoutPanel1.Controls.Add(this.btnOk, 0, 4);
            this.opmLayoutPanel1.Controls.Add(this.btnCancel, 1, 4);
            this.opmLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.opmLayoutPanel1.Controls.Add(this.lbDvdMedias, 0, 3);
            this.opmLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.opmLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.opmLayoutPanel1.Name = "opmLayoutPanel1";
            this.opmLayoutPanel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLayoutPanel1.RowCount = 5;
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmLayoutPanel1.Size = new System.Drawing.Size(468, 227);
            this.opmLayoutPanel1.TabIndex = 0;
            // 
            // SelectDvdMediaDlg
            // 
            this.ClientSize = new System.Drawing.Size(470, 251);
            this.MinimumSize = new System.Drawing.Size(200, 100);
            this.Name = "SelectDvdMediaDlg";
            this.ShowIcon = false;
            this.pnlContent.ResumeLayout(false);
            this.opmLayoutPanel1.ResumeLayout(false);
            this.opmLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void tmrUpdateUi_Tick(object sender, EventArgs e)
        {
            try
            {
                tmrUpdateUi.Stop();
                RefreshDvdMedias();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            finally
            {
                tmrUpdateUi.Start();
            }
        }


        private void btnOpenDvdFolder_Click(object sender, EventArgs e)
        {
            OPMFolderBrowserDialog dlg = new OPMFolderBrowserDialog();
            dlg.ShowNewFolderButton = false;
            dlg.Description = Translator.Translate("TXT_LOAD_DVD_FOLDER");
            dlg.PerformPathValidation += new PerformPathValidationHandler(dlg_PerformPathValidation);

            dlg.InheritAppIcon = false;
            dlg.Icon = this.Icon;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _selectedMedia = DvdMedia.FromPath(dlg.SelectedPath);
                }
                catch 
                {
                    _selectedMedia = null;
                }

                if (_selectedMedia != null)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageDisplay.Show(Translator.Translate("TXT_INVALIDDVDVOLUME"),
                        Translator.Translate("TXT_ERROR"), MessageBoxIcon.Warning);
                }
            }
        }

        bool dlg_PerformPathValidation(string path)
        {
            try
            {
                // Check whether we can build a DVD media from that folder ...
                DvdMedia dvd = DvdMedia.FromPath(path);
                return (dvd != null);
            }
            catch
            {
            }

            return false;
        }

        private void lbDvdMedias_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedMedia = lbDvdMedias.SelectedItem as DvdMedia;
        }
    }
}
