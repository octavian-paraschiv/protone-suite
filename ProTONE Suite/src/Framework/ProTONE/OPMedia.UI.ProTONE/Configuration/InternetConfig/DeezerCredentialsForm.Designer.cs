namespace OPMedia.UI.ProTONE.Configuration.InternetConfig
{
    partial class DeezerCredentialsForm
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
            this.txtAppId = new OPMedia.UI.Controls.OPMTextBox();
            this.opmTableLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.opmLabel1 = new OPMedia.UI.Controls.OPMLabel();
            this.btnRequest = new OPMedia.UI.Controls.OPMButton();
            this.opmLabel2 = new OPMedia.UI.Controls.OPMLabel();
            this.pnlContent.SuspendLayout();
            this.opmTableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.opmTableLayoutPanel1);
            // 
            // txtAppId
            // 
            this.txtAppId.AutoSize = true;
            this.txtAppId.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.txtAppId.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.opmTableLayoutPanel1.SetColumnSpan(this.txtAppId, 2);
            this.txtAppId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAppId.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.txtAppId.Lines = new string[0];
            this.txtAppId.Location = new System.Drawing.Point(109, 0);
            this.txtAppId.Margin = new System.Windows.Forms.Padding(0);
            this.txtAppId.MaximumSize = new System.Drawing.Size(3000, 22);
            this.txtAppId.MaxLength = 32767;
            this.txtAppId.MinimumSize = new System.Drawing.Size(22, 22);
            this.txtAppId.Name = "txtAppId";
            this.txtAppId.OverrideBackColor = System.Drawing.Color.Transparent;
            this.txtAppId.OverrideForeColor = System.Drawing.Color.Empty;
            this.txtAppId.PasswordChar = '\0';
            this.txtAppId.ReadOnly = false;
            this.txtAppId.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtAppId.ShortcutsEnabled = true;
            this.txtAppId.Size = new System.Drawing.Size(216, 22);
            this.txtAppId.TabIndex = 10;
            this.txtAppId.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtAppId.UseSystemPasswordChar = false;
            this.txtAppId.WordWrap = true;
            // 
            // opmTableLayoutPanel1
            // 
            this.opmTableLayoutPanel1.ColumnCount = 4;
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.opmTableLayoutPanel1.Controls.Add(this.opmLabel1, 0, 0);
            this.opmTableLayoutPanel1.Controls.Add(this.txtAppId, 1, 0);
            this.opmTableLayoutPanel1.Controls.Add(this.btnRequest, 1, 2);
            this.opmTableLayoutPanel1.Controls.Add(this.opmLabel2, 1, 1);
            this.opmTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.opmTableLayoutPanel1.Name = "opmTableLayoutPanel1";
            this.opmTableLayoutPanel1.RowCount = 4;
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.Size = new System.Drawing.Size(328, 74);
            this.opmTableLayoutPanel1.TabIndex = 2;
            // 
            // opmLabel1
            // 
            this.opmLabel1.AutoSize = true;
            this.opmLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel1.Location = new System.Drawing.Point(0, 0);
            this.opmLabel1.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.opmLabel1.Name = "opmLabel1";
            this.opmLabel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel1.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel1.Size = new System.Drawing.Size(106, 22);
            this.opmLabel1.TabIndex = 0;
            this.opmLabel1.Text = "TXT_REDIRECTURL";
            this.opmLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnRequest
            // 
            this.btnRequest.AutoSize = true;
            this.btnRequest.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRequest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRequest.Location = new System.Drawing.Point(109, 42);
            this.btnRequest.Margin = new System.Windows.Forms.Padding(0);
            this.btnRequest.Name = "btnRequest";
            this.btnRequest.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnRequest.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnRequest.ShowDropDown = false;
            this.btnRequest.Size = new System.Drawing.Size(167, 27);
            this.btnRequest.TabIndex = 11;
            this.btnRequest.Text = "TXT_REQUEST_NEW_TOKEN";
            this.btnRequest.UseVisualStyleBackColor = true;
            this.btnRequest.Click += new System.EventHandler(this.btnRequest_Click);
            // 
            // opmLabel2
            // 
            this.opmLabel2.AutoSize = true;
            this.opmTableLayoutPanel1.SetColumnSpan(this.opmLabel2, 2);
            this.opmLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel2.FontSize = OPMedia.UI.Themes.FontSizes.Smallest;
            this.opmLabel2.Location = new System.Drawing.Point(112, 22);
            this.opmLabel2.Name = "opmLabel2";
            this.opmLabel2.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel2.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel2.Size = new System.Drawing.Size(210, 20);
            this.opmLabel2.TabIndex = 14;
            this.opmLabel2.Text = "TXT_REDIRECTURL_HINT";
            // 
            // DeezerCredentialsForm
            // 
            this.ClientSize = new System.Drawing.Size(332, 100);
            this.MinimumSize = new System.Drawing.Size(200, 85);
            this.Name = "DeezerCredentialsForm";
            this.pnlContent.ResumeLayout(false);
            this.opmTableLayoutPanel1.ResumeLayout(false);
            this.opmTableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Controls.OPMTableLayoutPanel opmTableLayoutPanel1;
        private UI.Controls.OPMLabel opmLabel1;
        private UI.Controls.OPMTextBox txtAppId;
        private UI.Controls.OPMButton btnRequest;
        private UI.Controls.OPMLabel opmLabel2;

    }
}