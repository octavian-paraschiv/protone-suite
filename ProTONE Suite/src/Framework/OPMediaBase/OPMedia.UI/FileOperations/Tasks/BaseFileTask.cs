using OPMedia.Core;
using OPMedia.Core.Logging;
using OPMedia.UI.FileOperations.Tasks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace OPMedia.UI.FileTasks
{
    #region Enums

    public enum FileTaskType
    {
        None = 0,
        Copy,
        Move,
        Delete
    }

    public enum ProgressEventType
    {
        Started = 0,
        KeepAlive,
        Progress,
        Aborted,
        Finished,
    }

    #endregion

    #region Delegates

    public delegate void FileTaskProgressDG(ProgressEventType eventType, string file, UpdateProgressData data);

    #endregion

    #region Helper classes

    public class TaskInterruptedException : Exception
    {
        public TaskInterruptedException(string msg)
            : base(msg)
        {
        }
    }

    public class UpdateProgressData
    {
        public static UpdateProgressData Empty = new UpdateProgressData();
        public static UpdateProgressData FileDone = new UpdateProgressData(100, 100);

        public long TotalFileSize { get; private set; }
        public long TotalBytesTransferred { get; private set; }

        public bool IsDone
        {
            get
            {
                return (TotalBytesTransferred >= TotalFileSize);
            }
        }

        private UpdateProgressData()
        {
            this.TotalFileSize = 0;
            this.TotalBytesTransferred = 0;
        }

        public UpdateProgressData(long totalFileSize, long totalBytesTransferred)
        {
            this.TotalFileSize = totalFileSize;
            this.TotalBytesTransferred = totalBytesTransferred;
        }

        public override string ToString()
        {
            return string.Format("transf={0}, Total={1}, Done={2}", TotalBytesTransferred, TotalFileSize, IsDone);
        }
    }

    #endregion

    public abstract class BaseFileTask
    {
        public event FileTaskProgressDG FileTaskProgress = null;

        public FileTaskType TaskType { get; private set; }

        public Dictionary<string, string> ErrorMap { get; private set; }

        public bool IsFinished { get; private set; }

        public List<string> SrcFiles { get; private set; }
        public string SrcFolder { get; private set; }
        public string DestFolder { get; private set; }

        public int TotalObjects { get; private set; }
        public int ProcessedObjects
        {
            get
            {
                return _processedFiles.Count;
            }
        }


        ConcurrentDictionary<string, int> _processedFiles = new ConcurrentDictionary<string, int>();

        protected Thread _fileTaskThread;
        protected FileTaskSupport _support = null;

        protected System.Timers.Timer _timer = null;

        private bool _requiresRefresh = false;
        public bool RequiresRefresh
        {
            get
            {
                return (_support == null) ? _requiresRefresh : _support.RequiresRefresh;
            }
        }

        public bool CanContinue
        {
            get
            {
                return (_support == null) ? false : _support.CanContinue;
            }
        }

        public BaseFileTask(FileTaskType type, List<string> srcFiles, string srcFolder)
        {
            this.TaskType = type;
            this.ErrorMap = new Dictionary<string, string>();

            this.SrcFiles = srcFiles;
            this.SrcFolder = srcFolder;

            _timer = new System.Timers.Timer(500);
            _timer.AutoReset = true;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);

            _support = InitSupport();
            this.IsFinished = false;
        }

        protected virtual FileTaskSupport InitSupport()
        {
            return new FileTaskSupport(this);
        }

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentPath))
            {
                FireTaskProgress(ProgressEventType.KeepAlive, _currentPath, UpdateProgressData.Empty);
            }
        }



        public void FireTaskProgress(ProgressEventType eventType, string file, UpdateProgressData data)
        {
            if (eventType == ProgressEventType.Progress &&
                data == UpdateProgressData.FileDone)
            {
                string lowercaseFile = file.ToLowerInvariant();
                if (_processedFiles.ContainsKey(lowercaseFile))
                    return;

                _processedFiles.TryAdd(lowercaseFile, 0);
            }

            if (FileTaskProgress != null)
            {
                FileTaskProgress(eventType, file, data);
            }
        }

        public void AddToErrorMap(string path, string error)
        {
            if (ErrorMap.ContainsKey(path))
            {
                ErrorMap[path] = error;
            }
            else
            {
                ErrorMap.Add(path, error);
            }
        }

        protected string GetDestinationPath(string srcPath)
        {
            if (TaskType == FileTaskType.Copy || TaskType == FileTaskType.Move)
            {
                string diffPath = srcPath.Replace(SrcFolder, string.Empty);
                return Path.Combine(DestFolder, diffPath.TrimStart(PathUtils.DirectorySeparatorChars));
            }

            return string.Empty;
        }


        public void RunTask(string destFolder)
        {
            this.DestFolder = destFolder ?? string.Empty;

            _fileTaskThread = new Thread(new ThreadStart(RunTaskAsync));
            _fileTaskThread.Priority = ThreadPriority.Normal;
            _fileTaskThread.Start();
        }

        public void RequestAbort()
        {
            _support.RequestAbort();
        }

        string _currentPath = string.Empty;
        private void RunTaskAsync()
        {
            try
            {
                _processedFiles.Clear();

                TotalObjects = 0;
                ErrorMap.Clear();

                List<string> allLinkedFiles_Lowercase = new List<string>();
                foreach (string path in SrcFiles)
                {
                    if (File.Exists(path))
                    {
                        List<String> linkedFiles = _support.GetChildFiles(path, TaskType);
                        if (linkedFiles != null)
                        {
                            foreach (string f in linkedFiles)
                            {
                                string fl = f.ToLowerInvariant();

                                if (allLinkedFiles_Lowercase.Contains(fl) == false)
                                    allLinkedFiles_Lowercase.Add(fl);
                            }
                        }

                        string parentFile = _support.GetParentFile(path, TaskType);
                        if (File.Exists(parentFile) && SrcFiles.Contains(parentFile) == false)
                        {
                            string fl = parentFile.ToLowerInvariant();

                            if (allLinkedFiles_Lowercase.Contains(fl) == false)
                                allLinkedFiles_Lowercase.Add(fl);
                        }
                    }
                }

                List<string> srcFilesClone = new List<string>(SrcFiles);

                foreach (string srcFile in srcFilesClone)
                {
                    string fl = srcFile.ToLowerInvariant();

                    if (allLinkedFiles_Lowercase.Contains(fl))
                        SrcFiles.Remove(srcFile);
                }

                foreach (string path in SrcFiles)
                {
                    TotalObjects++;

                    if (Directory.Exists(path))
                    {
                        List<string> entries = PathUtils.EnumFileSystemEntries(path, "*", SearchOption.AllDirectories);
                        if (entries != null && entries.Count > 0)
                        {
                            TotalObjects += entries.Count;
                        }
                    }
                }

                TotalObjects += allLinkedFiles_Lowercase.Count;

                FireTaskProgress(ProgressEventType.Started, string.Empty, UpdateProgressData.Empty);

                _currentPath = string.Empty;
                _timer.Start();

                try
                {
                    foreach (string path in SrcFiles)
                    {
                        _currentPath = path;
                        FireTaskProgress(ProgressEventType.Progress, path, UpdateProgressData.Empty);

                        switch (TaskType)
                        {
                            case FileTaskType.Copy:
                                CopyObject(path);
                                break;

                            case FileTaskType.Move:
                                MoveObject(path);
                                break;

                            case FileTaskType.Delete:
                                DeleteObject(path);
                                break;
                        }
                    }
                }
                catch (TaskInterruptedException)
                {
                    FireTaskProgress(ProgressEventType.Aborted, string.Empty, UpdateProgressData.Empty);
                    return;
                }
                finally
                {
                    _timer.Stop();
                    _currentPath = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            finally
            {
                IsFinished = true;
            }

            FireTaskProgress(ProgressEventType.Finished, string.Empty, UpdateProgressData.Empty);
            _requiresRefresh = _support.RequiresRefresh;
            _support = null;
        }

        #region Overridables

        protected virtual bool CopyObject(string path) { return true; }
        protected virtual bool MoveObject(string path) { return true; }
        protected virtual bool DeleteObject(string path) { return true; }

        #endregion
    }
}
