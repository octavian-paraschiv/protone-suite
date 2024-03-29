using OPMedia.Core.Configuration;
using OPMedia.Core.TranslationSupport;
using OPMedia.Runtime.ProTONE;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.RemoteControl;
using OPMedia.UI.Configuration;
using OPMedia.UI.Controls;
using OPMedia.UI.Generic;
using OPMedia.UI.ProTONE.Properties;
using OPMedia.UI.Themes;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;


namespace OPMedia.UI.ProTONE.Configuration
{
    public partial class FileTypesPanel : BaseCfgPanel
    {
        private OPMLabel label3;
        private OPMComboBox cmbExplorerLaunchType;
        private OPMButton btnSelAllAudio;
        private OPMButton btnUnselAllAudio;
        private OPMButton btnUnselAllVideo;
        private OPMButton btnSelAllVideo;
        private OPMButton btnUnselAllPlaylists;
        private OPMButton btnSelAllPlaylists;
        private Panel pnlButtons;

        const string pattern = "GGGG";
        private Size _sizeText;
        private Size _sizeGlyph;

        public override Image Image
        {
            get
            {
                return Resources.FileTypes;
            }
        }

        private ExplorerLaunchType[] _launchTypes = new ExplorerLaunchType[]
            {
                new ExplorerLaunchType(CommandType.EnqueueFiles),
                new ExplorerLaunchType(CommandType.PlayFiles)
            };

        private OPMHeaderLabel hdrAudio;
        private OPMHeaderLabel hdrPlaylists;
        private OPMHeaderLabel hdrVideo;
        private OPMTableLayoutPanel tableLayoutPanel1;
        private OPMTableLayoutPanel opmLayoutPanel1;
        private OPMFlowLayoutPanel pnlPlaylists;
        private OPMFlowLayoutPanel pnlVideoFiles;
        private OPMFlowLayoutPanel pnlAudioFiles;
        private OPMCheckBox chkShellExtension;
        private OPMLabel lblFileTypes;

        public FileTypesPanel() : base()
        {
            this.Title = "TXT_S_FILETYPES";

            InitializeComponent();

            hdrAudio.Image = ImageProcessing.AudioFile16;
            hdrVideo.Image = ImageProcessing.VideoFile16;
            hdrPlaylists.Image = ImageProcessing.Playlist16;

            Translator.TranslateControl(this, DesignMode);
            ThemeManager.SetFont(btnSelAllAudio, FontSizes.Small);
            ThemeManager.SetFont(btnSelAllPlaylists, FontSizes.Small);
            ThemeManager.SetFont(btnSelAllVideo, FontSizes.Small);
            ThemeManager.SetFont(btnUnselAllAudio, FontSizes.Small);
            ThemeManager.SetFont(btnUnselAllPlaylists, FontSizes.Small);
            ThemeManager.SetFont(btnUnselAllVideo, FontSizes.Small);

            FillExplorerLaunchTypes();

            this.Enabled = AppConfig.CurrentUserIsAdministrator;

            this.HandleCreated += new EventHandler(FileTypesPanel_HandleCreated);
        }


        void FileTypesPanel_HandleCreated(object sender, EventArgs e)
        {
            using (Graphics g = CreateGraphics())
            {
                _sizeText = g.MeasureString(pattern, ThemeManager.SmallFont).ToSize();
                _sizeGlyph = CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.CheckedHot);
            }

            chkShellExtension.Checked = RegistrationSupport.IsShellExtensionRegistered();

            FillFileTypeAssociations();
        }

        private OPMCheckBox CreateCheckBox(string fileType, bool isChecked)
        {
            OPMCheckBox cb = new OPMCheckBox();
            cb.Checked = isChecked;
            cb.Margin = new Padding(0);
            cb.FontSize = FontSizes.Small;
            cb.Padding = new Padding(0);
            cb.AutoSize = false;
            cb.Text = fileType;

            cb.Width = 4 + _sizeText.Width + _sizeGlyph.Width;
            cb.Height = 8 + _sizeGlyph.Height;

            cb.Tag = fileType;
            cb.CheckedChanged += new EventHandler(OnCheckedChanged);
            return cb;
        }

