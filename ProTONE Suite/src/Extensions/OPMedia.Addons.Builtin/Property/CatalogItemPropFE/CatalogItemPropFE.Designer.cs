using OPMedia.UI.Controls;
namespace OPMedia.Addons.Builtin.CatalogExplorer.CatalogItemPropFE
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pgProperties = new OPMPropertyGrid();
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
            this.pgProperties.Size = new System.Drawing.Size(296, 238);
            this.pgProperties.TabIndex = 0;
            this.pgProperties.ToolbarVisible = false;
            // 
            // AddonPanel
            // 
            this.Controls.Add(this.pgProperties);
            this.Name = "AddonPanel";
            this.Size = new System.Drawing.Size(306, 248);
            this.ResumeLayout(false);

        }

        #endregion

        private OPMPropertyGrid pgProperties;
    }
}
