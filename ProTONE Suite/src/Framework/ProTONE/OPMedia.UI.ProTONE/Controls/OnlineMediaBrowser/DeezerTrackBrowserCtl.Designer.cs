namespace OPMedia.UI.ProTONE.Controls.OnlineMediaBrowser
{
    partial class DeezerTrackBrowserCtl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lvTracks = new OPMedia.UI.Controls.OPMListView();
            this.colEmpty = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colArtist = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAlbum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colURL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.opmTableLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.lblFilterHint = new OPMedia.UI.Controls.TransparentRichTextBox();
            this.opmTableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvTracks
            // 
            this.lvTracks.AllowEditing = false;
            this.lvTracks.AlternateRowColors = true;
            this.lvTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colEmpty,
            this.colName,
            this.colArtist,
            this.colAlbum,
            this.colURL});
            this.lvTracks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvTracks.Location = new System.Drawing.Point(3, 3);
            this.lvTracks.MultiSelect = false;
            this.lvTracks.Name = "lvTracks";
            this.lvTracks.OverrideBackColor = System.Drawing.Color.Empty;
            this.lvTracks.Size = new System.Drawing.Size(562, 306);
            this.lvTracks.TabIndex = 2;
            this.lvTracks.UseCompatibleStateImageBehavior = false;
            this.lvTracks.View = System.Windows.Forms.View.Details;
            // 
            // colEmpty
            // 
            this.colEmpty.Text = "";
            this.colEmpty.Width = 0;
            // 
            // colName
            // 
            this.colName.Text = "TXT_NAME";
            this.colName.Width = 116;
            // 
            // colArtist
            // 
            this.colArtist.Text = "TXT_ARTIST";
            // 
            // colAlbum
            // 
            this.colAlbum.Text = "TXT_ALBUM";
            this.colAlbum.Width = 105;
            // 
            // colURL
            // 
            this.colURL.Text = "URL";
            this.colURL.Width = 113;
            // 
            // opmTableLayoutPanel1
            // 
            this.opmTableLayoutPanel1.ColumnCount = 1;
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.Controls.Add(this.lblFilterHint, 0, 1);
            this.opmTableLayoutPanel1.Controls.Add(this.lvTracks, 0, 0);
            this.opmTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.opmTableLayoutPanel1.Name = "opmTableLayoutPanel1";
            this.opmTableLayoutPanel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmTableLayoutPanel1.RowCount = 2;
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.opmTableLayoutPanel1.Size = new System.Drawing.Size(568, 428);
            this.opmTableLayoutPanel1.TabIndex = 3;
            // 
            // lblFilterHint
            // 
            this.lblFilterHint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.lblFilterHint.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblFilterHint.DetectUrls = false;
            this.lblFilterHint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFilterHint.ForeColor = System.Drawing.Color.Black;
            this.lblFilterHint.Location = new System.Drawing.Point(3, 315);
            this.lblFilterHint.Name = "lblFilterHint";
            this.lblFilterHint.ReadOnly = true;
            this.lblFilterHint.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.lblFilterHint.ShortcutsEnabled = false;
            this.lblFilterHint.Size = new System.Drawing.Size(562, 110);
            this.lblFilterHint.TabIndex = 3;
            this.lblFilterHint.Text = "";
            // 
            // DeezerTrackBrowserCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.opmTableLayoutPanel1);
            this.Name = "DeezerTrackBrowserCtl";
            this.Size = new System.Drawing.Size(568, 428);
            this.opmTableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Controls.OPMListView lvTracks;
        private System.Windows.Forms.ColumnHeader colEmpty;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colURL;
        private System.Windows.Forms.ColumnHeader colArtist;
        private System.Windows.Forms.ColumnHeader colAlbum;
        private UI.Controls.OPMTableLayoutPanel opmTableLayoutPanel1;
        private UI.Controls.TransparentRichTextBox lblFilterHint;
    }
}
