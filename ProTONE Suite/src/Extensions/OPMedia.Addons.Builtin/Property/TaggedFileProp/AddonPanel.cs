using OPMedia.Addons.Builtin.Configuration;
using OPMedia.Addons.Builtin.Properties;
using OPMedia.Addons.Builtin.TaggedFileProp.TaggingWizard;
using OPMedia.Core.GlobalEvents;
using OPMedia.Core.Logging;
using OPMedia.Core.Shortcuts;
using OPMedia.Core.TranslationSupport;
using OPMedia.Runtime.Addons.AddonsBase.Prop;
using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.Runtime.Shortcuts;
using OPMedia.UI;
using OPMedia.UI.Controls;
using OPMedia.UI.Wizards;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OPMedia.Addons.Builtin.TaggedFileProp
{
    public partial class AddonPanel : PropBaseCtl
    {


        List<object> lii = null;
        List<string> strItems = null;

        private Timer _reloadTimer = null;

        public override string GetHelpTopic()
        {
            return "TaggedFilePropertyPanel";
        }

        public AddonPanel()
            : base()
        {
            InitializeComponent();
            this.HandleCreated += new EventHandler(AddonPanel_HandleCreated);
        }

        [EventSink(OPMedia.Core.EventNames.ExecuteShortcut)]
        public void OnExecuteShortcut(OPMShortcutEventArgs args)
        {
            if (args.Handled)
                return;

            switch (args.cmd)
            {
                case OPMShortcut.CmdGenericApply:
                    if (btnSave.Enabled) btnSave_Click(null, null);
                    args.Handled = true;
                    break;

                case OPMShortcut.CmdGenericUndo:
                    if (btnUndo.Enabled) btnUndo_Click(null, null);
                    args.Handled = true;
                    break;
            }
        }

        void AddonPanel_HandleCreated(object sender, EventArgs e)
        {
            Translator.TranslateControl(this, DesignMode);
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
                return TaggedMediaFileInfoFactory.TaggedFileTypes;
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

        public override void SaveProperties()
        {
            if (DoSaveProperties())
            {
                base.Modified = false;
                CheckButtonsState();
            }
        }

        private void pgProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            base.Modified = true;
            CheckButtonsState();
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            DoShowProperties();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveProperties();
        }

        private void DoShowProperties()
        {
            if (_reloadTimer == null)
            {
                _reloadTimer = new Timer();
                _reloadTimer.Interval = (int)(BuiltinAddonConfig.FEPreviewTimer * 1000);
                _reloadTimer.Tick += new EventHandler(_reloadTimer_Tick);
            }

            _reloadTimer.Stop();
            _reloadTimer.Start();

            InternalShowProperties(false);
        }

        void _reloadTimer_Tick(object sender, EventArgs e)
        {
            _reloadTimer.Stop();
            InternalShowProperties(true);
        }

        private void InternalShowProperties(bool deepLoad)
        {
            PerformTranslation();

            lii = new List<object>();
            foreach (string item in strItems)
            {
                ITaggedMediaFileInfo tmfi = TaggedMediaFileInfoFactory.GetTaggedMediaFileInfo(item, deepLoad && (strItems.Count == 1));
                if (tmfi != null && tmfi.IsValid)
                {
                    lii.Add(tmfi);
                }
            }

            FileAttributesBrowser.ProcessObjectAttributes(lii);

            pgProperties.SelectedObjects = lii.ToArray();
            base.Modified = false;

            CheckButtonsState();
        }

        private bool DoSaveProperties()
        {
            foreach (ITaggedMediaFileInfo tmfi in pgProperties.SelectedObjects)
            {
                try
                {
                    tmfi.Save();
                }
                catch (Exception ex)
                {
                    ErrorDispatcher.DispatchError(ex, false);
                }
            }
            return true;
        }

        private void CheckButtonsState()
        {
            btnSave.Enabled = base.Modified;
            btnUndo.Enabled = base.Modified;
            btnLaunchWizard.Visible = (pgProperties.SelectedObjects.Length > 0);
        }

        private void btnLaunchWizard_Click(object sender, EventArgs e)
        {
            Task task = new Task();
            task.Files = strItems;
            TaggingWizardMain.Execute(FindForm(), task);
        }

        private void PerformTranslation()
        {
            btnLaunchWizard.Text = Translator.Translate("TXT_LAUNCH_WIZARD");
            btnSave.Text = Translator.Translate("TXT_APPLY");
            btnUndo.Text = Translator.Translate("TXT_UNDO");
        }
    }

    public static class TaggingWizardMain
    {
        public static DialogResult Execute(Form parentForm, BackgroundTask initTask)
        {
            Type[] pages = new Type[]
                {
                    typeof(WizTagStep1Ctl),
                    typeof(WizTagStep2Ctl)
                };

            return WizardHostForm.CreateWizard("TXT_TAGGINGWIZARD", pages, true, initTask, Resources.Tagging);
        }

        public static DialogResult Execute(Form parentForm)
        {
            return Execute(parentForm, null);
        }
    }
}
