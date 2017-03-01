namespace OPMedia.UI.ProTONE.Dialogs
{
    partial class StreamingServerChooserDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lvServers = new OPMedia.UI.Controls.OPMListView();
            this.colEmpty = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colURL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colGenre = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.opmTableLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.opmLabel2 = new OPMedia.UI.Controls.OPMLabel();
            this.btnOK = new OPMedia.UI.Controls.OPMButton();
            this.opmLabel1 = new OPMedia.UI.Controls.OPMLabel();
            this.txtSearch = new OPMedia.UI.Controls.OPMTextBox();
            this.txtSelectedURL = new OPMedia.UI.Controls.OPMTextBox();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSource = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colBitrate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMediaType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colContent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlContent.SuspendLayout();
            this.opmTableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.opmTableLayoutPanel1);
            // 
            // lvServers
            // 
            this.lvServers.AllowEditing = true;
            this.lvServers.AlternateRowColors = true;
            this.lvServers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colEmpty,
            this.colName,
            this.colSource,
            this.colURL,
            this.colContent,
            this.colGenre,
            this.colBitrate,
            this.colMediaType});
            this.opmTableLayoutPanel1.SetColumnSpan(this.lvServers, 3);
            this.lvServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvServers.Location = new System.Drawing.Point(3, 36);
            this.lvServers.MultiSelect = false;
            this.lvServers.Name = "lvServers";
            this.lvServers.OverrideBackColor = System.Drawing.Color.Empty;
            this.lvServers.Size = new System.Drawing.Size(948, 325);
            this.lvServers.TabIndex = 0;
            this.lvServers.UseCompatibleStateImageBehavior = false;
            this.lvServers.View = System.Windows.Forms.View.Details;
            this.lvServers.SelectedIndexChanged += new System.EventHandler(this.lvServers_SelectedIndexChanged);
            // 
            // colEmpty
            // 
            this.colEmpty.Text = "";
            this.colEmpty.Width = 0;
            // 
            // colURL
            // 
            this.colURL.Text = "TXT_SERVERURL";
            this.colURL.Width = 113;
            // 
            // colGenre
            // 
            this.colGenre.Text = "TXT_GENRE";
            this.colGenre.Width = 105;
            // 
            // opmTableLayoutPanel1
            // 
            this.opmTableLayoutPanel1.ColumnCount = 3;
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmTableLayoutPanel1.Controls.Add(this.opmLabel2, 0, 4);
            this.opmTableLayoutPanel1.Controls.Add(this.lvServers, 0, 3);
            this.opmTableLayoutPanel1.Controls.Add(this.btnOK, 2, 4);
            this.opmTableLayoutPanel1.Controls.Add(this.opmLabel1, 0, 0);
            this.opmTableLayoutPanel1.Controls.Add(this.txtSearch, 1, 0);
            this.opmTableLayoutPanel1.Controls.Add(this.txtSelectedURL, 1, 4);
            this.opmTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.opmTableLayoutPanel1.Name = "opmTableLayoutPanel1";
            this.opmTableLayoutPanel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmTableLayoutPanel1.RowCount = 5;
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.opmTableLayoutPanel1.Size = new System.Drawing.Size(954, 395);
            this.opmTableLayoutPanel1.TabIndex = 1;
            // 
            // opmLabel2
            // 
            this.opmLabel2.AutoSize = true;
            this.opmLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel2.Location = new System.Drawing.Point(3, 364);
            this.opmLabel2.Name = "opmLabel2";
            this.opmLabel2.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel2.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel2.Size = new System.Drawing.Size(87, 31);
            this.opmLabel2.TabIndex = 10;
            this.opmLabel2.Text = "TXT_SERVERURL";
            this.opmLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.AutoSize = true;
            this.btnOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Location = new System.Drawing.Point(896, 367);
            this.btnOK.Name = "btnOK";
            this.btnOK.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnOK.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnOK.ShowDropDown = false;
            this.btnOK.Size = new System.Drawing.Size(55, 25);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "TXT_OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // opmLabel1
            // 
            this.opmLabel1.AutoSize = true;
            this.opmLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel1.FontSize = OPMedia.UI.Themes.FontSizes.NormalBold;
            this.opmLabel1.Location = new System.Drawing.Point(3, 10);
            this.opmLabel1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.opmLabel1.Name = "opmLabel1";
            this.opmLabel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel1.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel1.Size = new System.Drawing.Size(87, 20);
            this.opmLabel1.TabIndex = 1;
            this.opmLabel1.Text = "TXT_SEARCH:";
            this.opmLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSearch
            // 
            this.txtSearch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.txtSearch.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.opmTableLayoutPanel1.SetColumnSpan(this.txtSearch, 2);
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.txtSearch.Location = new System.Drawing.Point(93, 10);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0, 10, 0, 3);
            this.txtSearch.MaximumSize = new System.Drawing.Size(2000, 20);
            this.txtSearch.MaxLength = 32767;
            this.txtSearch.MinimumSize = new System.Drawing.Size(20, 20);
            this.txtSearch.Multiline = false;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.OverrideBackColor = System.Drawing.Color.Empty;
            this.txtSearch.OverrideForeColor = System.Drawing.Color.Empty;
            this.txtSearch.Padding = new System.Windows.Forms.Padding(3);
            this.txtSearch.PasswordChar = '\0';
            this.txtSearch.ReadOnly = false;
            this.txtSearch.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtSearch.ShortcutsEnabled = true;
            this.txtSearch.Size = new System.Drawing.Size(861, 20);
            this.txtSearch.TabIndex = 6;
            this.txtSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtSearch.UseSystemPasswordChar = false;
            this.txtSearch.WordWrap = true;
            // 
            // txtSelectedURL
            // 
            this.txtSelectedURL.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.txtSelectedURL.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtSelectedURL.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSelectedURL.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.txtSelectedURL.Location = new System.Drawing.Point(93, 369);
            this.txtSelectedURL.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.txtSelectedURL.MaximumSize = new System.Drawing.Size(2000, 20);
            this.txtSelectedURL.MaxLength = 32767;
            this.txtSelectedURL.MinimumSize = new System.Drawing.Size(20, 20);
            this.txtSelectedURL.Multiline = false;
            this.txtSelectedURL.Name = "txtSelectedURL";
            this.txtSelectedURL.OverrideBackColor = System.Drawing.Color.Empty;
            this.txtSelectedURL.OverrideForeColor = System.Drawing.Color.Empty;
            this.txtSelectedURL.Padding = new System.Windows.Forms.Padding(3);
            this.txtSelectedURL.PasswordChar = '\0';
            this.txtSelectedURL.ReadOnly = false;
            this.txtSelectedURL.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtSelectedURL.ShortcutsEnabled = true;
            this.txtSelectedURL.Size = new System.Drawing.Size(800, 20);
            this.txtSelectedURL.TabIndex = 9;
            this.txtSelectedURL.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtSelectedURL.UseSystemPasswordChar = false;
            this.txtSelectedURL.WordWrap = true;
            // 
            // colName
            // 
            this.colName.Text = "TXT_NAME";
            this.colName.Width = 116;
            // 
            // colSource
            // 
            this.colSource.Text = "TXT_SOURCE";
            this.colSource.Width = 119;
            // 
            // colBitrate
            // 
            this.colBitrate.Text = "TXT_BITRATE";
            this.colBitrate.Width = 82;
            // 
            // colMediaType
            // 
            this.colMediaType.Text = "TXT_MEDIATYPE";
            this.colMediaType.Width = 83;
            // 
            // colContent
            // 
            this.colContent.Text = "TXT_CONTENT";
            // 
            // StreamingServerChooserDlg
            // 
            this.ClientSize = new System.Drawing.Size(956, 419);
            this.MinimumSize = new System.Drawing.Size(200, 85);
            this.Name = "StreamingServerChooserDlg";
            this.pnlContent.ResumeLayout(false);
            this.opmTableLayoutPanel1.ResumeLayout(false);
            this.opmTableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Controls.OPMTableLayoutPanel opmTableLayoutPanel1;
        private UI.Controls.OPMListView lvServers;
        private UI.Controls.OPMLabel opmLabel1;
        private UI.Controls.OPMButton btnOK;
        private System.Windows.Forms.ColumnHeader colEmpty;
        private System.Windows.Forms.ColumnHeader colURL;
        private System.Windows.Forms.ColumnHeader colGenre;
        private UI.Controls.OPMTextBox txtSearch;
        private UI.Controls.OPMLabel opmLabel2;
        private UI.Controls.OPMTextBox txtSelectedURL;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colSource;
        private System.Windows.Forms.ColumnHeader colBitrate;
        private System.Windows.Forms.ColumnHeader colMediaType;
        private System.Windows.Forms.ColumnHeader colContent;
    }
}