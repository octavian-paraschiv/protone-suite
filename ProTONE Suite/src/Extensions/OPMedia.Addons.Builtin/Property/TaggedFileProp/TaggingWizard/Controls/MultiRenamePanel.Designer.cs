using OPMedia.UI.Controls;

namespace OPMedia.Addons.Builtin.TaggedFileProp.TaggingWizard
{
    partial class MultiRenamePanel
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
            this.label1 = new OPMedia.UI.Controls.OPMLabel();
            this.txtRenamePattern = new OPMedia.UI.Controls.OPMTextBox();
            this.txtHints = new OPMedia.UI.Controls.MultilineEditTextBox();
            this.opmTableLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.opmTableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.OverrideBackColor = System.Drawing.Color.Empty;
            this.label1.OverrideForeColor = System.Drawing.Color.Empty;
            this.label1.Size = new System.Drawing.Size(350, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "TXT_RENAMEPATTERN";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtRenamePattern
            // 
            this.txtRenamePattern.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtRenamePattern.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRenamePattern.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.txtRenamePattern.Lines = new string[] {
        "<N>"};
            this.txtRenamePattern.Location = new System.Drawing.Point(0, 20);
            this.txtRenamePattern.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.txtRenamePattern.MaximumSize = new System.Drawing.Size(3000, 22);
            this.txtRenamePattern.MaxLength = 32767;
            this.txtRenamePattern.MinimumSize = new System.Drawing.Size(22, 22);
            this.txtRenamePattern.Name = "txtRenamePattern";
            this.txtRenamePattern.OverrideForeColor = System.Drawing.Color.Empty;
            this.txtRenamePattern.PasswordChar = '\0';
            this.txtRenamePattern.ReadOnly = false;
            this.txtRenamePattern.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtRenamePattern.ShortcutsEnabled = true;
            this.txtRenamePattern.Size = new System.Drawing.Size(350, 22);
            this.txtRenamePattern.TabIndex = 1;
            this.txtRenamePattern.Text = "<N>";
            this.txtRenamePattern.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtRenamePattern.UseSystemPasswordChar = false;
            this.txtRenamePattern.WordWrap = true;
            // 
            // txtHints
            // 
            this.txtHints.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtHints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHints.FontSize = OPMedia.UI.Themes.FontSizes.Small;
            this.txtHints.Lines = new string[] {
        "TXT_TAGGINGPATTERNS"};
            this.txtHints.Location = new System.Drawing.Point(0, 45);
            this.txtHints.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.txtHints.MaxLength = 32767;
            this.txtHints.MultiLineText = "TXT_TAGGINGPATTERNS";
            this.txtHints.MultiLineTextSeparator = ';';
            this.txtHints.Name = "txtHints";
            this.txtHints.OverrideForeColor = System.Drawing.Color.Empty;
            this.txtHints.PasswordChar = '\0';
            this.txtHints.ReadOnly = true;
            this.txtHints.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtHints.ShortcutsEnabled = true;
            this.txtHints.Size = new System.Drawing.Size(350, 232);
            this.txtHints.TabIndex = 2;
            this.txtHints.Text = "TXT_TAGGINGPATTERNS";
            this.txtHints.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtHints.UseSystemPasswordChar = false;
            this.txtHints.WordWrap = true;
            // 
            // opmTableLayoutPanel1
            // 
            this.opmTableLayoutPanel1.ColumnCount = 1;
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.opmTableLayoutPanel1.Controls.Add(this.txtHints, 0, 2);
            this.opmTableLayoutPanel1.Controls.Add(this.txtRenamePattern, 0, 1);
            this.opmTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.opmTableLayoutPanel1.Name = "opmTableLayoutPanel1";
            this.opmTableLayoutPanel1.RowCount = 3;
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.Size = new System.Drawing.Size(350, 280);
            this.opmTableLayoutPanel1.TabIndex = 3;
            // 
            // MultiRenamePanel
            // 
            this.Controls.Add(this.opmTableLayoutPanel1);
            this.Name = "MultiRenamePanel";
            this.opmTableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private OPMLabel label1;
        private OPMTextBox txtRenamePattern;
        private MultilineEditTextBox txtHints;
        private OPMTableLayoutPanel opmTableLayoutPanel1;
    }
}
