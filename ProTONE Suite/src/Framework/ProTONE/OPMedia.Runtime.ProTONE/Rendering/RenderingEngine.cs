#region Copyright © 2006 OPMedia Research
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written permission of the copyright owner.

// File: 	MediaRenderer.cs
#endregion

#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Configuration;
using OPMedia.Runtime.ProTONE.Rendering.Base;
using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.Core.Logging;
using OPMedia.Core.TranslationSupport;

#if HAVE_DSHOW
    using OPMedia.Runtime.ProTONE.Rendering.DS;
#else
    using OPMedia.Runtime.ProTONE.Rendering.Mono;
#endif

using OPMedia.Core;
using System.IO;

using OPMedia.Core.Utilities;
using OPMedia.Runtime.ProTONE.SubtitleDownload;
using OPMedia.Runtime.ProTONE.FfdShowApi;
using OPMedia.Core.Configuration;
using OPMedia.UI.Generic;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using OPMedia.Runtime.ProTONE.Utilities;
using OPMedia.Runtime.ProTONE.Configuration;
using System.Runtime.Serialization;
using System.ServiceModel;
using OPMedia.Runtime.ProTONE.WorkerSupport;
using OPMedia.Runtime.ProTONE.Rendering.WorkerSupport;
using NAudio.CoreAudioApi;
using System.Threading.Tasks;
using System.Threading;
using OPMedia.Runtime.ProTONE.OnlineMediaContent;
using OPMedia.ShellSupport;

#endregion

namespace OPMedia.Runtime.ProTONE.Rendering
{
    public delegate void MediaRendererEventHandler();
    public delegate void FilterStateChangedHandler(OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState oldState, string oldMedia, 
        OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState newState, string newMedia);

    public delegate void MediaRenderingExceptionHandler(RenderingExceptionEventArgs args);
    
    public delegate void RenderedStreamPropertyChangedHandler(Dictionary<string, string> newData);

    public sealed class RenderingEngine : IDisposable
    {
        public const int VolumeFull = 0;
        public const int VolumeSilence = -10000;

        private MMDeviceEnumerator _mmEnumerator = null;
        private MMDevice _mmDevice = null;

        private int _hash = DateTime.Now.GetHashCode();
        private double _position = 0;

        public event RenderedStreamPropertyChangedHandler RenderedStreamPropertyChanged = null;

        #region Members
        private static RenderingEngine __defaultInstance = new RenderingEngine(true);

        private WorkerRenderer _renderer = null;
        private System.Windows.Forms.Timer timerCheckState = null;

        private OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState oldState = 
            OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Stopped;

        private string oldMedia = string.Empty;

        Control _renderPanel = null;
        Control _messageDrain = null;

        volatile bool playlistAtEnd = false;
        object _syncPlaylist = new object();

        #endregion

        #region Properties
        
        public static RenderingEngine DefaultInstance
        {
            get
            {
                return __defaultInstance;
            }
        }

        public static RenderingEngine NewInstance()
        {
            return new RenderingEngine(false);
        }

        

        internal object GraphFilter 
        { get { return (_renderer != null) ? _renderer.GraphFilter : null; } }

        public double[] EqFrequencies
        {
            get
            {
                double[] freqs = new double[10];

                using (FfdShowLib ff = FfdShowInstance())
                {
                    for (FFDShowConstants.FFDShowDataId i = FFDShowConstants.FFDShowDataId.IDFF_filterEQ; i < FFDShowConstants.FFDShowDataId.IDFF_filterWinamp2; i++)
                    {
                        int iVal = ff.getIntParam(i);
                        string sVal = ff.getStringParam(i);
                    }

                }

                return freqs;
            }

            set
            {
            }
        }


        public int[] EqLevels
        {
            get
            {
                int[] levels = new int[10];

                return levels;
            }

            set
            {
            }
        }

        public Control RenderPanel
        {
            get { return _renderPanel; }
            set { _renderPanel = value; }
        }

        public Control MessageDrain
        {
            get { return _messageDrain; }
            set { _messageDrain = value; }
        }

