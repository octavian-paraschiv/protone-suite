using OPMedia.Core.Configuration;
using OPMedia.Core.Logging;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;

namespace OPMedia.Core.NetworkAccess
{
    public delegate void FileRetrieveCompleteEventHandler(string path, bool success, bool cancelled, string errorDetails);
    public delegate void ProgressEventHandler(int percentage, long bytesReceived, long totalBytes);

    public class WebFileRetriever : IDisposable
    {
        ProxySettings _ns = null;
        string _downloadUrl = string.Empty;
        string _destinationPath = string.Empty;
        WebClient _wc = null;

        public event FileRetrieveCompleteEventHandler FileRetrieveComplete = null;
        public event ProgressEventHandler DownloadProgress = null;

        public WebFileRetriever(ProxySettings ns, string downloadUrl, string destinationPath)
        {
            _ns = ns;
            _downloadUrl = downloadUrl;
            _destinationPath = destinationPath;
        }

        public void PerformDownload(bool isAsync)
        {
            string destFolder = Path.GetDirectoryName(_destinationPath);

            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }

            _wc = new WebClient();
            _wc.Proxy = AppConfig.GetWebProxy();

            if (isAsync)
            {
                _wc.DownloadProgressChanged += OnDownloadProgressChanged;
                _wc.DownloadFileCompleted += OnDownloadFileCompleted;
                _wc.DownloadFileAsync(new Uri(_downloadUrl), _destinationPath);
            }
            else
            {
                try
                {
                    _wc.DownloadFile(new Uri(_downloadUrl), _destinationPath);
                    FileRetrieveComplete?.Invoke(_destinationPath, true, false, string.Empty);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                    FileRetrieveComplete?.Invoke(_destinationPath, false, false, ex.Message);
                }
            }
        }

        private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            bool isSuccess = true;
            string message = string.Empty;

            if (e.Error != null)
            {
                isSuccess = false;
                message = e.Error.Message;
                Logger.LogWarning(message);
            }

            FileRetrieveComplete?.Invoke(_destinationPath, isSuccess, e.Cancelled, message);
        }

        private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadProgress?.Invoke(e.ProgressPercentage, e.BytesReceived, e.TotalBytesToReceive);
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_wc != null)
            {
                _wc.CancelAsync();

                _wc.Dispose();
                _wc = null;
            }
        }

        #endregion
    }
}
