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
            this.tabContentBrowser = new OPMedia.UI.Controls.OPMTabControl();
            this.tpLocalDatabase = new System.Windows.Forms.TabPage();
            this.localDbBrowser = new OPMedia.UI.ProTONE.Controls.OnlineMediaBrowser.LocalDatabaseBrowserCtl();
            this.tpShoutcastDir = new System.Windows.Forms.TabPage();
            this.shoutcastDirBrowser = new OPMedia.UI.ProTONE.Controls.OnlineMediaBrowser.ShoutcastDirectoryBrowserCtl();
            this.tpDeezerContent = new System.Windows.Forms.TabPage();
            this.deezerTrackBrowser = new OPMedia.UI.ProTONE.Controls.OnlineMediaBrowser.DeezerTrackBrowserCtl();
            this.tabContentBrowser.SuspendLayout();
            this.tpLocalDatabase.SuspendLayout();
            this.tpShoutcastDir.SuspendLayout();
            this.tpDeezerContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabContentBrowser
            // 
            this.tabContentBrowser.Controls.Add(this.tpLocalDatabase);
            this.tabContentBrowser.Controls.Add(this.tpShoutcastDir);
            this.tabContentBrowser.Controls.Add(this.tpDeezerContent);
            this.tabContentBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabContentBrowser.InnerPadding = new System.Windows.Forms.Padding(0);
            this.tabContentBrowser.ItemSize = new System.Drawing.Size(106, 55);
            this.tabContentBrowser.Location = new System.Drawing.Point(0, 0);
            this.tabContentBrowser.Margin = new System.Windows.Forms.Padding(3, 5, 1, 0);
            this.tabContentBrowser.Name = "tabContentBrowser";
            this.tabContentBrowser.SelectedIndex = 0;
            this.tabContentBrowser.Size = new System.Drawing.Size(943, 676);
            this.tabContentBrowser.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabContentBrowser.TabIndex = 3;
            this.tabContentBrowser.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // tpLocalDatabase
            // 
            this.tpLocalDatabase.BackColor = System.Drawing.Color.White;
            this.tpLocalDatabase.Controls.Add(this.localDbBrowser);
            this.tpLocalDatabase.Location = new System.Drawing.Point(4, 59);
            this.tpLocalDatabase.Margin = new System.Windows.Forms.Padding(0);
            this.tpLocalDatabase.Name = "tpLocalDatabase";
            this.tpLocalDatabase.Size = new System.Drawing.Size(935, 613);
            this.tpLocalDatabase.TabIndex = 0;
            this.tpLocalDatabase.Text = "TXT_LOCAL_DB";
            // 
            // localDbBrowser
            // 
            this.localDbBrowser.AutoSize = true;
            this.localDbBrowser.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.localDbBrowser.ContextMenuStrip = null;
            this.localDbBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.localDbBrowser.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.localDbBrowser.Location = new System.Drawing.Point(0, 0);
            this.localDbBrowser.Margin = new System.Windows.Forms.Padding(0);
            this.localDbBrowser.Name = "localDbBrowser";
            this.localDbBrowser.Size = new System.Drawing.Size(935, 613);
            this.localDbBrowser.TabIndex = 0;
            // 
            // tpShoutcastDir
            // 
            this.tpShoutcastDir.BackColor = System.Drawing.Color.White;
            this.tpShoutcastDir.Controls.Add(this.shoutcastDirBrowser);
            this.tpShoutcastDir.Location = new System.Drawing.Point(4, 23);
            this.tpShoutcastDir.Margin = new System.Windows.Forms.Padding(0);
            this.tpShoutcastDir.Name = "tpShoutcastDir";
            this.tpShoutcastDir.Size = new System.Drawing.Size(192, 73);
            this.tpShoutcastDir.TabIndex = 1;
            this.tpShoutcastDir.Text = "TXT_SHOUTCAST_DIR";
            // 
            // shoutcastDirBrowser
            // 
            this.shoutcastDirBrowser.ContextMenuStrip = null;
            this.shoutcastDirBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shoutcastDirBrowser.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.shoutcastDirBrowser.Location = new System.Drawing.Point(0, 0);
            this.shoutcastDirBrowser.Name = "shoutcastDirBrowser";
            this.shoutcastDirBrowser.Size = new System.Drawing.Size(192, 73);
            this.shoutcastDirBrowser.TabIndex = 0;
            // 
            // tpDeezerContent
            // 
            this.tpDeezerContent.BackColor = System.Drawing.Color.White;
            this.tpDeezerContent.Controls.Add(this.deezerTrackBrowser);
            this.tpDeezerContent.Location = new System.Drawing.Point(4, 23);
            this.tpDeezerContent.Margin = new System.Windows.Forms.Padding(0);
            this.tpDeezerContent.Name = "tpDeezerContent";
            this.tpDeezerContent.Size = new System.Drawing.Size(192, 73);
            this.tpDeezerContent.TabIndex = 2;
            this.tpDeezerContent.Text = "TXT_DEEZER_CONTENT";
            // 
            // deezerTrackBrowser
            // 
            this.deezerTrackBrowser.ContextMenuStrip = null;
            this.deezerTrackBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deezerTrackBrowser.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.deezerTrackBrowser.Location = new System.Drawing.Point(0, 0);
            this.deezerTrackBrowser.Margin = new System.Windows.Forms.Padding(0);
            this.deezerTrackBrowser.Name = "deezerTrackBrowser";
            this.deezerTrackBrowser.Size = new System.Drawing.Size(192, 73);
            this.deezerTrackBrowser.TabIndex = 0;
            // 
            // OnlineContentBrowser
            // 
            this.ClientSize = new System.Drawing.Size(943, 676);
            this.Controls.Add(this.tabContentBrowser);
            this.MinimumSize = new System.Drawing.Size(200, 85);
            this.Name = "OnlineContentBrowser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.tabContentBrowser.ResumeLayout(false);
            this.tpLocalDatabase.ResumeLayout(false);
            this.tpLocalDatabase.PerformLayout();
            this.tpShoutcastDir.ResumeLayout(false);
            this.tpDeezerContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private UI.Controls.OPMTabControl tabContentBrowser;
        private System.Windows.Forms.TabPage tpLocalDatabase;
        private System.Windows.Forms.TabPage tpShoutcastDir;
        private System.Windows.Forms.TabPage tpDeezerContent;
        private Controls.OnlineMediaBrowser.LocalDatabaseBrowserCtl localDbBrowser;
        private Controls.OnlineMediaBrowser.ShoutcastDirectoryBrowserCtl shoutcastDirBrowser;
        private Controls.OnlineMediaBrowser.DeezerTrackBrowserCtl deezerTrackBrowser;
    }
}