        public bool PlaylistAtEnd
        {
            get 
            {
                lock (_syncPlaylist)
                {
                    return playlistAtEnd;
                }
            }
            
            set 
            {
                lock (_syncPlaylist)
                {
                    playlistAtEnd = value;
                }
            }
        }

        protected bool IsEndOfMedia
        {
            get
            {
                if (_renderer == null)
                    return false;

                bool isEnd = _renderer.EndOfMedia;

                if (UseCrossFading)
                {
                    // Check if we are "anticipating" the end of current media so that we can toggle XFade...
                    if (!isEnd && !_renderer.IsStreamedMedia)
                    {
                        double pos = _renderer.MediaPosition;
                        double len = _renderer.MediaLength;

                        if ((len - pos) <= (ProTONEConfig.XFadeAnticipatedEnd))
                        {
                            Logger.LogTrace($"[XFADE] End of media is approaching. Triggering cross fading....");
                            isEnd = true;
                        }
                    }
                }

                return isEnd;
            }
        }

        public double MediaPosition
        {
            get { return (_renderer == null) ? 0 : _renderer.MediaPosition; }
            set { if (_renderer != null) { _renderer.MediaPosition = value; } }
        }

        public int AudioBalance
        {
            get { return (_renderer == null) ? 0 : _renderer.AudioBalance; }
            set { if (_renderer != null) { _renderer.AudioBalance = value; } }
        }

        public int SubtitleStream
        {
            get
            {
                return (_renderer == null &&
                    FilterState != OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Stopped) ?
                    -1 : _renderer.GetSubtitleStream();
            }

            set
            {
                if (_renderer != null &&
                    FilterState != OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Stopped)
                {
                    _renderer.SetSubtitleStream(value);
                }
            }
        }

        public bool CanSeekMedia
        {
            get
            {
                return (_renderer != null && _renderer.MediaSeekable && _renderer.MediaLength > 0 && _xfadeInProgress.WaitOne(0) == false);
            }
        }

        public double DurationScaleFactor
        { get { return (_renderer == null) ? 0 : _renderer.DurationScaleFactor; } }

        public double MediaLength
        { get { return (_renderer == null) ? 0 : _renderer.MediaLength; } }

        public double EffectiveMediaLength
        {
            get
            {
                return Math.Max(MediaLength - ProTONEConfig.XFadeAnticipatedEnd, 1);
            }
        }

        public string RenderMediaName
        {
            get
            {
                if (_renderer == null)
                    return string.Empty;

                return _renderer.RenderMediaName;
            }
        }

        public MediaFileInfo RenderedMediaInfo
        {
            get
            {
                if (_renderer == null)
                    return null;

                return _renderer.RenderMediaInfo;
            }
        }

        public MediaTypes RenderedMediaType
        { 
            get 
            {
                MediaTypes mediaType = MediaTypes.None;

                if (_renderer != null)
                {
                    if (_renderer.AudioMediaAvailable)
                    {
                        mediaType = (_renderer.VideoMediaAvailable) ?
                            MediaTypes.Both : MediaTypes.Audio;
                    }
                    else if (_renderer.VideoMediaAvailable)
                    {
                        mediaType = MediaTypes.Video;
                    }
                }

                return mediaType;
            } 
        }

        public OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState FilterState
         { 
            get 
            {
                if (UseCrossFading && _crossFadePendingStop)
                    return FilterState.Stopped;

                return (_renderer == null) ?
                    OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Stopped : 
                    _renderer.FilterState; 
            } 
        }


        public string TranslatedFilterState
        { get { return Translator.Translate("TXT_" + FilterState.ToString().ToUpperInvariant()); } }

        bool _hasRenderingErrors = false;
        public bool HasRenderingErrors
        {
            get
            {
                return _hasRenderingErrors;
            }
        }

        #endregion

        #region Methods

        public bool IsStreamedMedia
        {
            get
            {
                if (_renderer != null)
                    return _renderer.IsStreamedMedia;

                return false;
            }
        }
        
        public Dictionary<string, string> StreamData { get; private set; }

