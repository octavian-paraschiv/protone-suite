using OPMedia.UI.Controls;
namespace OPMedia.Addons.Builtin.VideoProp
{
    partial class AddonPanel
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
            this.pgProperties = new OPMedia.UI.Controls.OPMPropertyGrid();
            this.btnSearchSubtitles = new OPMedia.UI.Controls.OPMButton();
            this.SuspendLayout();
            // 
            // pgProperties
            // 
            this.pgProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgProperties.HelpVisible = false;
            this.pgProperties.Location = new System.Drawing.Point(5, 5);
            this.pgProperties.Margin = new System.Windows.Forms.Padding(5);
            this.pgProperties.Name = "pgProperties";
            this.pgProperties.Size = new System.Drawing.Size(195, 184);
            this.pgProperties.TabIndex = 0;
            this.pgProperties.ToolbarVisible = false;
            // 
            // btnSearchSubtitles
            // 
            this.btnSearchSubtitles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearchSubtitles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchSubtitles.Location = new System.Drawing.Point(60, 196);
            this.btnSearchSubtitles.Margin = new System.Windows.Forms.Padding(3, 3, 5, 5);
            this.btnSearchSubtitles.Name = "btnSearchSubtitles";
            this.btnSearchSubtitles.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnSearchSubtitles.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnSearchSubtitles.ShowDropDown = false;
            this.btnSearchSubtitles.Size = new System.Drawing.Size(140, 24);
            this.btnSearchSubtitles.TabIndex = 1;
            this.btnSearchSubtitles.Text = "TXT_SEARCH_SUBTITLES";
            this.btnSearchSubtitles.Click += new System.EventHandler(this.btnSearchSubtitles_Click);
            // 
            // AddonPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.Controls.Add(this.btnSearchSubtitles);
            this.Controls.Add(this.pgProperties);
            this.Name = "AddonPanel";
            this.Size = new System.Drawing.Size(205, 223);
            this.ResumeLayout(false);

        }

        #endregion

        private OPMPropertyGrid pgProperties;
        private OPMButton btnSearchSubtitles;
    }
}
