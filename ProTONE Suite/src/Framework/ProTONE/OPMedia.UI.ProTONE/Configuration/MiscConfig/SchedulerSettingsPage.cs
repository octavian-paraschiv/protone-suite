using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using OPMedia.UI.Configuration;
using OPMedia.Runtime.ProTONE;
using OPMedia.Core.Configuration;
using OPMedia.Core.TranslationSupport;
using OPMedia.Runtime;
using OPMedia.UI.Controls;
using System.Threading;
using OPMedia.Runtime.Shortcuts;
using OPMedia.UI.ProTONE.Properties;
using OPMedia.UI.Themes;
using OPMedia.Core.GlobalEvents;
using OPMedia.UI.ProTONE.GlobalEvents;
using OPMedia.Runtime.ProTONE.Configuration;

namespace OPMedia.UI.ProTONE.Configuration.MiscConfig
{
    public partial class SchedulerSettingsPage : BaseCfgPanel
    {
        public override Image Image
        {
            get
            {
                return Resources.Scheduler;
            }
        }

        public SchedulerSettingsPage(): base()
        {
            this.Title = "TXT_S_SCHEDULERSETTINGS";
            InitializeComponent();

            ApplyColors();

            this.Load += new EventHandler(SchedulerSettingsPanel_Load);
            opmLayoutPanel1.Resize += new EventHandler(OnResize);
        }

        void OnResize(object sender, EventArgs e)
        {
            layoutPanel.AutoScrollMinSize = new Size(
                opmLayoutPanel1.Width - SystemInformation.VerticalScrollBarWidth,
                opmLayoutPanel1.Height - SystemInformation.HorizontalScrollBarHeight);
        }

        protected override void OnThemeUpdatedInternal()
        {
            base.OnThemeUpdatedInternal();
            ApplyColors();
        }


        public void ApplyColors()
        {
            lblSep1.OverrideBackColor =
            lblSep2.OverrideBackColor = 
            lblSep3.OverrideBackColor = ThemeManager.ForeColor;
            lblCaution.OverrideForeColor = ThemeManager.ColorValidationFailed;
        }

        void SchedulerSettingsPanel_Load(object sender, EventArgs e)
        {
            dtpScheduledEvtTime.CustomFormat =
                Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortTimePattern;

            cmbPlaylistEvtHandler.Items.Clear();
            cmbScheduledEvtHandler.Items.Clear();
            foreach (ScheduledActionType act in Enum.GetValues(typeof(ScheduledActionType)))
            {
                string str = string.Format("TXT_ACT_{0}", act.ToString().ToUpperInvariant());
                cmbPlaylistEvtHandler.Items.Add(Translator.Translate(str));
                cmbScheduledEvtHandler.Items.Add(Translator.Translate(str));
            }

            chkEnablePlaylistEvt.Checked = SystemScheduler.PlaylistEventEnabled;

            cmbPlaylistEvtHandler.SelectedIndex = ProTONEConfig.PlaylistEventHandler;
            psiPlaylistEvtData.ProgramStartupInfo = 
                ProgramStartupInfo.FromString(ProTONEConfig.PlaylistEventData);

            chkEnableScheduledEvt.Checked = ProTONEConfig.EnableScheduledEvent;

            cmbScheduledEvtHandler.SelectedIndex = ProTONEConfig.ScheduledEventHandler;
            psiScheduledEvtData.ProgramStartupInfo =
                ProgramStartupInfo.FromString(ProTONEConfig.ScheduledEventData);
            wsScheduledEvtDays.Weekdays = (Weekday)ProTONEConfig.ScheduledEventDays;

            DateTime dtDisplay = DateTime.Today;
            TimeSpan tsTimeOfDay = 
                new TimeSpan(ProTONEConfig.ScheduledEventTime.Hours, ProTONEConfig.ScheduledEventTime.Minutes, ProTONEConfig.ScheduledEventTime.Seconds);
            dtDisplay = dtDisplay.Add(tsTimeOfDay);

            dtpScheduledEvtTime.Value = dtDisplay;

            nudSchedulerWaitTimerProceed.Value = ProTONEConfig.SchedulerWaitTimerProceed;

            ManageVisibility();
            SubscribeAll();
        }