        internal void FireStreamPropertyChanged(Dictionary<string, string> newData)
        {
            if (IsStreamedMedia && RenderedStreamPropertyChanged != null)
            {
                if (StreamData == null)
                    StreamData = new Dictionary<string, string>();

                foreach (KeyValuePair<string, string> kvp in newData)
                {
                    if (StreamData.ContainsKey(kvp.Key))
                    {
                        if (String.IsNullOrEmpty(kvp.Value))
                            StreamData.Remove(kvp.Key);
                        else
                            StreamData[kvp.Key] = kvp.Value;
                    }
                    else if (String.IsNullOrEmpty(kvp.Value) == false)
                        StreamData.Add(kvp.Key, kvp.Value);
                }

                RenderedStreamPropertyChanged(newData);
            }
        }

        public string GetRenderFile()
        {
            string retVal = string.Empty;
            try
            {
                if (_renderer == null)
                    return string.Empty;

                return _renderer.RenderMediaName;
            }
            catch (Exception ex)
            {
                ReportRenderingException(ex);
            }
            return retVal;
        }

        public void SetRenderFile(string file)
        {
            try
            {
                _hasRenderingErrors = false;


                // Select the proper renderer for the specified media
                Uri uri = null;
                try
                {
                    uri = new Uri(file, UriKind.Absolute);
                }
                catch
                {
                    uri = null;
                }

                WorkerType wt = WorkerType.Unsupported;

                if (uri != null && !uri.IsFile)
                {
                    if (uri.OriginalString.StartsWith(DeezerTrackItem.DeezerTrackUrlBase))
                        wt = WorkerType.Deezer;
                    else
                        wt = WorkerType.Shoutcast;
                }
                else
                {
                    if (DvdMedia.FromPath(file) != null)
                        wt = WorkerType.VideoDvd;
                    else
                    {
                        string streamType = PathUtils.GetExtension(file).ToLowerInvariant();
                        if (streamType == "cda")
                            wt = WorkerType.AudioCd;
                        else
                        {
                            if (SupportedFileProvider.Instance.SupportedVideoTypes.Contains(streamType))
                                wt = WorkerType.Video;
                            else
                                wt = WorkerType.Audio;
                        }
                    }
                }

                if (wt == WorkerType.Unsupported)
                    throw new Exception($"Unsupported media: {file}");

                CreateWorkerRenderer(wt);

                Logger.LogTrace("Now playing media: {0}", file);

                if (_renderer != null)
                {
                    if (this.FilterState == FilterState.Stopped)
                    {
                        _renderer.RenderMediaName = file;
                    }

                    var workerRenderer = _renderer as WorkerSupport.WorkerRenderer;
                    if (workerRenderer != null && workerRenderer.IsVideo)
                    {
                        _renderer.SetRenderRegion(_renderPanel, GraphNotifyWnd.Instance);
                        _renderPanel.Resize -= RenderPanel_Resize;
                        _renderPanel.Resize += RenderPanel_Resize;
                    }
                }
            }
            catch (Exception ex)
            {
                ReportRenderingException(ex);
            }
        }

        private void RenderPanel_Resize(object sender, EventArgs e)
        {
            var workerRenderer = _renderer as WorkerSupport.WorkerRenderer;
            if (workerRenderer != null && workerRenderer.IsVideo)
            {
                _renderPanel.SuspendLayout();
                _renderer.ResizeRenderRegion();
                _renderPanel.ResumeLayout();
            }
        }

        WorkerRenderer _oldRenderer = null;

        internal bool UseCrossFading
        {
            get
            {
                // TODO: Figure out how to prevent CrossFading when a video file is involved.
                return ProTONEConfig.XFade;
            }
        }

