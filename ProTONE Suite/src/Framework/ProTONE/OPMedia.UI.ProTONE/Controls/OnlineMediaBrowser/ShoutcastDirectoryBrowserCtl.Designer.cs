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
            this.components = new System.ComponentModel.Container();
            this.lvRadioStations = new OPMedia.UI.Controls.OPMListView();
            this.colEmpty = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSource = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colURL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colContent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colGenre = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colBitrate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMediaType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.opmTableLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.opmLabel1 = new OPMedia.UI.Controls.OPMLabel();
            this.cmbSearch = new System.Windows.Forms.ComboBox();
            this.btnSearch = new OPMedia.UI.Controls.OPMButton();
            this.opmTableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvRadioStations
            // 
            this.lvRadioStations.AllowEditing = false;
            this.lvRadioStations.AlternateRowColors = true;
            this.lvRadioStations.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvRadioStations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colEmpty,
            this.colName,
            this.colSource,
            this.colURL,
            this.colContent,
            this.colGenre,
            this.colBitrate,
            this.colMediaType});
            this.opmTableLayoutPanel1.SetColumnSpan(this.lvRadioStations, 3);
            this.lvRadioStations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvRadioStations.Location = new System.Drawing.Point(0, 0);
            this.lvRadioStations.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lvRadioStations.MultiSelect = false;
            this.lvRadioStations.Name = "lvRadioStations";
            this.lvRadioStations.OverrideBackColor = System.Drawing.Color.Empty;
            this.lvRadioStations.Size = new System.Drawing.Size(449, 349);
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
            // opmTableLayoutPanel1
            // 
            this.opmTableLayoutPanel1.ColumnCount = 3;
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmTableLayoutPanel1.Controls.Add(this.opmLabel1, 0, 1);
            this.opmTableLayoutPanel1.Controls.Add(this.lvRadioStations, 0, 0);
            this.opmTableLayoutPanel1.Controls.Add(this.cmbSearch, 1, 1);
            this.opmTableLayoutPanel1.Controls.Add(this.btnSearch, 2, 1);
            this.opmTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.opmTableLayoutPanel1.Name = "opmTableLayoutPanel1";
            this.opmTableLayoutPanel1.RowCount = 2;
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.Size = new System.Drawing.Size(449, 381);
            this.opmTableLayoutPanel1.TabIndex = 3;
            // 
            // opmLabel1
            // 
            this.opmLabel1.AutoSize = true;
            this.opmLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel1.FontSize = OPMedia.UI.Themes.FontSizes.NormalBold;
            this.opmLabel1.Location = new System.Drawing.Point(0, 354);
            this.opmLabel1.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.opmLabel1.Name = "opmLabel1";
            this.opmLabel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel1.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel1.Size = new System.Drawing.Size(82, 27);
            this.opmLabel1.TabIndex = 2;
            this.opmLabel1.Text = "TXT_SEARCH:";
            this.opmLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbSearch
            // 
            this.cmbSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSearch.FormattingEnabled = true;
            this.cmbSearch.Location = new System.Drawing.Point(85, 356);
            this.cmbSearch.Margin = new System.Windows.Forms.Padding(0, 2, 3, 0);
            this.cmbSearch.Name = "cmbSearch";
            this.cmbSearch.Size = new System.Drawing.Size(272, 23);
            this.cmbSearch.TabIndex = 3;
            this.cmbSearch.TextChanged += new System.EventHandler(this.OnSearchTextChanged);
            this.cmbSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cmbSearch_KeyUp);
            // 
            // btnSearch
            // 
            this.btnSearch.AutoSize = true;
            this.btnSearch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Location = new System.Drawing.Point(360, 354);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnSearch.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnSearch.ShowDropDown = false;
            this.btnSearch.Size = new System.Drawing.Size(89, 27);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "TXT_SEARCH";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ShoutcastDirectoryBrowserCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.opmTableLayoutPanel1);
            this.Name = "ShoutcastDirectoryBrowserCtl";
            this.Size = new System.Drawing.Size(449, 381);
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
        private UI.Controls.OPMLabel opmLabel1;
        private System.Windows.Forms.ComboBox cmbSearch;
        private UI.Controls.OPMButton btnSearch;
    }
}
