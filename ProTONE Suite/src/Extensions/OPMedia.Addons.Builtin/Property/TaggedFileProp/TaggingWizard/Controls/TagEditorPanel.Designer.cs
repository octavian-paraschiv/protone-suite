using OPMedia.UI.Controls;
using System.Windows.Forms;

namespace OPMedia.Addons.Builtin.TaggedFileProp.TaggingWizard
{
    partial class TagEditorPanel
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
            this.txtHints = new OPMedia.UI.Controls.MultilineEditTextBox();
            this.pgPatterns = new OPMedia.UI.Controls.OPMPropertyGrid();
            this.opmTableLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.opmTableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtHints
            // 
            this.txtHints.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtHints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHints.FontSize = OPMedia.UI.Themes.FontSizes.Small;
            this.txtHints.Lines = new string[] {
        "TXT_TAGGINGPATTERNS"};
            this.txtHints.Location = new System.Drawing.Point(140, 0);
            this.txtHints.Margin = new System.Windows.Forms.Padding(0);
            this.txtHints.MaxLength = 32767;
            this.txtHints.MultiLineText = "TXT_TAGGINGPATTERNS";
            this.txtHints.MultiLineTextSeparator = ';';
            this.txtHints.Name = "txtHints";
            this.txtHints.OverrideBackColor = System.Drawing.Color.Transparent;
            this.txtHints.OverrideForeColor = System.Drawing.Color.Empty;
            this.txtHints.PasswordChar = '\0';
            this.txtHints.ReadOnly = true;
            this.txtHints.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtHints.ShortcutsEnabled = true;
            this.txtHints.Size = new System.Drawing.Size(210, 280);
            this.txtHints.TabIndex = 1;
            this.txtHints.Text = "TXT_TAGGINGPATTERNS";
            this.txtHints.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtHints.UseSystemPasswordChar = false;
            this.txtHints.WordWrap = true;
            // 
            // pgPatterns
            // 
            this.pgPatterns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgPatterns.HelpVisible = false;
            this.pgPatterns.Location = new System.Drawing.Point(0, 0);
            this.pgPatterns.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.pgPatterns.Name = "pgPatterns";
            this.pgPatterns.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.pgPatterns.Size = new System.Drawing.Size(137, 280);
            this.pgPatterns.TabIndex = 0;
            this.pgPatterns.ToolbarVisible = false;
            // 
            // opmTableLayoutPanel1
            // 
            this.opmTableLayoutPanel1.ColumnCount = 2;
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.opmTableLayoutPanel1.Controls.Add(this.txtHints, 1, 0);
            this.opmTableLayoutPanel1.Controls.Add(this.pgPatterns, 0, 0);
            this.opmTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.opmTableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.opmTableLayoutPanel1.Name = "opmTableLayoutPanel1";
            this.opmTableLayoutPanel1.RowCount = 1;
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.Size = new System.Drawing.Size(350, 280);
            this.opmTableLayoutPanel1.TabIndex = 2;
            // 
            // TagEditorPanel
            // 
            this.Controls.Add(this.opmTableLayoutPanel1);
            this.Name = "TagEditorPanel";
            this.opmTableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MultilineEditTextBox txtHints;
        private OPMPropertyGrid pgPatterns;
        private OPMTableLayoutPanel opmTableLayoutPanel1;
    }
}