        private void CreateWorkerRenderer(WorkerType workerType)
        {
            if (UseCrossFading && _renderer != null)
            {
                Logger.LogTrace($"[XFADE] Creating a new WorkerRenderer of type {workerType}");

                // Cleanup the older renderer
                if (_oldRenderer != null)
                {
                    _oldRenderer.Dispose();
                    _oldRenderer = null;
                }

                _crossFadePendingStop = false;
                _oldRenderer = _renderer;

                // Set up new one
                _renderer = new WorkerSupport.WorkerRenderer(workerType);
            }
            else
            {
                // Cleanup old renderer
                if (_renderer != null)
                {
                    _renderer.Dispose();
                    _renderer = null;
                }

                Logger.LogTrace($"Creating a new WorkerRenderer of type {workerType}");

                _renderer = new WorkerSupport.WorkerRenderer(workerType);
            }
        }

        public void StartRendererWithHint(RenderingStartHint startHint)
        {
            try
            {
                _hasRenderingErrors = false;
                _position = 0;
                StreamData = new Dictionary<string, string>();

                if (this.FilterState == OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Paused)
                {
                    double position = this.MediaPosition;
                    this.ResumeRenderer(position);
                }
                else if (_renderer != null)
                {
                    _renderer.StartRendererWithHint(startHint);
                    HandleCrossFading();
                }
            }
            catch (Exception ex)
            {
                ReportRenderingException(ex);
            }
        }

        private ManualResetEvent _xfadeInProgress = new ManualResetEvent(false);

        public int AudioVolume
        {
            get
            {
                return (_renderer == null) ? (int)VolumeRange.Minimum : _renderer.AudioVolume;
            }

            set
            {
                if (_renderer != null)
                {
                    Task.Factory.StartNew(() =>
                    {
                        while (_xfadeInProgress.WaitOne(0))
                            Thread.Sleep(100);

                        _renderer.AudioVolume = value;
                    });
                }
            }
        }

