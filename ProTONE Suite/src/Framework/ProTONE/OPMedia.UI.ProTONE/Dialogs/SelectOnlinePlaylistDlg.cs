using System;
using System.Collections.Generic;
using System.Text;
using OPMedia.UI.Themes;
using OPMedia.Runtime.ProTONE.FileInformation;
using System.Windows.Forms;
using OPMedia.Core.TranslationSupport;
using OPMedia.Runtime.ProTONE.Playlists;
using OPMedia.UI.Controls;
using System.Threading;
using OPMedia.Core;
using OPMedia.Runtime.ProTONE.OnlineMediaContent;


namespace OPMedia.UI.ProTONE.Dialogs
{
    public class SelectOnlinePlaylistDlg : ToolForm
    {
        private OPMButton btnCancel;
        private OPMButton btnOk;
        private ListBox lbPlaylists;
        private OPMLabel label1;
        private OPMTableLayoutPanel opmLayoutPanel1;

        public OnlinePlaylist SelectedItem
        {
            get { return lbPlaylists.SelectedItem as OnlinePlaylist; }
        }
        
        public SelectOnlinePlaylistDlg(List<OnlinePlaylist> playlists)
            : base("TXT_SELECT_ONLINE_PLAYLIST")
        {
            InitializeComponent();

            lbPlaylists.DisplayMember = "Title";
            ThemeManager.SetFont(lbPlaylists, FontSizes.Large);

            playlists.ForEach((p) => lbPlaylists.Items.Add(p));
        }

        private void InitializeComponent()
        {
            this.btnCancel = new OPMedia.UI.Controls.OPMButton();
            this.btnOk = new OPMedia.UI.Controls.OPMButton();
            this.lbPlaylists = new System.Windows.Forms.ListBox();
            this.label1 = new OPMedia.UI.Controls.OPMLabel();
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
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(197, 305);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0, 0, 5, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnCancel.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnCancel.ShowDropDown = false;
            this.btnCancel.Size = new System.Drawing.Size(90, 38);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "TXT_CANCEL";
            // 
            // btnOk
            // 
            this.btnOk.AutoSize = true;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Location = new System.Drawing.Point(120, 305);
            this.btnOk.Margin = new System.Windows.Forms.Padding(0, 0, 5, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnOk.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnOk.ShowDropDown = false;
            this.btnOk.Size = new System.Drawing.Size(72, 38);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "TXT_OK";
            // 
            // lbPlaylists
            // 
            this.opmLayoutPanel1.SetColumnSpan(this.lbPlaylists, 3);
            this.lbPlaylists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPlaylists.FormattingEnabled = true;
            this.lbPlaylists.Location = new System.Drawing.Point(5, 25);
            this.lbPlaylists.Margin = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.lbPlaylists.Name = "lbPlaylists";
            this.lbPlaylists.Size = new System.Drawing.Size(282, 275);
            this.lbPlaylists.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.opmLayoutPanel1.SetColumnSpan(this.label1, 3);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.OverrideBackColor = System.Drawing.Color.Empty;
            this.label1.OverrideForeColor = System.Drawing.Color.Empty;
            this.label1.Size = new System.Drawing.Size(282, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "TXT_SELECT_PLAYLIST";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // opmLayoutPanel1
            // 
            this.opmLayoutPanel1.ColumnCount = 3;
            this.opmLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmLayoutPanel1.Controls.Add(this.btnCancel, 2, 4);
            this.opmLayoutPanel1.Controls.Add(this.lbPlaylists, 0, 3);
            this.opmLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.opmLayoutPanel1.Controls.Add(this.btnOk, 1, 4);
            this.opmLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.opmLayoutPanel1.Name = "opmLayoutPanel1";
            this.opmLayoutPanel1.RowCount = 5;
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.opmLayoutPanel1.Size = new System.Drawing.Size(292, 348);
            this.opmLayoutPanel1.TabIndex = 6;
            // 
            // SelectOnlinePlaylistDlg
            // 
            this.ClientSize = new System.Drawing.Size(296, 374);
            this.MinimumSize = new System.Drawing.Size(200, 85);
            this.Name = "SelectOnlinePlaylistDlg";
            this.ShowIcon = false;
            this.pnlContent.ResumeLayout(false);
            this.opmLayoutPanel1.ResumeLayout(false);
            this.opmLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        
    }
}
