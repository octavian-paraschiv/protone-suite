using OPMedia.Core;
using OPMedia.Core.Logging;
using OPMedia.Core.TranslationSupport;
using OPMedia.Core.Utilities;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.Rendering;
using OPMedia.Runtime.ProTONE.SubtitleDownload;
using OPMedia.Runtime.ProTONE.SubtitleDownload.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;
using LocalEventNames = OPMedia.UI.ProTONE.GlobalEvents.EventNames;

namespace OPMedia.UI.ProTONE.SubtitleDownload
{
    public static class SubtitleDownloadProcessor
    {
        static List<String> __filesInProgress = new List<string>();

        public static bool IsFileOnDownloadList(string file)
        {
            lock (__filesInProgress)
            {
                return __filesInProgress.Contains(file.ToLowerInvariant());
            }
        }

        private static void FireNotify(string message, string title, MessageBoxIcon icon)
        {
            EventDispatch.DispatchEvent(LocalEventNames.ShowTrayMessage, message, title, icon);
        }

        public static void TryFindSubtitle(string strFile, int duration, bool askToOverwrite)
        {
            try
            {
                if (TestForExistingSubtitle(strFile))
                {
                    if (!askToOverwrite || MessageDisplay.Query(Translator.Translate("TXT_OVERWRITE_SUBTITLE"),
                        Translator.Translate("TXT_CONFIRM_OVERWRITE_SUBTITLE"), MessageBoxIcon.Information) != DialogResult.Yes)
                    {
                        return;
                    }
                }

                if (CanPerformSubtitleDownload(strFile, duration))
                {
                    Dictionary<SubtitleDownloader, List<SubtitleInfo>> subs = null;

                    Task.Factory.StartNew(() =>
                    {
                        subs = GetSubtitles(strFile);

                    }).ContinueWith(_ =>
                    {
                        try
                        {
                            var totalFound = (from subInfos in subs.Values.ToList()
                                              from si in subInfos
                                              where si.SubFileName?.Length > 0
                                              select 1).Count();

                            if (totalFound > 0)
                            {
                                SubtitleDownloadNotifyForm dlg = new SubtitleDownloadNotifyForm(strFile, subs);
                                dlg.SubtitleDownloadNotify += new SubtitleDownloadNotifyHandler(dlg_SubtitleDownloadNotify);
                                dlg.FormClosed += (ss, ee) => SubDownloaderComplete(strFile, subs);
                                dlg.Show(MainThread.MainWindow);
                                return;
                            }

                            SubDownloaderComplete(strFile, subs);

                            // No subtitles found, give message and exit
                            FireNotify(Translator.Translate("TXT_NO_SUBS_FOUND"),
                                Translator.Translate("TXT_CAUTION"), MessageBoxIcon.Warning);
                        }
                        catch (Exception ex)
                        {
                            Logger.LogException(ex);
                            SubDownloaderComplete(strFile, subs);
                        }

                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
            finally
            {
                // This is for enforcing playlist refresh
                EventDispatch.DispatchEvent(LocalEventNames.UpdatePlaylistNames, false);
            }
        }

        private static void SubDownloaderComplete(string strFile, Dictionary<SubtitleDownloader, List<SubtitleInfo>> subs)
        {
            try
            {
                subs.Keys.ToList().ForEach(sd => sd?.Dispose());

                // Perform cleanup
                lock (__filesInProgress)
                {
                    __filesInProgress.Remove(strFile.ToLowerInvariant());

                    // This is for enforcing playlist refresh
                    EventDispatch.DispatchEvent(LocalEventNames.UpdatePlaylistNames, false);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static bool CanPerformSubtitleDownload(string strFile, int duration)
        {
            if (duration < ProTONEConfig.SubtitleMinimumMovieDuration * 60)
            {
                // Movie is too short
                Logger.LogTrace("This movie does not have a subtitle but is shorter than {0} minutes. Not starting download.", 20);
                return false;
            }
            if (!ProTONEConfig.SubtitleDownloadEnabled)
            {
                // Subtitle download not enabled
                Logger.LogTrace("This movie does not have a subtitle but online subtitle download is disabled.");
                return false;
            }
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                // Subtitle download not enabled
                Logger.LogTrace("This movie does not have a subtitle but there is no available network connection to use for download.");
                return false;
            }
            if (SubtitleDownloadProcessor.IsFileOnDownloadList(strFile))
            {
                // Already on download list
                Logger.LogTrace("This movie has the subtitle already on download list.");
                return false;
            }

            return true;
        }

        static string _prevSubtitleFile = null;
        public static bool TestForExistingSubtitle(string movieFileName)
        {
            // If strFile indicates only a disk root, the movie is actually a DVD
            // We don't want to look up on Internet for DVD subtitles. Usually DVD's 
            // come with their builtin subtitles.
            if (PathUtils.IsRootPath(movieFileName))
                return false;

            var moviePath = Path.GetFullPath(movieFileName);
            var renderedPath = RenderingEngine.DefaultInstance.GetRenderFile();

            if (string.Compare(moviePath, renderedPath, true) == 0 && RenderingEngine.DefaultInstance.IsRunningOrPaused)
            {
                string subtitleFile = RenderingEngine.DefaultInstance.CurrentSubtitleFile;

                bool usingSubtitle = subtitleFile?.Length > 0;

                if (string.Compare(_prevSubtitleFile, subtitleFile, true) != 0)
                {
                    _prevSubtitleFile = subtitleFile;

                    // using another subtitle file that last time
                    if (usingSubtitle)
                        Logger.LogTrace("Using subtitle file: {0}", subtitleFile);
                    else
                        Logger.LogTrace("Not using a subtitle file");
                }

                return usingSubtitle;
            }
            else
            {
                // The file is not being played now.
                string fileName = Path.GetFileNameWithoutExtension(moviePath);
                string directory = Path.GetDirectoryName(moviePath);

                if (Directory.Exists(directory))
                {
                    List<string> files = PathUtils.EnumFiles(directory, string.Format("{0}.*", fileName));
                    if (files.Count > 1)
                    {
                        // There is at least another file having a similar name in the folder.
                        // Check if its extension is a valid subtitle extension.
                        foreach (string file in files)
                        {
                            if (file.ToLowerInvariant() == movieFileName.ToLowerInvariant())
                                continue; // This is our video file ... not interesting

                            string ext = PathUtils.GetExtension(file);
                            if (SubtitleDownloader.AllowedSubtitleExtensions.Contains(ext))
                                return true;
                        }
                    }

                }

                return false;
            }
        }

        private static Dictionary<SubtitleDownloader, List<SubtitleInfo>> GetSubtitles(string movieFilePath)
        {
            Dictionary<SubtitleDownloader, List<SubtitleInfo>> subs =
                new Dictionary<SubtitleDownloader, List<SubtitleInfo>>();

            try
            {
                if (!string.IsNullOrEmpty(movieFilePath))
                {
                    RenderingEngine.DefaultInstance.CurrentSubtitleFile = string.Empty;

                    lock (__filesInProgress)
                    {
                        __filesInProgress.Add(movieFilePath.ToLowerInvariant());

                        // This is for enforcing playlist refresh
                        EventDispatch.DispatchEvent(LocalEventNames.UpdatePlaylistNames, false);
                    }

                    Logger.LogTrace("A subtitle was not found for this movie. Attempting to find one ...");

                    string[] subtitleDownloadURIs = StringUtils.ToStringArray(ProTONEConfig.SubtitleDownloadURIs, '\\');
                    if (subtitleDownloadURIs != null)
                    {
                        int prio = 1;

                        foreach (string subtitleDownloadURI in subtitleDownloadURIs)
                        {
                            SubtitleDownloader sd = null;
                            List<SubtitleInfo> foundSubtitles = null;

                            try
                            {
                                sd = SubtitleDownloader.FromDownloadURI(subtitleDownloadURI);
                                if (sd.IsActive)
                                {
                                    sd.Priority = prio++;
                                    foundSubtitles = sd.GetSubtitles(movieFilePath);
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.LogException(ex);
                            }
                            finally
                            {
                                if (sd != null && foundSubtitles?.Count > 0)
                                    subs.Add(sd, foundSubtitles);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return subs;
        }

        static void dlg_SubtitleDownloadNotify(string movieFile, string subtitleFile)
        {
            // This is for enforcing playlist refresh
            EventDispatch.DispatchEvent(LocalEventNames.UpdatePlaylistNames, false);

            RenderingEngine.DefaultInstance.CurrentSubtitleFile = string.Empty;
            RenderingEngine.DefaultInstance.CurrentSubtitleFile = subtitleFile;

            Logger.LogTrace("A subtitle was found: {0}, for movie: {1}", subtitleFile, movieFile);

            if (ProTONEConfig.SubDownloadedNotificationsEnabled)
            {
                string msg = Translator.Translate("TXT_SUB_LOADED");
                FireNotify(msg, ApplicationInfo.ApplicationName, MessageBoxIcon.Information);
            }
        }
    }
}
