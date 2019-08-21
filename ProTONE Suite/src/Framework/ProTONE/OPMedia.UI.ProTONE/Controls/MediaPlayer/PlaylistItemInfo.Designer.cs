using OPMedia.UI.Controls;

namespace OPMedia.UI.ProTONE.Controls.MediaPlayer
{
    partial class PlaylistItemInfo
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
            this.pnlLayout = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.lblDesc = new OPMedia.UI.Controls.OPMLabel();
            this.pbInfo = new System.Windows.Forms.PictureBox();
            this.pnlDisplay = new OPMCustomPanel();
            this.propDisplay = new OPMedia.UI.Controls.InfoTextBox();
            this.pnlLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).BeginInit();
            this.pnlDisplay.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLayout
            // 
            this.pnlLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlLayout.ColumnCount = 1;
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlLayout.Controls.Add(this.lblDesc, 0, 0);
            this.pnlLayout.Controls.Add(this.pbInfo, 0, 1);
            this.pnlLayout.Controls.Add(this.pnlDisplay, 0, 2);
            this.pnlLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLayout.Location = new System.Drawing.Point(0, 0);
            this.pnlLayout.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLayout.Name = "pnlLayout";
            this.pnlLayout.RowCount = 3;
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlLayout.Size = new System.Drawing.Size(278, 475);
            this.pnlLayout.TabIndex = 0;
            // 
            // lblDesc
            // 
            this.lblDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDesc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblDesc.FontSize = OPMedia.UI.Themes.FontSizes.Large;
            this.lblDesc.Location = new System.Drawing.Point(5, 0);
            this.lblDesc.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.lblDesc.MaximumSize = new System.Drawing.Size(3500, 20);
            this.lblDesc.MinimumSize = new System.Drawing.Size(20, 20);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblDesc.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblDesc.SingleLine = true;
            this.lblDesc.Size = new System.Drawing.Size(270, 20);
            this.lblDesc.TabIndex = 0;
            this.lblDesc.Text = "opmLabel1";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbInfo
            // 
            this.pbInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbInfo.Location = new System.Drawing.Point(0, 20);
            this.pbInfo.Margin = new System.Windows.Forms.Padding(0);
            this.pbInfo.Name = "pbInfo";
            this.pbInfo.Size = new System.Drawing.Size(278, 270);
            this.pbInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbInfo.TabIndex = 3;
            this.pbInfo.TabStop = false;
            // 
            // pnlDisplay
            // 
            this.pnlDisplay.Controls.Add(this.propDisplay);
            this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDisplay.ForeColor = System.Drawing.Color.Black;
            this.pnlDisplay.Location = new System.Drawing.Point(5, 295);
            this.pnlDisplay.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.pnlDisplay.Name = "pnlDisplay";
            this.pnlDisplay.Padding = new System.Windows.Forms.Padding(3);
            this.pnlDisplay.Size = new System.Drawing.Size(268, 180);
            this.pnlDisplay.TabIndex = 2;
            // 
            // propDisplay
            // 
            this.propDisplay.AutoSize = true;
            this.propDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.propDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propDisplay.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.propDisplay.ForeColor = System.Drawing.Color.Black;
            this.propDisplay.Location = new System.Drawing.Point(3, 3);
            this.propDisplay.Margin = new System.Windows.Forms.Padding(0);
            this.propDisplay.Name = "propDisplay";
            this.propDisplay.ReadOnly = true;
            this.propDisplay.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.propDisplay.Size = new System.Drawing.Size(260, 172);
            this.propDisplay.TabIndex = 2;
            this.propDisplay.Text = "";
            // 
            // PlaylistItemInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.pnlLayout);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PlaylistItemInfo";
            this.Size = new System.Drawing.Size(278, 475);
            this.pnlLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).EndInit();
            this.pnlDisplay.ResumeLayout(false);
            this.pnlDisplay.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Controls.OPMTableLayoutPanel pnlLayout;
        private UI.Controls.OPMLabel lblDesc;
        private UI.Controls.InfoTextBox propDisplay;
        private System.Windows.Forms.PictureBox pbInfo;
        private UI.Controls.OPMCustomPanel pnlDisplay;
    }
}
