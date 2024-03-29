using OPMedia.Core.TranslationSupport;
using OPMedia.Runtime.ProTONE.OnlineMediaContent;
using OPMedia.UI.Controls;
using OPMedia.UI.Themes;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace OPMedia.UI.ProTONE.Dialogs
{
    public class SelectOnlinePlaylistDlg : ToolForm
    {
        private OPMButton btnCancel;
        private OPMButton btnOk;
        private ListBox lbPlaylists;
        private OPMLabel lblDesc;
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

            if (playlists != null)
            {
                bool addToPlaylist = (playlists.Count > 0 && playlists[0].Id <= 0);
                if (addToPlaylist)
                    lblDesc.Text = Translator.Translate("TXT_SELECT_PLAYLIST_OR_NEW:");

                playlists.ForEach((p) => lbPlaylists.Items.Add(p));
                this.Shown += (s, e) =>
                {
                    int sel = (lbPlaylists.Items.Count > 0) ? 0 : -1;
                    lbPlaylists.SelectedIndex = sel;
                };

                txtNewName.TextChanged += (s, e) => ActivateOKButton();

                lbPlaylists.MouseDoubleClick += (s, e) =>
                {
                    if (IsSelectionValid())
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                };
            }
        }

        private void ActivateOKButton()
        {
            btnOk.Enabled = IsSelectionValid();
        }

        protected override bool AllowCloseOnKeyDown(Keys keyDown)
        {
            if (!IsSelectionValid() && keyDown == Keys.Enter)
                return false;

            return base.AllowCloseOnKeyDown(keyDown);
        }

        private bool IsSelectionValid()
        {
            var sel = lbPlaylists.SelectedItem as OnlinePlaylist;
            bool valid = (sel != null);

            if (valid && sel.Id <= 0)
            {
                valid &= (string.IsNullOrEmpty(txtNewName.Text) == false);
                valid &= (txtNewName.Text.StartsWith("_") == false);
            }

            return valid;
        }

        private void InitializeComponent()
        {
            this.btnCancel = new OPMedia.UI.Controls.OPMButton();
            this.btnOk = new OPMedia.UI.Controls.OPMButton();
            this.lbPlaylists = new System.Windows.Forms.ListBox();
            this.lblDesc = new OPMedia.UI.Controls.OPMLabel();
            this.opmLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.lblNewName = new OPMedia.UI.Controls.OPMLabel();
            this.txtNewName = new OPMedia.UI.Controls.OPMTextBox();
            this.opmLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(326, 327);
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
            this.btnOk.Location = new System.Drawing.Point(260, 327);
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
            this.lbPlaylists.Size = new System.Drawing.Size(411, 270);
            this.lbPlaylists.TabIndex = 3;
            this.lbPlaylists.SelectedIndexChanged += new System.EventHandler(this.lbPlaylists_SelectedIndexChanged);
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.opmLayoutPanel1.SetColumnSpan(this.lblDesc, 4);
            this.lblDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDesc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblDesc.Location = new System.Drawing.Point(5, 5);
            this.lblDesc.Margin = new System.Windows.Forms.Padding(5);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblDesc.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblDesc.Size = new System.Drawing.Size(411, 15);
            this.lblDesc.TabIndex = 2;
            this.lblDesc.Text = "TXT_SELECT_PLAYLIST";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.opmLayoutPanel1.Controls.Add(this.lblDesc, 0, 2);
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
            this.opmLayoutPanel1.Size = new System.Drawing.Size(421, 359);
            this.opmLayoutPanel1.TabIndex = 6;
            // 
            // lblNewName
            // 
            this.lblNewName.AutoSize = true;
            this.lblNewName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNewName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblNewName.Location = new System.Drawing.Point(0, 300);
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
            this.txtNewName.Location = new System.Drawing.Point(158, 300);
            this.txtNewName.Margin = new System.Windows.Forms.Padding(0, 0, 5, 5);
            this.txtNewName.MaximumSize = new System.Drawing.Size(3000, 22);
            this.txtNewName.MaxLength = 32767;
            this.txtNewName.MinimumSize = new System.Drawing.Size(22, 22);
            this.txtNewName.Name = "txtNewName";
            this.txtNewName.OverrideForeColor = System.Drawing.Color.Empty;
            this.txtNewName.PasswordChar = '\0';
            this.txtNewName.ReadOnly = false;
            this.txtNewName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtNewName.ShortcutsEnabled = true;
            this.txtNewName.Size = new System.Drawing.Size(258, 22);
            this.txtNewName.TabIndex = 7;
            this.txtNewName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtNewName.UseSystemPasswordChar = false;
            this.txtNewName.WordWrap = false;
            // 
            // SelectOnlinePlaylistDlg
            // 
            this.ClientSize = new System.Drawing.Size(421, 359);
            this.Controls.Add(this.opmLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(200, 85);
            this.Name = "SelectOnlinePlaylistDlg";
            this.ShowIcon = false;
            this.opmLayoutPanel1.ResumeLayout(false);
            this.opmLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void lbPlaylists_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActivateOKButton();

            bool addNew = false;
            var sel = lbPlaylists.SelectedItem as OnlinePlaylist;
            if (sel != null)
                addNew = (sel.Id <= 0);

            lblNewName.Visible = txtNewName.Visible = addNew;
            if (addNew)
            {
                txtNewName.Select();
                txtNewName.Focus();
            }

        }
    }
}
