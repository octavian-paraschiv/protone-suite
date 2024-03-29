using OPMedia.Core;
using OPMedia.Core.Configuration;
using OPMedia.Core.Logging;
using OPMedia.Core.TranslationSupport;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.Rendering;
using OPMedia.Runtime.SystemScheduler;
using OPMedia.UI.Dialogs;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace OPMedia.Runtime.ProTONE
{
    public sealed class SystemScheduler : HeartbeatConsumer
    {
        private static SystemScheduler _instance = null;

        private bool _playlistEventEnabled = false;
        private static object _playlistSync = new object();

        ManualResetEvent _isPlaylistEvent = new ManualResetEvent(false);
        ManualResetEvent _isScheduledEvent = new ManualResetEvent(false);

        public static bool PlaylistEventEnabled
        {
            get
            {
                lock (_playlistSync)
                {
                    return (_instance != null) && _instance._playlistEventEnabled;
                }
            }

            set
            {
                lock (_playlistSync)
                {
                    if (_instance == null)
                    {
                        Start();
                    }

                    if (_instance != null)
                    {
                        _instance._playlistEventEnabled = value;
                    }
                }
            }
        }

        public static void Start()
        {
            if (_instance == null)
            {
                _instance = new SystemScheduler();
                Logger.LogInfo("Timer scheduler has been started.");
            }
        }

        public static void Stop()
        {
            if (_instance != null)
            {
                _instance.StopAndDisconnect();
                Logger.LogInfo("Timer scheduler has been stopped.");
            }
        }

        private SystemScheduler()
            : base("timer scheduler")
        {
        }

        protected override bool InitTask()
        {
            return true;
        }

        protected override void CleanupTask()
        {
            _isPlaylistEvent.Reset();
            _isScheduledEvent.Reset();
        }

        protected override void ExecuteTask()
        {
            CheckForPlaylistEvent();
            CheckForScheduledEvent();
        }

        private void CheckForPlaylistEvent()
        {
            if (RenderingEngine.DefaultInstance.PlaylistAtEnd)
            {
                RenderingEngine.DefaultInstance.PlaylistAtEnd = false;

                if (!_isPlaylistEvent.WaitOne(50, true))
                {
                    _isPlaylistEvent.Set();

                    if (PlaylistEventEnabled)
                    {
                        SchedulerAction sat = (SchedulerAction)ProTONEConfig.PlaylistEventHandler;

                        // Apply the language ID on the timer scheduler thread, 
                        // otherwise the translator will default to English
                        Translator.SetInterfaceLanguage(AppConfig.LanguageID);

                        string msg = Translator.Translate("TXT_PLAYLIST_END_MSG");

                        Logger.LogInfo("The playlist has reached the end. Action to execute: " + sat);
                        PreProcessEvent(sat, msg);
                    }
                }
            }
            else
            {
                _isPlaylistEvent.Reset();
            }
        }

        private void CheckForScheduledEvent()
        {
            bool matchDay = false;
            bool matchHour = false;

            DateTime now = DateTime.Now;

            TimeSpan diff = now.TimeOfDay.Subtract(ProTONEConfig.ScheduledEventTime);
            if (Math.Abs(diff.TotalSeconds) <= 3f)
            {
                matchHour = true;

                if (diff.TotalSeconds < 0)
                {
                    int sleep = (int)(Math.Abs(diff.TotalSeconds) * 1000);
                    Thread.Sleep(sleep);
                }
            }

            Weekday scheduledWeekDay = (Weekday)ProTONEConfig.ScheduledEventDays;

            if (scheduledWeekDay == Weekday.Everyday)
            {
                matchDay = true;
            }
            else
            {
                switch (now.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        matchDay = (scheduledWeekDay & Weekday.Monday) == Weekday.Monday;
                        break;
                    case DayOfWeek.Tuesday:
                        matchDay = (scheduledWeekDay & Weekday.Tuesday) == Weekday.Tuesday;
                        break;
                    case DayOfWeek.Wednesday:
                        matchDay = (scheduledWeekDay & Weekday.Wednesday) == Weekday.Wednesday;
                        break;
                    case DayOfWeek.Thursday:
                        matchDay = (scheduledWeekDay & Weekday.Thursday) == Weekday.Thursday;
                        break;
                    case DayOfWeek.Friday:
                        matchDay = (scheduledWeekDay & Weekday.Friday) == Weekday.Friday;
                        break;
                    case DayOfWeek.Saturday:
                        matchDay = (scheduledWeekDay & Weekday.Saturday) == Weekday.Saturday;
                        break;
                    case DayOfWeek.Sunday:
                        matchDay = (scheduledWeekDay & Weekday.Sunday) == Weekday.Sunday;
                        break;
                }
            }


            if (matchDay && matchHour)
            {
                if (!_isScheduledEvent.WaitOne(50, true))
                {
                    _isScheduledEvent.Set();

                    if (ProTONEConfig.EnableScheduledEvent)
                    {
                        SchedulerAction sat = (SchedulerAction)ProTONEConfig.ScheduledEventHandler;

                        // Apply the language ID on the timer scheduler thread, 
                        // otherwise the translator will default to English
                        Translator.SetInterfaceLanguage(AppConfig.LanguageID);

                        string msg = Translator.Translate("TXT_SCHEDULED_EVENT_MSG");

                        Logger.LogInfo("The scheduled moment has occurred. Action to execute: " + sat);
                        PreProcessEvent(sat, msg);
                    }
                }
            }
            else
            {
                _isScheduledEvent.Reset();
            }
        }

        volatile SchedulerAction _action = SchedulerAction.None;
        ManualResetEvent _canProceed = new ManualResetEvent(false);

        private void PreProcessEvent(SchedulerAction action, string msg)
        {
            _action = action;

            _canProceed.Reset();

            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessEvent));

            if (ProTONEConfig.SchedulerWaitTimerProceed > 0)
            {
                _action = SchedulerAction.None;

                if (action != SchedulerAction.None)
                {
                    Logger.LogInfo("Asking the user how to continue with action: " + action);

                    bool res = false;

                    MainThread.Send(delegate (object x)
                    {
                        res = QueryScheduledAction(action, msg);

                    });

                    if (res)
                    {
                        Logger.LogInfo("The user has chosen to continue with the action");
                        _action = action;
                    }
                    else
                    {
                        Logger.LogInfo("The user has chosen to abort the action");
                    }
                }
            }

            _canProceed.Set();
        }

        private bool QueryScheduledAction(SchedulerAction action, string msg)
        {
            string actionType = string.Empty;
            switch (action)
            {
                case SchedulerAction.Shutdown:
                    actionType = Translator.Translate("TXT_SHUTDOWN_PC");
                    break;

                case SchedulerAction.StandBy:
                    actionType = Translator.Translate("TXT_STANDBY_PC");
                    break;

                case SchedulerAction.Hibernate:
                    actionType = Translator.Translate("TXT_HIBERNATE_PC");
                    break;

                case SchedulerAction.None:
                default:
                    return false;

            }

            //RestoreWindowFromTray();

            string info = Translator.Translate("TXT_PROCEED_SCHEDULED_ACTION", actionType, msg);
            TimerWaitingDialog dlg = new TimerWaitingDialog(
                "TXT_SCHEDULED_ACTION_PROCEED", info, 60 * ProTONEConfig.SchedulerWaitTimerProceed);
            DialogResult res = dlg.ShowDialog();

            return (res == DialogResult.Yes);
        }

        private void ProcessEvent(object inf)
        {

            // Wait here for proceed signal.
            while (true)
            {
                if (_canProceed.WaitOne(10, true))
                    break;
            }

            if (_action != SchedulerAction.None)
            {
                Logger.LogInfo("Proceeding with action: " + _action);

                try
                {
                    switch (_action)
                    {
                        case SchedulerAction.Shutdown:
                            Process.Start("shutdown.exe", "-s -t 1");
                            break;

                        case SchedulerAction.StandBy:
                            Application.SetSuspendState(PowerState.Suspend, true, false);
                            break;

                        case SchedulerAction.Hibernate:
                            Application.SetSuspendState(PowerState.Hibernate, true, false);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ErrorDispatcher.DispatchError(ex, true);
                }
            }
        }
    }
}
