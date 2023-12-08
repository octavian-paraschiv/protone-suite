using OPMedia.UI.Controls;

namespace OPMedia.Addons.Builtin.TaggedFileProp.TaggingWizard
{
    partial class FSTagEditorPanel
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
            this.label3 = new OPMedia.UI.Controls.OPMLabel();
            this.cmbFilePattern = new OPMedia.UI.Controls.OPMEditableComboBox();
            this.cmbFolderPattern = new OPMedia.UI.Controls.OPMEditableComboBox();
            this.opmTableLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.txtHints = new OPMedia.UI.Controls.MultilineEditTextBox();
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
            this.label1.Size = new System.Drawing.Size(354, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "TXT_FILENAMEPATTERN";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Location = new System.Drawing.Point(0, 46);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.OverrideBackColor = System.Drawing.Color.Empty;
            this.label3.OverrideForeColor = System.Drawing.Color.Empty;
            this.label3.Size = new System.Drawing.Size(354, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "TXT_FOLDERNAMEPATTERN";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbFilePattern
            // 
            this.cmbFilePattern.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbFilePattern.FormattingEnabled = true;
            this.cmbFilePattern.Items.AddRange(new object[] {
            "<A> - <T>",
            "<#> <A> - <T>",
            "<#> - <A> - <T>"});
            this.cmbFilePattern.Location = new System.Drawing.Point(0, 20);
            this.cmbFilePattern.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.cmbFilePattern.Name = "cmbFilePattern";
            this.cmbFilePattern.Size = new System.Drawing.Size(354, 23);
            this.cmbFilePattern.TabIndex = 1;
            this.cmbFilePattern.TextChanged += new System.EventHandler(this.cmbFilePattern_TextChanged);
            // 
            // cmbFolderPattern
            // 
            this.cmbFolderPattern.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbFolderPattern.FormattingEnabled = true;
            this.cmbFolderPattern.Items.AddRange(new object[] {
            "<A> - <B>",
            "<A> - <B> - <Y>"});
            this.cmbFolderPattern.Location = new System.Drawing.Point(0, 66);
            this.cmbFolderPattern.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.cmbFolderPattern.Name = "cmbFolderPattern";
            this.cmbFolderPattern.Size = new System.Drawing.Size(354, 23);
            this.cmbFolderPattern.TabIndex = 3;
            this.cmbFolderPattern.TextChanged += new System.EventHandler(this.cmbFolderPattern_TextChanged);
            // 
            // opmTableLayoutPanel1
            // 
            this.opmTableLayoutPanel1.ColumnCount = 1;
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.opmTableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.opmTableLayoutPanel1.Controls.Add(this.cmbFilePattern, 0, 1);
            this.opmTableLayoutPanel1.Controls.Add(this.cmbFolderPattern, 0, 3);
            this.opmTableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.opmTableLayoutPanel1.Controls.Add(this.txtHints, 0, 4);
            this.opmTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.opmTableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.opmTableLayoutPanel1.Name = "opmTableLayoutPanel1";
            this.opmTableLayoutPanel1.RowCount = 5;
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.opmTableLayoutPanel1.Size = new System.Drawing.Size(354, 276);
            this.opmTableLayoutPanel1.TabIndex = 5;
            // 
            // txtHints
            // 
            this.txtHints.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtHints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHints.FontSize = OPMedia.UI.Themes.FontSizes.Small;
            this.txtHints.Lines = new string[] {
        "TXT_TAGGINGPATTERNS"};
            this.txtHints.Location = new System.Drawing.Point(0, 92);
            this.txtHints.Margin = new System.Windows.Forms.Padding(0);
            this.txtHints.MaxLength = 32767;
            this.txtHints.MultiLineText = "TXT_TAGGINGPATTERNS";
            this.txtHints.MultiLineTextSeparator = ';';
            this.txtHints.Name = "txtHints";
            this.txtHints.OverrideForeColor = System.Drawing.Color.Empty;
            this.txtHints.PasswordChar = '\0';
            this.txtHints.ReadOnly = true;
            this.txtHints.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtHints.ShortcutsEnabled = false;
            this.txtHints.Size = new System.Drawing.Size(354, 184);
            this.txtHints.TabIndex = 4;
            this.txtHints.Text = "TXT_TAGGINGPATTERNS";
            this.txtHints.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtHints.UseSystemPasswordChar = false;
            this.txtHints.WordWrap = true;
            // 
            // FSTagEditorPanel
            // 
            this.Controls.Add(this.opmTableLayoutPanel1);
            this.Name = "FSTagEditorPanel";
            this.Size = new System.Drawing.Size(354, 276);
            this.opmTableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private OPMLabel label1;
        private OPMLabel label3;
        private OPMEditableComboBox cmbFilePattern;
        private OPMEditableComboBox cmbFolderPattern;
        private OPMTableLayoutPanel opmTableLayoutPanel1;
        private MultilineEditTextBox txtHints;
    }
}