        void OnCheckedChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        private void FillExplorerLaunchTypes()
        {
            cmbExplorerLaunchType.Items.Clear();
            cmbExplorerLaunchType.Items.AddRange(_launchTypes);

            try
            {
                CommandType type = (CommandType)Enum.Parse(typeof(CommandType),
                    ProTONEConfig.ExplorerLaunchType);

                cmbExplorerLaunchType.SelectedItem = new ExplorerLaunchType(type);
            }
            catch
            {
            }

            cmbExplorerLaunchType.SelectedIndexChanged -= new EventHandler(cmbExplorerLaunchType_SelectedIndexChanged);
            cmbExplorerLaunchType.SelectedIndexChanged += new EventHandler(cmbExplorerLaunchType_SelectedIndexChanged);
        }

        void cmbExplorerLaunchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        private void FillFileTypeAssociations()
        {
            pnlAudioFiles.Padding = new Padding(0);
            pnlVideoFiles.Padding = new Padding(0);
            pnlPlaylists.Padding = new Padding(0);
            pnlAudioFiles.Margin = new Padding(0);
            pnlVideoFiles.Margin = new Padding(0);
            pnlPlaylists.Margin = new Padding(0);

            foreach (string str in SupportedFileProvider.Instance.SupportedAudioTypes)
            {
                bool isRegistered = RegistrationSupport.IsFileTypeRegistered(str);
                string type = str.ToUpperInvariant();

                OPMCheckBox cb = CreateCheckBox(type, isRegistered);
                pnlAudioFiles.Controls.Add(cb);
            }

            foreach (string str in SupportedFileProvider.Instance.SupportedVideoTypes)
            {
                bool isRegistered = RegistrationSupport.IsFileTypeRegistered(str);
                string type = str.ToUpperInvariant();

                OPMCheckBox cb = CreateCheckBox(type, isRegistered);
                pnlVideoFiles.Controls.Add(cb);
            }

            foreach (string str in SupportedFileProvider.Instance.SupportedPlaylists)
            {
                bool isRegistered = RegistrationSupport.IsFileTypeRegistered(str);
                string type = str.ToUpperInvariant();

                OPMCheckBox cb = CreateCheckBox(type, isRegistered);
                pnlPlaylists.Controls.Add(cb);
            }
        }