        private void HandleCrossFading()
        {
            if (UseCrossFading == false)
                return;

            if (_oldRenderer == null || _oldRenderer.Valid == false || _renderer == null)
                return;

            int startVol = _oldRenderer.AudioVolume;
            int quant = startVol / 10;
            _renderer.AudioVolume = 0;

            Task.Factory.StartNew(() =>
            {
                Logger.LogTrace($"[XFADE] Before loop: old=[{_oldRenderer}] new=[{_renderer}]");
                _xfadeInProgress.Set();

                try
                {
                    DateTime dtStart = DateTime.Now;
                    int i = 0;

                    while (true)
                    {
                        i++;
                        int delta = i * quant;
                        _oldRenderer.AudioVolume = startVol - delta;
                        _renderer.AudioVolume = delta;

                        Logger.LogTrace($"[XFADE] Loop: old=[{_oldRenderer}] new=[{_renderer}]");

                        // _crossFadeLength is in sec
                        // To sleep for _crossFadeLength/10 sec, need to multiply with 100
                        Thread.Sleep(100 * ProTONEConfig.XFadeLength);

                        DateTime dtNow = DateTime.Now;
                        if (dtNow.Subtract(dtStart).TotalSeconds >= ProTONEConfig.XFadeLength)
                            break;
                    }

                    Logger.LogTrace($"[XFADE] Exited loop: old=[{_oldRenderer}] new=[{_renderer}]");

                    _oldRenderer.AudioVolume = 0;
                    _renderer.AudioVolume = startVol;

                    Logger.LogTrace($"[XFADE] Final: old=[{_oldRenderer}] new=[{_renderer}]");

                    // Cleanup old renderer
                    if (_oldRenderer != null && _oldRenderer.Valid)
                    {
                        _oldRenderer.StopRenderer();
                        _oldRenderer.Dispose();
                        _oldRenderer = null;
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
                finally
                {
                    _xfadeInProgress.Reset();
                }
            });
        }

        public void StartRenderer()
        {
            try
            {
                _hasRenderingErrors = false;
                _position = 0;
                StreamData = new Dictionary<string, string>();

                if (_renderer != null)
                {
                    Logger.LogTrace("Media will be rendered using {0}", _renderer.GetType().Name);

                    if (this.FilterState == OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Stopped)
                    {
                        _renderer.StartRenderer();
                        HandleCrossFading();
                    }
                    else if (_renderer.FilterState == OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Paused)
                    {
                        double position = _renderer.MediaPosition;
                        _renderer.ResumeRenderer(position);
                    }
                }
            }
            catch (Exception ex)
            {
                ReportRenderingException(ex);
            }
        }

        public void PauseRenderer()
        {
            try
            {
                _hasRenderingErrors = false;

                if (_renderer != null &&
                    _renderer.FilterState == OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Running)
                {
                    _renderer.PauseRenderer();
                }
            }
            catch (Exception ex)
            {
                ReportRenderingException(ex);
            }
        }

        public void ResumeRenderer(double fromPosition)
        {
            try
            {
                _hasRenderingErrors = false;

                if (_renderer != null &&
                    _renderer.FilterState == OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Paused)
                {
                    _position = fromPosition;
                    _renderer.ResumeRenderer(fromPosition);
                }
            }
            catch (Exception ex)
            {
                ReportRenderingException(ex);
            }

        }

        bool _isStopFromGui = false;

        public bool IsStopFromGui
        {
            get
            {
                return _isStopFromGui;
            }
        }

        private bool _crossFadePendingStop = false;

        public void StopRenderer(bool isStopFromGui)
        {
            if (isStopFromGui)
            {
                if (_oldRenderer != null)
                {
                    _oldRenderer.StopRenderer();
                    _oldRenderer.Dispose();
                    _oldRenderer = null;
                }
            }
            else if (UseCrossFading)
            {
                _isStopFromGui = false;
                _crossFadePendingStop = true;
                return;
            }

            try
            {
                _hasRenderingErrors = false;
                _isStopFromGui = isStopFromGui;

                _position = 0;

                if (_renderer != null &&
                    (_renderer.FilterState == OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Running ||
                    _renderer.FilterState == OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Paused))
                {
                    _renderer.StopRenderer();
                }
            }
            catch (Exception ex)
            {
                ReportRenderingException(ex);
            }
        }

        public string AvailableFileTypesFilter
        {
            get
            {
                return string.Format("{0}{1}{2}{3}",
                    AudioFilesFilter, VideoFilesFilter, VideoHDFilesFilter, PlaylistsFilter);
            }
        }

        public string AudioFilesFilter
        { get { return ConstructFilter("TXT_AUDIO_FILES", SupportedFileProvider.Instance.SupportedAudioTypes); } }

        public string VideoHDFilesFilter
        { get { return ConstructFilter("TXT_VIDEO_HD_FILES", SupportedFileProvider.Instance.SupportedHDVideoTypes); } }

        public string VideoFilesFilter
        { get { return ConstructFilter("TXT_VIDEO_FILES", SupportedFileProvider.Instance.SupportedVideoTypes); } }

        public string PlaylistsFilter
        { get { return ConstructFilter("TXT_PLAYLISTS", SupportedFileProvider.Instance.SupportedPlaylists); } }

        private string ConstructFilter(string tag, List<string> fileTypes)
        {
            string filterFormat = tag + " ({0})|{1}|";
            string filterPart1 = "";
            string filterPart2 = "";

            foreach (string fileType in fileTypes)
            {
                filterPart1 += "*." + fileType;
                filterPart1 += ",";

                filterPart2 += "*." + fileType;
                filterPart2 += ";";
            }

            filterPart1 = filterPart1.TrimEnd(new char[] { ',' });
            filterPart2 = filterPart2.TrimEnd(new char[] { ';' });
            
            return string.Format(filterFormat, filterPart1, filterPart2);
        }

        public VideoFileInfo QueryVideoMediaInfo(string path)
        {
            VideoFileInfo vfi = null;

            DvdMedia dvdDrive = DvdMedia.FromPath(path);
            if (dvdDrive != null)
            {
                vfi = dvdDrive.VideoDvdInformation;
            }
            else
            {
                vfi = new VideoFileInfo(path, false);

                try
                {
                    if (vfi != null && vfi.IsValid)
                    {
                        Guid filterGraphGuid = ProTONEConfig.FilterGraphGuid;
                        Type mediaControlType = Type.GetTypeFromCLSID(filterGraphGuid, true);

                        IMediaControl mediaControl = Activator.CreateInstance(mediaControlType) as IMediaControl;
                        IBasicAudio basicAudio = mediaControl as IBasicAudio;
                        IBasicVideo basicVideo = mediaControl as IBasicVideo;
                        IMediaPosition mediaPosition = mediaControl as IMediaPosition;

                        mediaControl.RenderFile(path);

                        double val = 0;
                        DsError.ThrowExceptionForHR(mediaPosition.get_Duration(out val));
                        vfi.Duration = TimeSpan.FromSeconds(val);

                        DsError.ThrowExceptionForHR(basicVideo.get_AvgTimePerFrame(out val));
                        vfi.FrameRate = new FrameRate(1f / val);

                        int h = 0, w = 0;
                        DsError.ThrowExceptionForHR(basicVideo.get_VideoHeight(out h));
                        DsError.ThrowExceptionForHR(basicVideo.get_VideoWidth(out w));
                        vfi.VideoSize = new VSize(w, h);

                        mediaControl.Stop();
                        mediaControl = null;
                        mediaPosition = null;
                        basicVideo = null;
                        basicAudio = null;

                        GC.Collect();
                    }
                }
                catch
                {
                }
            }

            return vfi;
        }

        public string GetStateDescription()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("[DUR] : {0}\r\n", this.MediaLength);
            sb.AppendFormat("[POS] : {0}\r\n", this.MediaPosition);
            sb.AppendFormat("[MST] : {0}\r\n", this.FilterState);
            sb.AppendFormat("[FIL] : {0}\r\n", this.GetRenderFile());

            return sb.ToString();
        }
        #endregion

        #region Construction
        private RenderingEngine(bool isDefaultInstance)
        {
            _renderer = null;

            RegistrationSupport.Init();

            _mmEnumerator = new MMDeviceEnumerator();
            _mmDevice = _mmEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            timerCheckState = new System.Windows.Forms.Timer();
            timerCheckState.Enabled = true;
            timerCheckState.Interval = 500;
            timerCheckState.Start();
            timerCheckState.Tick += new EventHandler(timerCheckState_Tick);

            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
        }
        void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (ProTONEConfig.IsPlayer && ProTONEConfig.DeezerHasValidConfig)
            {
                string deezerCachePath = PathUtils.GetCacheFolderPath("dzrcache");
                // Cleanup cache
                if (Directory.Exists(deezerCachePath))
                {
                    Logger.LogInfo($"Purging Deezer cache folder at {deezerCachePath}");
                    Directory.Delete(deezerCachePath, true);
                }
            }
        }


