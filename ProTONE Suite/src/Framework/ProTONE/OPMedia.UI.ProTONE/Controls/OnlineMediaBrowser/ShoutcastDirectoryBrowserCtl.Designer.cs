namespace OPMedia.UI.ProTONE.Controls.OnlineMediaBrowser
{
    partial class ShoutcastDirectoryBrowserCtl
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
            this.opmTableLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.lvRadioStations = new OPMedia.UI.Controls.OPMListView();
            this.colEmpty = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSource = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colURL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colContent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colGenre = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colBitrate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMediaType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblFilterHint = new OPMedia.UI.Controls.TransparentRichTextBox();
            this.opmTableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // opmTableLayoutPanel1
            // 
            this.opmTableLayoutPanel1.ColumnCount = 1;
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.Controls.Add(this.lvRadioStations, 0, 0);
            this.opmTableLayoutPanel1.Controls.Add(this.lblFilterHint, 0, 1);
            this.opmTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.opmTableLayoutPanel1.Name = "opmTableLayoutPanel1";
            this.opmTableLayoutPanel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmTableLayoutPanel1.RowCount = 2;
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.Size = new System.Drawing.Size(617, 395);
            this.opmTableLayoutPanel1.TabIndex = 2;
            // 
            // lvRadioStations
            // 
            this.lvRadioStations.AllowEditing = false;
            this.lvRadioStations.AlternateRowColors = true;
            this.lvRadioStations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colEmpty,
            this.colName,
            this.colSource,
            this.colURL,
            this.colContent,
            this.colGenre,
            this.colBitrate,
            this.colMediaType});
            this.lvRadioStations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvRadioStations.Location = new System.Drawing.Point(3, 3);
            this.lvRadioStations.MultiSelect = false;
            this.lvRadioStations.Name = "lvRadioStations";
            this.lvRadioStations.OverrideBackColor = System.Drawing.Color.Empty;
            this.lvRadioStations.Size = new System.Drawing.Size(611, 283);
            this.lvRadioStations.TabIndex = 1;
            this.lvRadioStations.UseCompatibleStateImageBehavior = false;
            this.lvRadioStations.View = System.Windows.Forms.View.Details;
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
            // colSource
            // 
            this.colSource.Text = "TXT_SOURCE";
            this.colSource.Width = 119;
            // 
            // colURL
            // 
            this.colURL.Text = "TXT_SERVERURL";
            this.colURL.Width = 113;
            // 
            // colContent
            // 
            this.colContent.Text = "TXT_CONTENT";
            // 
            // colGenre
            // 
            this.colGenre.Text = "TXT_GENRE";
            this.colGenre.Width = 105;
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
            // lblFilterHint
            // 
            this.lblFilterHint.AutoSize = true;
            this.lblFilterHint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.lblFilterHint.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblFilterHint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFilterHint.ForeColor = System.Drawing.Color.Black;
            this.lblFilterHint.Location = new System.Drawing.Point(3, 292);
            this.lblFilterHint.Name = "lblFilterHint";
            this.lblFilterHint.ReadOnly = true;
            this.lblFilterHint.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.lblFilterHint.Size = new System.Drawing.Size(611, 100);
            this.lblFilterHint.TabIndex = 2;
            this.lblFilterHint.Text = "TXT_SHOUTCASTFILTER_HINT";
            // 
            // ShoutcastDirectoryBrowserCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.opmTableLayoutPanel1);
            this.Name = "ShoutcastDirectoryBrowserCtl";
            this.Size = new System.Drawing.Size(617, 395);
            this.opmTableLayoutPanel1.ResumeLayout(false);
            this.opmTableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Controls.OPMListView lvRadioStations;
        private System.Windows.Forms.ColumnHeader colEmpty;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colSource;
        private System.Windows.Forms.ColumnHeader colURL;
        private System.Windows.Forms.ColumnHeader colContent;
        private System.Windows.Forms.ColumnHeader colGenre;
        private System.Windows.Forms.ColumnHeader colBitrate;
        private System.Windows.Forms.ColumnHeader colMediaType;
        private UI.Controls.OPMTableLayoutPanel opmTableLayoutPanel1;
        private UI.Controls.TransparentRichTextBox lblFilterHint;
    }
}
