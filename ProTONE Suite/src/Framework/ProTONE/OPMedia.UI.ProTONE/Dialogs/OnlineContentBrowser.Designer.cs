namespace OPMedia.UI.ProTONE.Dialogs
{
    partial class OnlineContentBrowser
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
            this.components = new System.ComponentModel.Container();
            this.opmTableLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.opmLabel1 = new OPMedia.UI.Controls.OPMLabel();
            this.txtSearch = new OPMedia.UI.Controls.OPMTextBox();
            this.btnSearch = new OPMedia.UI.Controls.OPMButton();
            this.btnEnqueue = new OPMedia.UI.Controls.OPMButton();
            this.tabContentBrowser = new OPMedia.UI.Controls.OPMTabControl();
            this.tpLocalDatabase = new System.Windows.Forms.TabPage();
            this.localDbBrowser = new OPMedia.UI.ProTONE.Controls.OnlineMediaBrowser.LocalDatabaseBrowserCtl();
            this.tpShoutcastDir = new System.Windows.Forms.TabPage();
            this.shoutcastDirBrowser = new OPMedia.UI.ProTONE.Controls.OnlineMediaBrowser.ShoutcastDirectoryBrowserCtl();
            this.tpDeezerContent = new System.Windows.Forms.TabPage();
            this.deezerTrackBrowser = new OPMedia.UI.ProTONE.Controls.OnlineMediaBrowser.DeezerTrackBrowserCtl();
            this.btnPlay = new OPMedia.UI.Controls.OPMButton();
            this.pnlContent.SuspendLayout();
            this.opmTableLayoutPanel1.SuspendLayout();
            this.tabContentBrowser.SuspendLayout();
            this.tpLocalDatabase.SuspendLayout();
            this.tpShoutcastDir.SuspendLayout();
            this.tpDeezerContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.opmTableLayoutPanel1);
            // 
            // opmTableLayoutPanel1
            // 
            this.opmTableLayoutPanel1.ColumnCount = 3;
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmTableLayoutPanel1.Controls.Add(this.opmLabel1, 0, 0);
            this.opmTableLayoutPanel1.Controls.Add(this.txtSearch, 1, 0);
            this.opmTableLayoutPanel1.Controls.Add(this.btnSearch, 2, 0);
            this.opmTableLayoutPanel1.Controls.Add(this.btnEnqueue, 2, 2);
            this.opmTableLayoutPanel1.Controls.Add(this.tabContentBrowser, 0, 1);
            this.opmTableLayoutPanel1.Controls.Add(this.btnPlay, 1, 2);
            this.opmTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.opmTableLayoutPanel1.Name = "opmTableLayoutPanel1";
            this.opmTableLayoutPanel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmTableLayoutPanel1.RowCount = 2;
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.Size = new System.Drawing.Size(945, 652);
            this.opmTableLayoutPanel1.TabIndex = 0;
            // 
            // opmLabel1
            // 
            this.opmLabel1.AutoSize = true;
            this.opmLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel1.FontSize = OPMedia.UI.Themes.FontSizes.NormalBold;
            this.opmLabel1.Location = new System.Drawing.Point(3, 10);
            this.opmLabel1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.opmLabel1.Name = "opmLabel1";
            this.opmLabel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel1.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel1.Size = new System.Drawing.Size(76, 29);
            this.opmLabel1.TabIndex = 0;
            this.opmLabel1.Text = "TXT_SEARCH:";
            this.opmLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSearch
            // 
            this.txtSearch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.txtSearch.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.txtSearch.Location = new System.Drawing.Point(82, 14);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0, 14, 0, 0);
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
            this.txtSearch.Size = new System.Drawing.Size(757, 20);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtSearch.UseSystemPasswordChar = false;
            this.txtSearch.WordWrap = false;
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Location = new System.Drawing.Point(842, 11);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 11, 3, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnSearch.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnSearch.ShowDropDown = false;
            this.btnSearch.Size = new System.Drawing.Size(100, 25);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "TXT_SEARCH";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnEnqueue
            // 
            this.btnEnqueue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnqueue.Location = new System.Drawing.Point(842, 624);
            this.btnEnqueue.Name = "btnEnqueue";
            this.btnEnqueue.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnEnqueue.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnEnqueue.ShowDropDown = false;
            this.btnEnqueue.Size = new System.Drawing.Size(100, 25);
            this.btnEnqueue.TabIndex = 5;
            this.btnEnqueue.Text = "TXT_ENQUEUE";
            this.btnEnqueue.UseVisualStyleBackColor = true;
            this.btnEnqueue.Click += new System.EventHandler(this.btnEnqueue_Click);
            // 
            // tabContentBrowser
            // 
            this.opmTableLayoutPanel1.SetColumnSpan(this.tabContentBrowser, 3);
            this.tabContentBrowser.Controls.Add(this.tpLocalDatabase);
            this.tabContentBrowser.Controls.Add(this.tpShoutcastDir);
            this.tabContentBrowser.Controls.Add(this.tpDeezerContent);
            this.tabContentBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabContentBrowser.InnerPadding = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.tabContentBrowser.Location = new System.Drawing.Point(3, 42);
            this.tabContentBrowser.Name = "tabContentBrowser";
            this.tabContentBrowser.SelectedIndex = 0;
            this.tabContentBrowser.Size = new System.Drawing.Size(939, 576);
            this.tabContentBrowser.TabIndex = 3;
            // 
            // tpLocalDatabase
            // 
            this.tpLocalDatabase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.tpLocalDatabase.Controls.Add(this.localDbBrowser);
            this.tpLocalDatabase.Location = new System.Drawing.Point(4, 23);
            this.tpLocalDatabase.Name = "tpLocalDatabase";
            this.tpLocalDatabase.Padding = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.tpLocalDatabase.Size = new System.Drawing.Size(931, 549);
            this.tpLocalDatabase.TabIndex = 0;
            this.tpLocalDatabase.Text = "TXT_LOCAL_DB";
            // 
            // localDbBrowser
            // 
            this.localDbBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.localDbBrowser.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.localDbBrowser.Location = new System.Drawing.Point(5, 10);
            this.localDbBrowser.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.localDbBrowser.Name = "localDbBrowser";
            this.localDbBrowser.OverrideBackColor = System.Drawing.Color.Empty;
            this.localDbBrowser.Size = new System.Drawing.Size(921, 534);
            this.localDbBrowser.TabIndex = 0;
            // 
            // tpShoutcastDir
            // 
            this.tpShoutcastDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.tpShoutcastDir.Controls.Add(this.shoutcastDirBrowser);
            this.tpShoutcastDir.Location = new System.Drawing.Point(4, 23);
            this.tpShoutcastDir.Name = "tpShoutcastDir";
            this.tpShoutcastDir.Padding = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.tpShoutcastDir.Size = new System.Drawing.Size(931, 549);
            this.tpShoutcastDir.TabIndex = 1;
            this.tpShoutcastDir.Text = "TXT_SHOUTCAST_DIR";
            // 
            // shoutcastDirBrowser
            // 
            this.shoutcastDirBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shoutcastDirBrowser.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.shoutcastDirBrowser.Location = new System.Drawing.Point(5, 10);
            this.shoutcastDirBrowser.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.shoutcastDirBrowser.Name = "shoutcastDirBrowser";
            this.shoutcastDirBrowser.OverrideBackColor = System.Drawing.Color.Empty;
            this.shoutcastDirBrowser.Size = new System.Drawing.Size(921, 534);
            this.shoutcastDirBrowser.TabIndex = 0;
            // 
            // tpDeezerContent
            // 
            this.tpDeezerContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.tpDeezerContent.Controls.Add(this.deezerTrackBrowser);
            this.tpDeezerContent.Location = new System.Drawing.Point(4, 23);
            this.tpDeezerContent.Name = "tpDeezerContent";
            this.tpDeezerContent.Padding = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.tpDeezerContent.Size = new System.Drawing.Size(931, 549);
            this.tpDeezerContent.TabIndex = 2;
            this.tpDeezerContent.Text = "TXT_DEEZER_CONTENT";
            // 
            // deezerTrackBrowser
            // 
            this.deezerTrackBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deezerTrackBrowser.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.deezerTrackBrowser.Location = new System.Drawing.Point(5, 10);
            this.deezerTrackBrowser.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.deezerTrackBrowser.Name = "deezerTrackBrowser";
            this.deezerTrackBrowser.OverrideBackColor = System.Drawing.Color.Empty;
            this.deezerTrackBrowser.Size = new System.Drawing.Size(921, 534);
            this.deezerTrackBrowser.TabIndex = 0;
            // 
            // btnPlay
            // 
            this.btnPlay.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlay.Location = new System.Drawing.Point(736, 624);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnPlay.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnPlay.ShowDropDown = false;
            this.btnPlay.Size = new System.Drawing.Size(100, 25);
            this.btnPlay.TabIndex = 4;
            this.btnPlay.Text = "TXT_PLAY";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // OnlineContentBrowser
            // 
            this.AcceptButton = this.btnSearch;
            this.ClientSize = new System.Drawing.Size(947, 676);
            this.MinimumSize = new System.Drawing.Size(200, 85);
            this.Name = "OnlineContentBrowser";
            this.pnlContent.ResumeLayout(false);
            this.opmTableLayoutPanel1.ResumeLayout(false);
            this.opmTableLayoutPanel1.PerformLayout();
            this.tabContentBrowser.ResumeLayout(false);
            this.tpLocalDatabase.ResumeLayout(false);
            this.tpShoutcastDir.ResumeLayout(false);
            this.tpDeezerContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Controls.OPMTableLayoutPanel opmTableLayoutPanel1;
        private UI.Controls.OPMLabel opmLabel1;
        private UI.Controls.OPMTextBox txtSearch;
        private UI.Controls.OPMButton btnSearch;
        private UI.Controls.OPMTabControl tabContentBrowser;
        private System.Windows.Forms.TabPage tpLocalDatabase;
        private System.Windows.Forms.TabPage tpShoutcastDir;
        private UI.Controls.OPMButton btnEnqueue;
        private UI.Controls.OPMButton btnPlay;
        private System.Windows.Forms.TabPage tpDeezerContent;
        private Controls.OnlineMediaBrowser.LocalDatabaseBrowserCtl localDbBrowser;
        private Controls.OnlineMediaBrowser.ShoutcastDirectoryBrowserCtl shoutcastDirBrowser;
        private Controls.OnlineMediaBrowser.DeezerTrackBrowserCtl deezerTrackBrowser;
    }
}