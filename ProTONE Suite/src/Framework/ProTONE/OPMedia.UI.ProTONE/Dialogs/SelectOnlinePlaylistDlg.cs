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
        private OPMLabel lblNewName;
        private OPMTextBox txtNewName;
        private OPMTableLayoutPanel opmLayoutPanel1;

        public OnlinePlaylist SelectedItem
        {
            get
            {
                var pl = lbPlaylists.SelectedItem as OnlinePlaylist;

                if (pl != null && lbPlaylists.SelectedIndex == 0)
                {
                    pl.Title = txtNewName.Text;
                }

                return pl;
            }
        }
        
        public SelectOnlinePlaylistDlg(List<OnlinePlaylist> playlists)
            : base("TXT_SELECT_ONLINE_PLAYLIST")
        {
            InitializeComponent();

            lbPlaylists.DisplayMember = "Title";
            ThemeManager.SetFont(lbPlaylists, FontSizes.Large);

            playlists.ForEach((p) => lbPlaylists.Items.Add(p));

            int sel = (lbPlaylists.Items.Count > 0) ? 0 : -1;
            lbPlaylists.SelectedIndex = sel;
        }

        private void InitializeComponent()
        {
            this.btnCancel = new OPMedia.UI.Controls.OPMButton();
            this.btnOk = new OPMedia.UI.Controls.OPMButton();
            this.lbPlaylists = new System.Windows.Forms.ListBox();
            this.label1 = new OPMedia.UI.Controls.OPMLabel();
            this.opmLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.lblNewName = new OPMedia.UI.Controls.OPMLabel();
            this.txtNewName = new OPMedia.UI.Controls.OPMTextBox();
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
            this.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(319, 301);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0, 0, 5, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnCancel.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnCancel.ShowDropDown = false;
            this.btnCancel.Size = new System.Drawing.Size(90, 27);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "TXT_CANCEL";
            // 
            // btnOk
            // 
            this.btnOk.AutoSize = true;
            this.btnOk.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Location = new System.Drawing.Point(253, 301);
            this.btnOk.Margin = new System.Windows.Forms.Padding(0, 0, 5, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnOk.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnOk.ShowDropDown = false;
            this.btnOk.Size = new System.Drawing.Size(61, 27);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "TXT_OK";
            // 
            // lbPlaylists
            // 
            this.opmLayoutPanel1.SetColumnSpan(this.lbPlaylists, 4);
            this.lbPlaylists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPlaylists.FormattingEnabled = true;
            this.lbPlaylists.Location = new System.Drawing.Point(5, 25);
            this.lbPlaylists.Margin = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.lbPlaylists.Name = "lbPlaylists";
            this.lbPlaylists.Size = new System.Drawing.Size(404, 244);
            this.lbPlaylists.TabIndex = 3;
            this.lbPlaylists.SelectedIndexChanged += new System.EventHandler(this.lbPlaylists_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.opmLayoutPanel1.SetColumnSpan(this.label1, 4);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.OverrideBackColor = System.Drawing.Color.Empty;
            this.label1.OverrideForeColor = System.Drawing.Color.Empty;
            this.label1.Size = new System.Drawing.Size(404, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "TXT_SELECT_PLAYLIST";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // opmLayoutPanel1
            // 
            this.opmLayoutPanel1.ColumnCount = 4;
            this.opmLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmLayoutPanel1.Controls.Add(this.btnCancel, 3, 5);
            this.opmLayoutPanel1.Controls.Add(this.lbPlaylists, 0, 3);
            this.opmLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.opmLayoutPanel1.Controls.Add(this.btnOk, 2, 5);
            this.opmLayoutPanel1.Controls.Add(this.lblNewName, 0, 4);
            this.opmLayoutPanel1.Controls.Add(this.txtNewName, 1, 4);
            this.opmLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.opmLayoutPanel1.Name = "opmLayoutPanel1";
            this.opmLayoutPanel1.RowCount = 6;
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.opmLayoutPanel1.Size = new System.Drawing.Size(414, 333);
            this.opmLayoutPanel1.TabIndex = 6;
            // 
            // lblNewName
            // 
            this.lblNewName.AutoSize = true;
            this.lblNewName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNewName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblNewName.Location = new System.Drawing.Point(0, 274);
            this.lblNewName.Margin = new System.Windows.Forms.Padding(0, 0, 3, 5);
            this.lblNewName.Name = "lblNewName";
            this.lblNewName.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblNewName.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblNewName.Size = new System.Drawing.Size(155, 22);
            this.lblNewName.TabIndex = 6;
            this.lblNewName.Text = "TXT_NEW_PLAYLIST_NAME:";
            this.lblNewName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNewName
            // 
            this.txtNewName.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.opmLayoutPanel1.SetColumnSpan(this.txtNewName, 3);
            this.txtNewName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNewName.FontSize = OPMedia.UI.Themes.FontSizes.NormalBold;
            this.txtNewName.Lines = new string[0];
            this.txtNewName.Location = new System.Drawing.Point(158, 274);
            this.txtNewName.Margin = new System.Windows.Forms.Padding(0, 0, 5, 5);
            this.txtNewName.MaximumSize = new System.Drawing.Size(3000, 22);
            this.txtNewName.MaxLength = 32767;
            this.txtNewName.MinimumSize = new System.Drawing.Size(22, 22);
            this.txtNewName.Name = "txtNewName";
            this.txtNewName.OverrideBackColor = System.Drawing.Color.Transparent;
            this.txtNewName.OverrideForeColor = System.Drawing.Color.Empty;
            this.txtNewName.PasswordChar = '\0';
            this.txtNewName.ReadOnly = false;
            this.txtNewName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtNewName.ShortcutsEnabled = true;
            this.txtNewName.Size = new System.Drawing.Size(251, 22);
            this.txtNewName.TabIndex = 7;
            this.txtNewName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtNewName.UseSystemPasswordChar = false;
            this.txtNewName.WordWrap = false;
            // 
            // SelectOnlinePlaylistDlg
            // 
            this.ClientSize = new System.Drawing.Size(418, 359);
            this.MinimumSize = new System.Drawing.Size(200, 85);
            this.Name = "SelectOnlinePlaylistDlg";
            this.ShowIcon = false;
            this.pnlContent.ResumeLayout(false);
            this.opmLayoutPanel1.ResumeLayout(false);
            this.opmLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void lbPlaylists_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool addNew = (lbPlaylists.SelectedIndex == 0);
            lblNewName.Visible = txtNewName.Visible = addNew;
            if (addNew)
            {
                txtNewName.Select();
                txtNewName.Focus();
            }
        }
    }
}
