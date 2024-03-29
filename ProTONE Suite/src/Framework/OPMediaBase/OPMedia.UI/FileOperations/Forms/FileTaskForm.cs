using OPMedia.Core;
using OPMedia.Core.ComTypes;
using OPMedia.Core.Logging;
using OPMedia.Core.TranslationSupport;
using OPMedia.UI.Controls;
using OPMedia.UI.Themes;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OPMedia.UI.FileTasks
{
    public partial class FileTaskForm : ToolForm
    {
        protected string _taskDesc = string.Empty;
        protected string _taskProgress = string.Empty;

        private BaseFileTask _task = null;

        public bool RequiresRefresh
        {
            get
            {
                return _task.RequiresRefresh;
            }
        }

        public FileTaskType FileTaskType
        {
            get
            {
                return _task.TaskType;
            }
        }

        public string DestFolder { get; set; }

        public FileTaskForm(FileTaskType type, List<string> srcFiles, string srcPath)
        {
            InitializeComponent();

            _task = InitTask(type, srcFiles, srcPath);
            _task.FileTaskProgress += new FileTaskProgressDG(OnFileTaskProgress);

            this.Shown += new EventHandler(FileTaskForm_Shown);
            this.FormClosing += new FormClosingEventHandler(FileTaskForm_FormClosing);
        }

        void FileTaskForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If task is finished now, this is a Close following a task finish
            if (!_task.IsFinished)
            {
                // This will issue a task abort request
                OnCancel(null, null);

                // Don't close the form now ... let task finish first.
                e.Cancel = true;
            }
        }

        protected override bool AllowCloseOnKeyDown(Keys key)
        {
            // This will issue a task abort request
            OnCancel(null, null);

            // Don't close the form now ... let task finish first.
            return false;
        }

        protected virtual BaseFileTask InitTask(FileTaskType type, List<string> srcFiles, string srcPath)
        {
            switch (type)
            {
                case FileTaskType.Copy:
                    return new CopyFilesTask(srcFiles, srcPath);

                case FileTaskType.Move:
                    return new MoveFilesTask(srcFiles, srcPath);

                case FileTaskType.Delete:
                    return new DeleteFilesTask(srcFiles);
            }

            return null;
        }

        void FileTaskForm_Shown(object sender, EventArgs e)
        {
            _taskDesc = Translator.Translate(string.Format("TXT_DESC_{0}_TASK", _task.TaskType.ToString().ToUpperInvariant()));
            _taskProgress = Translator.Translate("TXT_CUR_PROGRESS");

            string title = string.Format("TXT_WAIT_{0}", _task.TaskType).ToUpperInvariant();
            SetTitle(Translator.Translate(title));

            btnCancel.Enabled = true;
            txtCurFile.Text = Translator.Translate("TXT_BUILDING_OBJECT_LIST");
            lblCurrentProgress.Text = string.Empty;
            pbOperation.Visible = false;

            Application.DoEvents();

            _task.RunTask(DestFolder);
        }

        void OnFileTaskProgress(ProgressEventType eventType, string file, UpdateProgressData data)
        {
            if (eventType == ProgressEventType.KeepAlive)
            {
                Application.DoEvents();
            }

            MainThread.Post(delegate (object x)
            {
                switch (eventType)
                {
                    case ProgressEventType.Started:
                        pbOperation.Value = 0;
                        pbOperation.Maximum = 10000;
                        pbOperation.Visible = (_task.TotalObjects > 1);
                        TaskbarThumbnailManager.Instance.SetProgressStatus(TaskbarProgressBarStatus.Indeterminate);
                        TaskbarThumbnailManager.Instance.UpdateProgress(0, 10000);
                        break;

                    case ProgressEventType.Aborted:
                        Close();
                        TaskbarThumbnailManager.Instance.SetProgressStatus(TaskbarProgressBarStatus.NoProgress);
                        break;

                    case ProgressEventType.Finished:
                        if (_task.ErrorMap.Count > 0)
                        {
                            string message = Translator.Translate(string.Format("TXT_ERRORS_{0}_TASK",
                                _task.TaskType.ToString().ToUpperInvariant()));

                            new FileTaskErrorReport(_task.ErrorMap,
                                Translator.Translate("TXT_ERRORS_ENCOUNTERED"), message).ShowDialog();

                            TaskbarThumbnailManager.Instance.SetProgressStatus(TaskbarProgressBarStatus.Error);
                        }
                        Close();
                        TaskbarThumbnailManager.Instance.SetProgressStatus(TaskbarProgressBarStatus.NoProgress);
                        break;

                    case ProgressEventType.KeepAlive:
                    case ProgressEventType.Progress:
                        txtCurFile.Text = file;
                        UpdateProgress(data);
                        break;
                }

            });
        }

        private void OnCancel(object sender, EventArgs e)
        {
            try
            {
                btnCancel.Enabled = false;
                _task.RequestAbort();
            }
            finally
            {
                btnCancel.Enabled = _task.CanContinue;
            }
        }

        private void UpdateProgress(UpdateProgressData data)
        {
            if (data.TotalFileSize > 0)
            {
                try
                {
                    long totalFileSize = data.TotalFileSize;
                    long totalBytesTransferred = data.TotalBytesTransferred;

                    double fractionTransfer = 0;

                    if (totalBytesTransferred > 0 && totalFileSize > 0)
                    {
                        fractionTransfer = (double)totalBytesTransferred / (double)totalFileSize;
                    }

                    long processedObjectCount = _task.ProcessedObjects;
                    long totalObjectCount = _task.TotalObjects;

                    int percent = 0;
                    if (data.IsDone)
                        percent = (int)((100 * processedObjectCount) / totalObjectCount);
                    else
                        percent = (int)((100 * (processedObjectCount + fractionTransfer)) / totalObjectCount);

                    pbCurrent.Value = 10000 * fractionTransfer;
                    pbOperation.Value = 100 * percent;

                    Logger.LogTrace($"Progress: percent={percent} proc={processedObjectCount} total={totalObjectCount} {data} file={txtCurFile.Text}");

                    lblCurrentProgress.Text = string.Format(_taskProgress, _taskDesc, processedObjectCount,
                            totalObjectCount, percent);

                    TaskbarThumbnailManager.Instance.SetProgressStatus(TaskbarProgressBarStatus.Normal);
                    TaskbarThumbnailManager.Instance.UpdateProgress((ulong)pbOperation.Value, (ulong)pbOperation.Maximum);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
        }

        private void OnVerifyTimer(object sender, EventArgs e)
        {
            //Logger.LogTrace("OnVerifyTimer: {0} ... {1}", lblCurrentProgress.Text, txtCurFile.Text);
        }
    }
}