        ~RenderingEngine()
        {
            if (timerCheckState != null)
            {
                timerCheckState.Dispose();
            }

            if (this == __defaultInstance)
            {
                if (ProTONEConfig.IsPlayer)
                {
                    SystemScheduler.Stop();
                }

                __defaultInstance = null;
            }

            _renderer = null;
        }

        double oldMediaPosition = 0;
        int nofPasses = 0;

        double _prevTime = 0;
        void timerCheckState_Tick(object sender, EventArgs e)
        {
            double newMediaPosition = this.MediaPosition;

            double nowTime = TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds;
            double diff = (nowTime - _prevTime);
            _prevTime = nowTime;

            FireMediaRendererClock();

            OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState newState = OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.NotOpened;

            try
            {
                newState = this.FilterState;
                string newMedia = this.RenderMediaName;

                if (newState == OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Running && oldMediaPosition == newMediaPosition)
                {
                    nofPasses++;
                    Logger.LogToConsole("Media position did not change in the last {0} iterations ... old={1}, new={2}", 
                        nofPasses, oldMediaPosition, newMediaPosition);
                }
                else
                {
                    nofPasses = 0;
                    Logger.LogToConsole("Media position changed ... old={0}, new={1}",
                        oldMediaPosition, newMediaPosition);

                    oldMediaPosition = newMediaPosition;
                }

                if (this.IsEndOfMedia || (!IsStreamedMedia && nofPasses > 10))
                {
                    if (newState != OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Stopped)
                    {
                        Logger.LogTrace("timerCheckState_Tick ... IsEndOfMedia={0}, IsStreamedMedia={1}, nofPasses={2}, newState={3}", 
                            IsEndOfMedia, IsStreamedMedia, nofPasses, newState);

                        this.StopRenderer(false);
                        newState = OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.NotOpened;
                    }
                    else
                    {
                        newState = OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Stopped;
                    }
                    
                }
                else if (oldState == OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.NotOpened && newState == OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Stopped)
                {
                    newState = OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.NotOpened;
                }

                if (newState != oldState || newMedia != oldMedia)
                {
                    FireFilterStateChanged(oldState, oldMedia, newState, newMedia);
                }

                switch (newState)
                {
                    case OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Running:
                        switch (oldState)
                        {
                            case OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Running:
                                _position += diff;
                                break;

                            case OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.NotOpened:
                            case OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Stopped:
                                //break;

                            case OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Paused:
                                _position = _renderer.MediaPosition;
                                break;
                        }
                        break;

                    case OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Stopped:
                    case OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.NotOpened:
                        _position = 0;
                        break;

                    case OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Paused:
                        break;
                }

                if (newState != OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.Stopped &&
                    newState != OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState.NotOpened)
                {
                    FireMediaRendererHeartbeat();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                oldState = newState;
                oldMedia = this.RenderMediaName;
            }
        }

        public void ReportRenderingException(Exception ex)
        {
             _hasRenderingErrors = true;

            try
            {
                RenderingException rex = RenderingException.FromException(ex);
                //rex.RenderedFile = this.GetRenderFile();
                rex.RenderedFile = "__DESC__";

                RenderingExceptionEventArgs args = new RenderingExceptionEventArgs(rex);
                FireMediaRenderingException(args);

                if (args.Handled)
                    return;
            }
            catch
            {
            }

            throw ex;
        }

        #endregion

        public override bool Equals(object obj)
        {
            RenderingEngine mr = (obj as RenderingEngine);
            if (mr != null)
            {
                return (this._hash == mr._hash);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return _hash;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_renderer != null)
            {
                _renderer.Dispose();
                _renderer = null;
            }
        }

        #endregion

        #region Published events
        public event MediaRendererEventHandler MediaRendererClock = null;
        private void FireMediaRendererClock()
        {
            if (MediaRendererClock != null)
            {
                MediaRendererClock();
            }
        }

        public event MediaRendererEventHandler MediaRendererHeartbeat = null;
        private void FireMediaRendererHeartbeat()
        {
            if (MediaRendererHeartbeat != null)
            {
                MediaRendererHeartbeat();
            }
        }

        public event FilterStateChangedHandler FilterStateChanged = null;
        private void FireFilterStateChanged(OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState oldState, string oldMedia,
            OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses.FilterState newState, string newMedia)
        {
            if (FilterStateChanged != null)
            {
                FilterStateChanged(oldState, oldMedia, newState, newMedia);
            }
        }
        
        public event MediaRenderingExceptionHandler MediaRenderingException = null;
        private void FireMediaRenderingException(RenderingExceptionEventArgs args)
        {
            if (MediaRenderingException != null)
            {
                MediaRenderingException(args);
            }
        }
        
        #endregion

        #region FFdShow subtitle and OSD

        IntPtr _oldFfdShowHandle = IntPtr.Zero;

        public string CurrentSubtitleFile
        {
            get
            {
                using (FfdShowLib i = FfdShowInstance())
                {
                    return i.CurrentSubtitleFile;
                }
            }

            set
            {
                using (FfdShowLib i = FfdShowInstance())
                {
                    i.CurrentSubtitleFile = value;
                }
            }
        }

        public void DisplayOsdMessage(string msg)
        {
            if (ProTONEConfig.OsdEnabled)
            {
                using (FfdShowLib i = FfdShowInstance())
                {
                    int osdPersistTimer = ProTONEConfig.OsdPersistTimer;
                    float frameRate = i.getFrameRate();
                    int persistFrames = (int)(osdPersistTimer * frameRate / 1000);

                    // OsdPersistTimer is given in msec
                    i.clearOsd();
                    i.setOsdDuration(persistFrames);
                    i.displayOSDMessage(msg, true);
                }
            }
        }

        public void ReloadFfdShowSettings()
        {
            using (FfdShowLib i = FfdShowInstance())
            {
                EnforceSettings(i);
            }
        }

        private void EnforceSettings(FfdShowLib ffdShowLib)
        {
            // Subtitles
            ffdShowLib.DoShowSubtitles = ProTONEConfig.SubEnabled;

            if (ProTONEConfig.SubEnabled)
            {
                ffdShowLib.setIntParam(FFDShowConstants.FFDShowDataId.IDFF_fontColor,
                    ColorHelper.BGR(ProTONEConfig.SubColor));
                ffdShowLib.setIntParam(FFDShowConstants.FFDShowDataId.IDFF_fontSizeA,
                    ProTONEConfig.SubFont.Height);
                ffdShowLib.setStringParam(FFDShowConstants.FFDShowDataId.IDFF_fontName,
                    ProTONEConfig.SubFont.OriginalFontName);
                ffdShowLib.setIntParam(FFDShowConstants.FFDShowDataId.IDFF_fontCharset,
                  ProTONEConfig.SubFont.GdiCharSet);

                LOGFONT lf = new LOGFONT();
                ProTONEConfig.SubFont.ToLogFont(lf);

                ffdShowLib.setIntParam(FFDShowConstants.FFDShowDataId.IDFF_fontWeight,
                    lf.lfWeight);
                ffdShowLib.setIntParam(FFDShowConstants.FFDShowDataId.IDFF_fontItalic,
                   lf.lfItalic);
                ffdShowLib.setIntParam(FFDShowConstants.FFDShowDataId.IDFF_fontUnderline,
                   lf.lfUnderline);
            }

            if (ProTONEConfig.OsdEnabled)
            {
                ffdShowLib.setIntParam(FFDShowConstants.FFDShowDataId.IDFF_OSDfontColor,
                    ColorHelper.BGR(ProTONEConfig.OsdColor));
                ffdShowLib.setIntParam(FFDShowConstants.FFDShowDataId.IDFF_OSDfontSize,
                    ProTONEConfig.OsdFont.Height);
                ffdShowLib.setStringParam(FFDShowConstants.FFDShowDataId.IDFF_OSDfontName,
                    ProTONEConfig.OsdFont.OriginalFontName);
                ffdShowLib.setIntParam(FFDShowConstants.FFDShowDataId.IDFF_OSDfontCharset,
                    ProTONEConfig.OsdFont.GdiCharSet);

                LOGFONT lf = new LOGFONT();
                ProTONEConfig.OsdFont.ToLogFont(lf);

                ffdShowLib.setIntParam(FFDShowConstants.FFDShowDataId.IDFF_OSDfontWeight,
                    lf.lfWeight);
                ffdShowLib.setIntParam(FFDShowConstants.FFDShowDataId.IDFF_OSDfontItalic,
                   lf.lfItalic);
                ffdShowLib.setIntParam(FFDShowConstants.FFDShowDataId.IDFF_OSDfontUnderline,
                   lf.lfUnderline);
            }
        }

        private FfdShowLib FfdShowInstance()
        {
            string renderFile = this.GetRenderFile();
            FfdShowLib ffdShowLib = new FfdShowLib(renderFile);

            if (ffdShowLib.checkFFDShowActive())
            {
                if (ffdShowLib.FFDShowInstanceHandle != _oldFfdShowHandle)
                {
                    // API re-created so enforce parameters
                    EnforceSettings(ffdShowLib);

                    _oldFfdShowHandle = ffdShowLib.FFDShowInstanceHandle;
                }
            }

            return ffdShowLib;
        }

        #endregion
    }

   

    public abstract class RenderingStartHint
    {
        public abstract bool IsSubtitleHint { get; }
    }
}

#region ChangeLog
#region Date: 01.08.2006			Author: octavian
// File created.
#endregion
#endregion