using OPMedia.Core.GlobalEvents;
using OPMedia.Core.TranslationSupport;
using OPMedia.Runtime.ProTONE;
using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.UI.Controls;
using OPMedia.UI.ProTONE.SubtitleDownload;
using System;
using System.Collections.Generic;

namespace OPMedia.Addons.Builtin.VideoProp
{
    public partial class AddonPanel : OPMedia.Runtime.Addons.AddonsBase.Prop.PropBaseCtl
    {
        List<object> lvi = null;
        List<string> strItems = null;

        public override string GetHelpTopic()
        {
            return "VideoPropertyPanel";
        }

        public AddonPanel()
            : base()
        {
            InitializeComponent();
            this.HandleCreated += new EventHandler(AddonPanel_HandleCreated);

        }

        void AddonPanel_HandleCreated(object sender, EventArgs e)
        {
            btnSearchSubtitles.Text = Translator.Translate("TXT_SEARCH_SUBTITLES");
        }

        public override bool CanHandleFolders
        {
            get
            {
                return false;
            }
        }

        public override List<string> HandledFileTypes
        {
            get
            {
                return SupportedFileProvider.Instance.SupportedVideoTypes;
            }
        }

        public override int MaximumHandledItems
        {
            get
            {
                return -1;
            }
        }

        public override void ShowProperties(List<string> strItems, object additionalData)
        {
            this.strItems = strItems;
            DoShowProperties();
        }

        private void DoShowProperties()
        {
            lvi = new List<object>();
            foreach (string item in strItems)
            {
                VideoFileInfo vi = VideoFileInfo.FromPath(item)
                    as VideoFileInfo;
                if (vi.IsValid)
                {
                    lvi.Add(vi);
                }
            }

            FileAttributesBrowser.ProcessObjectAttributes(lvi);

            pgProperties.SelectedObjects = lvi.ToArray();

            btnSearchSubtitles.Enabled = false;
            if (lvi.Count == 1)
            {
                VideoFileInfo vfi = lvi[0] as VideoFileInfo;
                if (vfi != null)
                {
                    btnSearchSubtitles.Enabled =
                        SubtitleDownloadProcessor.CanPerformSubtitleDownload(vfi.Path,
                        (int)vfi.Duration.GetValueOrDefault().TotalSeconds);
                }
            }
        }

        [EventSink(OPMedia.Core.EventNames.PerformTranslation)]
        public new void OnPerformTranslation()
        {
            btnSearchSubtitles.Text = Translator.Translate("TXT_SEARCH_SUBTITLES");
        }

        private void btnSearchSubtitles_Click(object sender, EventArgs e)
        {
            VideoFileInfo vi = pgProperties.SelectedObject as VideoFileInfo;
            if (vi != null)
            {
                SubtitleDownloadProcessor.TryFindSubtitle(vi.Path, (int)vi.Duration.GetValueOrDefault().TotalSeconds, true);
            }
        }
    }
}