        protected override void SaveInternal()
        {
            SystemScheduler.PlaylistEventEnabled = chkEnablePlaylistEvt.Checked;
            
            ProTONEConfig.PlaylistEventHandler =  cmbPlaylistEvtHandler.SelectedIndex;
            ProTONEConfig.PlaylistEventData =     psiPlaylistEvtData.GetProgramStartupInfo();

            ProTONEConfig.EnableScheduledEvent =  chkEnableScheduledEvt.Checked;
            ProTONEConfig.ScheduledEventHandler = cmbScheduledEvtHandler.SelectedIndex;
            ProTONEConfig.ScheduledEventData =    psiScheduledEvtData.GetProgramStartupInfo();
            ProTONEConfig.ScheduledEventDays =    (int)wsScheduledEvtDays.Weekdays;
            ProTONEConfig.ScheduledEventTime =
                new TimeSpan(dtpScheduledEvtTime.Value.TimeOfDay.Hours, dtpScheduledEvtTime.Value.TimeOfDay.Minutes, 0);

            ProTONEConfig.SchedulerWaitTimerProceed = (int)nudSchedulerWaitTimerProceed.Value;

            
        }

        private void OnSettingsChanged(object sender, EventArgs e)
        {
            try
            {
                Modified = true;

                if (sender == chkEnablePlaylistEvt)
                {
                    ShortcutMapper.DispatchCommand(OPMShortcut.CmdPlaylistEnd);
                }

                UnsubscribeAll();

                SystemScheduler.PlaylistEventEnabled = chkEnablePlaylistEvt.Checked;
                ManageVisibility();

            }
            finally
            {
                SubscribeAll();
            }
        }

        private void ManageVisibility()
        {
            grpPlaylistEvt.Enabled = chkEnablePlaylistEvt.Checked;
            grpPlaylistEvt.Visible = chkEnablePlaylistEvt.Checked;
            lblSep1.Visible = chkEnablePlaylistEvt.Checked;

            psiPlaylistEvtData.Visible =
               (cmbPlaylistEvtHandler.SelectedIndex == (int)ScheduledActionType.LaunchProgram);

            grpScheduledEvt.Enabled = chkEnableScheduledEvt.Checked;
            grpScheduledEvt.Visible = chkEnableScheduledEvt.Checked;
            lblSep2.Visible = chkEnableScheduledEvt.Checked;

            psiScheduledEvtData.Visible =
               (cmbScheduledEvtHandler.SelectedIndex == (int)ScheduledActionType.LaunchProgram);

            pnlProceedTimerOptions.Visible = (chkEnablePlaylistEvt.Checked || chkEnableScheduledEvt.Checked);
            lblSep3.Visible = (chkEnablePlaylistEvt.Checked || chkEnableScheduledEvt.Checked);
        }

        private void SubscribeAll()
        {
            UnsubscribeAll();

            wsScheduledEvtDays.InfoChanged += new System.EventHandler(this.OnSettingsChanged);
            psiPlaylistEvtData.InfoChanged += new System.EventHandler(this.OnSettingsChanged);
            cmbPlaylistEvtHandler.SelectedIndexChanged += new System.EventHandler(this.OnSettingsChanged);
            chkEnablePlaylistEvt.CheckedChanged += new System.EventHandler(this.OnSettingsChanged);
            dtpScheduledEvtTime.ValueChanged += new System.EventHandler(this.OnSettingsChanged);
            psiScheduledEvtData.InfoChanged += new System.EventHandler(this.OnSettingsChanged);
            chkEnableScheduledEvt.CheckedChanged += new System.EventHandler(this.OnSettingsChanged);
            cmbScheduledEvtHandler.SelectedIndexChanged += new System.EventHandler(this.OnSettingsChanged);
            nudSchedulerWaitTimerProceed.ValueChanged += new System.EventHandler(this.OnSettingsChanged);
        }

        private void UnsubscribeAll()
        {
            wsScheduledEvtDays.InfoChanged -= new System.EventHandler(this.OnSettingsChanged);
            psiPlaylistEvtData.InfoChanged -= new System.EventHandler(this.OnSettingsChanged);
            cmbPlaylistEvtHandler.SelectedIndexChanged -= new System.EventHandler(this.OnSettingsChanged);
            chkEnablePlaylistEvt.CheckedChanged -= new System.EventHandler(this.OnSettingsChanged);
            dtpScheduledEvtTime.ValueChanged -= new System.EventHandler(this.OnSettingsChanged);
            psiScheduledEvtData.InfoChanged -= new System.EventHandler(this.OnSettingsChanged);
            chkEnableScheduledEvt.CheckedChanged -= new System.EventHandler(this.OnSettingsChanged);
            cmbScheduledEvtHandler.SelectedIndexChanged -= new System.EventHandler(this.OnSettingsChanged);
            nudSchedulerWaitTimerProceed.ValueChanged -= new System.EventHandler(this.OnSettingsChanged);
        }
    }
}