        private void InitializeComponent()
        {
            this.label3 = new OPMedia.UI.Controls.OPMLabel();
            this.cmbExplorerLaunchType = new OPMedia.UI.Controls.OPMComboBox();
            this.btnSelAllAudio = new OPMedia.UI.Controls.OPMButton();
            this.btnUnselAllAudio = new OPMedia.UI.Controls.OPMButton();
            this.btnUnselAllVideo = new OPMedia.UI.Controls.OPMButton();
            this.btnSelAllVideo = new OPMedia.UI.Controls.OPMButton();
            this.btnUnselAllPlaylists = new OPMedia.UI.Controls.OPMButton();
            this.btnSelAllPlaylists = new OPMedia.UI.Controls.OPMButton();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.hdrAudio = new OPMedia.UI.Controls.OPMHeaderLabel();
            this.hdrPlaylists = new OPMedia.UI.Controls.OPMHeaderLabel();
            this.hdrVideo = new OPMedia.UI.Controls.OPMHeaderLabel();
            this.tableLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.pnlPlaylists = new OPMedia.UI.Controls.OPMFlowLayoutPanel();
            this.pnlVideoFiles = new OPMedia.UI.Controls.OPMFlowLayoutPanel();
            this.pnlAudioFiles = new OPMedia.UI.Controls.OPMFlowLayoutPanel();
            this.lblFileTypes = new OPMedia.UI.Controls.OPMLabel();
            this.opmLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.chkShellExtension = new OPMedia.UI.Controls.OPMCheckBox();
            this.pnlButtons.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.opmLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Location = new System.Drawing.Point(0, 30);
            this.label3.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.label3.Name = "label3";
            this.label3.OverrideBackColor = System.Drawing.Color.Empty;
            this.label3.OverrideForeColor = System.Drawing.Color.Empty;
            this.label3.Size = new System.Drawing.Size(573, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "TXT_HANDLEEXPLORERLAUNCH";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbExplorerLaunchType
            // 
            this.cmbExplorerLaunchType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbExplorerLaunchType.FormattingEnabled = true;
            this.cmbExplorerLaunchType.Location = new System.Drawing.Point(0, 50);
            this.cmbExplorerLaunchType.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.cmbExplorerLaunchType.Name = "cmbExplorerLaunchType";
            this.cmbExplorerLaunchType.OverrideForeColor = System.Drawing.Color.Empty;
            this.cmbExplorerLaunchType.Size = new System.Drawing.Size(275, 24);
            this.cmbExplorerLaunchType.TabIndex = 3;
            // 
            // btnSelAllAudio
            // 
            this.btnSelAllAudio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelAllAudio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelAllAudio.Location = new System.Drawing.Point(2, 2);
            this.btnSelAllAudio.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.btnSelAllAudio.Name = "btnSelAllAudio";
            this.btnSelAllAudio.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnSelAllAudio.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnSelAllAudio.ShowDropDown = false;
            this.btnSelAllAudio.Size = new System.Drawing.Size(135, 22);
            this.btnSelAllAudio.TabIndex = 0;
            this.btnSelAllAudio.Text = "TXT_SELECT_ALLAUDIO";
            this.btnSelAllAudio.Click += new System.EventHandler(this.btnSelAllAudio_Click);
            // 
            // btnUnselAllAudio
            // 
            this.btnUnselAllAudio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUnselAllAudio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnselAllAudio.Location = new System.Drawing.Point(2, 26);
            this.btnUnselAllAudio.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.btnUnselAllAudio.Name = "btnUnselAllAudio";
            this.btnUnselAllAudio.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnUnselAllAudio.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnUnselAllAudio.ShowDropDown = false;
            this.btnUnselAllAudio.Size = new System.Drawing.Size(135, 22);
            this.btnUnselAllAudio.TabIndex = 3;
            this.btnUnselAllAudio.Text = "TXT_UNSELECT_ALLAUDIO";
            this.btnUnselAllAudio.Click += new System.EventHandler(this.btnUnselAllAudio_Click);
            // 
            // btnUnselAllVideo
            // 
            this.btnUnselAllVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUnselAllVideo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnselAllVideo.Location = new System.Drawing.Point(141, 26);
            this.btnUnselAllVideo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.btnUnselAllVideo.Name = "btnUnselAllVideo";
            this.btnUnselAllVideo.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnUnselAllVideo.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnUnselAllVideo.ShowDropDown = false;
            this.btnUnselAllVideo.Size = new System.Drawing.Size(135, 22);
            this.btnUnselAllVideo.TabIndex = 4;
            this.btnUnselAllVideo.Text = "TXT_UNSELECT_ALLVIDEO";
            this.btnUnselAllVideo.Click += new System.EventHandler(this.btnUnselAllVideo_Click);
            // 
            // btnSelAllVideo
            // 
            this.btnSelAllVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelAllVideo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelAllVideo.Location = new System.Drawing.Point(141, 2);
            this.btnSelAllVideo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.btnSelAllVideo.Name = "btnSelAllVideo";
            this.btnSelAllVideo.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnSelAllVideo.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnSelAllVideo.ShowDropDown = false;
            this.btnSelAllVideo.Size = new System.Drawing.Size(135, 22);
            this.btnSelAllVideo.TabIndex = 1;
            this.btnSelAllVideo.Text = "TXT_SELECT_ALLVIDEO";
            this.btnSelAllVideo.Click += new System.EventHandler(this.btnSelAllVideo_Click);
            // 
            // btnUnselAllPlaylists
            // 
            this.btnUnselAllPlaylists.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUnselAllPlaylists.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnselAllPlaylists.Location = new System.Drawing.Point(279, 26);
            this.btnUnselAllPlaylists.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.btnUnselAllPlaylists.Name = "btnUnselAllPlaylists";
            this.btnUnselAllPlaylists.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnUnselAllPlaylists.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnUnselAllPlaylists.ShowDropDown = false;
            this.btnUnselAllPlaylists.Size = new System.Drawing.Size(135, 22);
            this.btnUnselAllPlaylists.TabIndex = 5;
            this.btnUnselAllPlaylists.Text = "TXT_UNSELECT_ALLPLAYLISTS";
            this.btnUnselAllPlaylists.Click += new System.EventHandler(this.btnUnselAllPlaylists_Click);
            // 
            // btnSelAllPlaylists
            // 
            this.btnSelAllPlaylists.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelAllPlaylists.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelAllPlaylists.Location = new System.Drawing.Point(279, 2);
            this.btnSelAllPlaylists.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.btnSelAllPlaylists.Name = "btnSelAllPlaylists";
            this.btnSelAllPlaylists.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnSelAllPlaylists.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnSelAllPlaylists.ShowDropDown = false;
            this.btnSelAllPlaylists.Size = new System.Drawing.Size(135, 22);
            this.btnSelAllPlaylists.TabIndex = 2;
            this.btnSelAllPlaylists.Text = "TXT_SELECT_ALLPLAYLISTS";
            this.btnSelAllPlaylists.Click += new System.EventHandler(this.btnSelAllPlaylists_Click);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnSelAllAudio);
            this.pnlButtons.Controls.Add(this.btnUnselAllPlaylists);
            this.pnlButtons.Controls.Add(this.btnUnselAllAudio);
            this.pnlButtons.Controls.Add(this.btnSelAllPlaylists);
            this.pnlButtons.Controls.Add(this.btnSelAllVideo);
            this.pnlButtons.Controls.Add(this.btnUnselAllVideo);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlButtons.Location = new System.Drawing.Point(0, 253);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(573, 50);
            this.pnlButtons.TabIndex = 6;
            // 
            // hdrAudio
            // 
            this.hdrAudio.AutoSize = true;
            this.hdrAudio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hdrAudio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.hdrAudio.Location = new System.Drawing.Point(0, 0);
            this.hdrAudio.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.hdrAudio.MaximumSize = new System.Drawing.Size(4000, 20);
            this.hdrAudio.MinimumSize = new System.Drawing.Size(4, 20);
            this.hdrAudio.Name = "hdrAudio";
            this.hdrAudio.Size = new System.Drawing.Size(573, 20);
            this.hdrAudio.TabIndex = 2;
            this.hdrAudio.Text = "TXT_AUDIO_FILES";
            this.hdrAudio.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // hdrPlaylists
            // 
            this.hdrPlaylists.AutoSize = true;
            this.hdrPlaylists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hdrPlaylists.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.hdrPlaylists.Location = new System.Drawing.Point(0, 60);
            this.hdrPlaylists.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.hdrPlaylists.MaximumSize = new System.Drawing.Size(4000, 20);
            this.hdrPlaylists.MinimumSize = new System.Drawing.Size(4, 20);
            this.hdrPlaylists.Name = "hdrPlaylists";
            this.hdrPlaylists.Size = new System.Drawing.Size(573, 20);
            this.hdrPlaylists.TabIndex = 2;
            this.hdrPlaylists.Text = "TXT_PLAYLISTS";
            this.hdrPlaylists.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // hdrVideo
            // 
            this.hdrVideo.AutoSize = true;
            this.hdrVideo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hdrVideo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.hdrVideo.Location = new System.Drawing.Point(0, 30);
            this.hdrVideo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.hdrVideo.MaximumSize = new System.Drawing.Size(4000, 20);
            this.hdrVideo.MinimumSize = new System.Drawing.Size(4, 20);
            this.hdrVideo.Name = "hdrVideo";
            this.hdrVideo.Size = new System.Drawing.Size(573, 20);
            this.hdrVideo.TabIndex = 1;
            this.hdrVideo.Text = "TXT_VIDEO_FILES";
            this.hdrVideo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.hdrAudio, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlPlaylists, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.hdrPlaylists, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.pnlVideoFiles, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.hdrVideo, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.pnlAudioFiles, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 99);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(573, 149);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // pnlPlaylists
            // 
            this.pnlPlaylists.AutoSize = true;
            this.pnlPlaylists.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlPlaylists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPlaylists.Location = new System.Drawing.Point(0, 85);
            this.pnlPlaylists.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.pnlPlaylists.Name = "pnlPlaylists";
            this.pnlPlaylists.Size = new System.Drawing.Size(573, 59);
            this.pnlPlaylists.TabIndex = 4;
            // 
            // pnlVideoFiles
            // 
            this.pnlVideoFiles.AutoSize = true;
            this.pnlVideoFiles.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlVideoFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlVideoFiles.Location = new System.Drawing.Point(0, 55);
            this.pnlVideoFiles.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.pnlVideoFiles.Name = "pnlVideoFiles";
            this.pnlVideoFiles.Size = new System.Drawing.Size(573, 1);
            this.pnlVideoFiles.TabIndex = 3;
            // 
            // pnlAudioFiles
            // 
            this.pnlAudioFiles.AutoSize = true;
            this.pnlAudioFiles.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlAudioFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAudioFiles.Location = new System.Drawing.Point(0, 25);
            this.pnlAudioFiles.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.pnlAudioFiles.Name = "pnlAudioFiles";
            this.pnlAudioFiles.Size = new System.Drawing.Size(573, 1);
            this.pnlAudioFiles.TabIndex = 2;
            // 
            // lblFileTypes
            // 
            this.lblFileTypes.AutoSize = true;
            this.lblFileTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFileTypes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblFileTypes.Location = new System.Drawing.Point(0, 79);
            this.lblFileTypes.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblFileTypes.Name = "lblFileTypes";
            this.lblFileTypes.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblFileTypes.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblFileTypes.Size = new System.Drawing.Size(573, 15);
            this.lblFileTypes.TabIndex = 4;
            this.lblFileTypes.Text = "TXT_S_FILETYPES_ASSOCIATIONS";
            this.lblFileTypes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // opmLayoutPanel1
            // 
            this.opmLayoutPanel1.ColumnCount = 1;
            this.opmLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.opmLayoutPanel1.Controls.Add(this.cmbExplorerLaunchType, 0, 4);
            this.opmLayoutPanel1.Controls.Add(this.lblFileTypes, 0, 5);
            this.opmLayoutPanel1.Controls.Add(this.tableLayoutPanel1, 0, 6);
            this.opmLayoutPanel1.Controls.Add(this.pnlButtons, 0, 7);
            this.opmLayoutPanel1.Controls.Add(this.chkShellExtension, 0, 0);
            this.opmLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.opmLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.opmLayoutPanel1.Name = "opmLayoutPanel1";
            this.opmLayoutPanel1.RowCount = 8;
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmLayoutPanel1.Size = new System.Drawing.Size(573, 303);
            this.opmLayoutPanel1.TabIndex = 0;
            // 
            // chkShellExtension
            // 
            this.chkShellExtension.AutoSize = true;
            this.chkShellExtension.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkShellExtension.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkShellExtension.Location = new System.Drawing.Point(3, 3);
            this.chkShellExtension.Name = "chkShellExtension";
            this.chkShellExtension.OverrideForeColor = System.Drawing.Color.Empty;
            this.chkShellExtension.Size = new System.Drawing.Size(567, 19);
            this.chkShellExtension.TabIndex = 7;
            this.chkShellExtension.Text = "TXT_SHELL_INTEGRATION";
            this.chkShellExtension.UseVisualStyleBackColor = true;
            this.chkShellExtension.CheckedChanged += OnCheckedChanged;
            // 
            // FileTypesPanel
            // 
            this.Controls.Add(this.opmLayoutPanel1);
            this.Name = "FileTypesPanel";
            this.Size = new System.Drawing.Size(573, 303);
            this.pnlButtons.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.opmLayoutPanel1.ResumeLayout(false);
            this.opmLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void cbShellIntegration_CheckStateChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        protected override void SaveInternal()
        {
            if (!Modified)
            {
                return;
            }

            SaveFileTypes(pnlAudioFiles);
            SaveFileTypes(pnlVideoFiles);
            SaveFileTypes(pnlPlaylists);

            ProTONEConfig.ExplorerLaunchType =
                (cmbExplorerLaunchType.SelectedItem as ExplorerLaunchType).CommandType.ToString();

            if (chkShellExtension.Checked)
                RegistrationSupport.RegisterShellExtension();
            else
                RegistrationSupport.UnregisterShellExtension();

            RegistrationSupport.ReloadFileAssociations();
        }

