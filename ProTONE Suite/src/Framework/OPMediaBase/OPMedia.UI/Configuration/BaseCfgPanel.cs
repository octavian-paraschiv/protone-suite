using OPMedia.Core.TranslationSupport;
using OPMedia.UI.Controls;
using System;
using System.Drawing;


namespace OPMedia.UI.Configuration
{
    public class BaseCfgPanel : OPMBaseControl
    {
        public virtual Image Image
        {
            get
            {
                return null;
            }
        }

        public virtual string GetHelpTopic() => Name;

        public event EventHandler ModifiedActive = null;

        private bool _modified = false;
        public bool Modified
        {
            get
            {
                return _modified;
            }

            set
            {
                _modified = value;
                if (value && ModifiedActive != null)
                {
                    ModifiedActive(this, EventArgs.Empty);
                }
            }
        }

        public string Title { get; protected set; }

        public BaseCfgPanel()
        {
            this.HandleCreated += new EventHandler(BaseCfgPanel_HandleCreated);
        }

        void BaseCfgPanel_HandleCreated(object sender, EventArgs e)
        {
            if (!DesignMode)
                Translator.TranslateControl(this, DesignMode);
        }

        public void Save()
        {
            if (Modified)
            {
                SaveInternal();
            }
        }

        public void Discard()
        {
            if (Modified)
            {
                DiscardInternal();
            }
        }

        protected virtual void SaveInternal()
        {
        }

        protected virtual void DiscardInternal()
        {
        }

        public virtual Size? RequestedItemSize
        {
            get
            {
                return null;
            }
        }
    }
}