        private void SaveFileTypes(OPMFlowLayoutPanel pnl)
        {
            foreach (Control ctl in pnl.Controls)
            {
                OPMCheckBox cb = ctl as OPMCheckBox;
                if (cb != null)
                {
                    string fileType = cb.Text;
                    bool isRegistered = RegistrationSupport.IsFileTypeRegistered(fileType);

                    if (cb.Checked != isRegistered)
                    {
                        if (cb.Checked)
                        {
                            RegistrationSupport.RegisterFileType(fileType, false);
                        }
                        else
                        {
                            RegistrationSupport.UnregisterFileType(fileType, false);
                        }
                    }
                }
            }
        }

        internal class ExplorerLaunchType
        {
            public CommandType CommandType = CommandType.EnqueueFiles;

            public override string ToString()
            {
                return Translator.Translate("TXT_" + CommandType.ToString().ToUpperInvariant());
            }

            public override bool Equals(object obj)
            {
                if (obj as ExplorerLaunchType != null)
                {
                    return (obj as ExplorerLaunchType).CommandType == this.CommandType;
                }

                return false;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public ExplorerLaunchType(CommandType CommandType)
            {
                this.CommandType = CommandType;
            }
        }

        private void btnSelAllAudio_Click(object sender, EventArgs e)
        {
            MarkAllInGroup(true, pnlAudioFiles);
        }

        private void btnUnselAllAudio_Click(object sender, EventArgs e)
        {
            MarkAllInGroup(false, pnlAudioFiles);
        }

        private void btnSelAllVideo_Click(object sender, EventArgs e)
        {
            MarkAllInGroup(true, pnlVideoFiles);
        }

        private void btnUnselAllVideo_Click(object sender, EventArgs e)
        {
            MarkAllInGroup(false, pnlVideoFiles);
        }

        private void btnSelAllPlaylists_Click(object sender, EventArgs e)
        {
            MarkAllInGroup(true, pnlPlaylists);
        }

        private void btnUnselAllPlaylists_Click(object sender, EventArgs e)
        {
            MarkAllInGroup(false, pnlPlaylists);
        }

        private void MarkAllInGroup(bool check, OPMFlowLayoutPanel pnl)
        {
            foreach (Control ctl in pnl.Controls)
            {
                OPMCheckBox cb = ctl as OPMCheckBox;
                if (cb != null)
                {
                    cb.Checked = check;
                }
            }
        }




    }
}


