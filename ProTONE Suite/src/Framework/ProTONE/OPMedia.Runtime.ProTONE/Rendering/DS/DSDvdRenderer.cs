using OPMedia.Core.Logging;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.Runtime.ProTONE.Rendering.Base;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace OPMedia.Runtime.ProTONE.Rendering.DS
{
    public class DSDvdRenderer : DsCustomRenderer
    {
        IDvdGraphBuilder dvdGraphBuilder = null;
        IDvdInfo2 dvdInfo = null;
        IDvdControl2 dvdControl2 = null;
        OptIDvdCmd _lastCmd = null;

        private MenuMode menuMode;

        TimeSpan _currentPosition = new TimeSpan();
        TimeSpan _totalTime = new TimeSpan();

        VideoDvdInformation _vdi = null;

        protected override void DoStartRenderer()
        {
            // Start rendering from the beginning of DVD media
            DoStartRendererWithHint(DvdRenderingStartHint.Beginning);
        }

        protected override void DoStartRendererWithHint(RenderingStartHint startHint)
        {
            DvdRenderingStartHint hint = startHint as DvdRenderingStartHint;

            _vdi = new VideoDvdInformation(renderMediaName);

            InitMedia();
            InitAudioAndVideo();

            // Run the graph to play the media file
            int hr = mediaControl.Run();
            WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);

            if (hint == DvdRenderingStartHint.MainMenu)
            {
                hr = dvdControl2.ShowMenu(DvdMenuId.Title, DvdCmdFlags.Flush | DvdCmdFlags.Block, _lastCmd);
                WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);
            }
            else if (hint == DvdRenderingStartHint.Beginning)
            {
                if (ProTONEConfig.DisableDVDMenu)
                    hr = dvdControl2.PlayTitle(1, DvdCmdFlags.Flush | DvdCmdFlags.Block, _lastCmd);
                else
                    //dvdControl.PlayForwards(1f, DvdCmdFlags.Flush | DvdCmdFlags.Block, _lastCmd);
                    hr = dvdControl2.ShowMenu(DvdMenuId.Title, DvdCmdFlags.Flush | DvdCmdFlags.Block, _lastCmd);

                WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);
            }
            else if (hint.Location.ChapterNum == 0)
            {
                hr = dvdControl2.PlayTitle(hint.Location.TitleNum, DvdCmdFlags.Flush | DvdCmdFlags.Block, _lastCmd);
                WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);
            }
            else
            {
                hr = dvdControl2.PlayChapterInTitle(hint.Location.TitleNum, hint.Location.ChapterNum,
                    DvdCmdFlags.Flush | DvdCmdFlags.Block, _lastCmd);
                WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);
            }

            if (ProTONEConfig.PrefferedSubtitleLang > 0)
            {
                int sid = _vdi.GetSubtitle(ProTONEConfig.PrefferedSubtitleLang);
                if (sid > 0)
                {
                    SetSubtitleStream(sid);
                }
            }
        }

        private void InitMedia()
        {
            GC.Collect();

            string volumePath = string.Empty;
            if (renderMediaName.ToUpperInvariant().EndsWith("VIDEO_TS"))
            {
                volumePath = renderMediaName;
            }
            else
            {
                volumePath = System.IO.Path.Combine(renderMediaName, "VIDEO_TS");
            }

            dvdGraphBuilder =
                Activator.CreateInstance(Type.GetTypeFromCLSID(Filters.DvdGraphBuilder, true))
                as IDvdGraphBuilder;

            AMDvdRenderStatus status;

            dvdGraphBuilder.TryRenderDVD(volumePath, out status);

            Logger.LogTrace("Failed to open DVD streams: {0}", status.dwFailedStreamsFlag);

            if (status.bDvdVolInvalid)
                throw new COMException(VideoDvdInformation.ErrDvdVolume, -1);

            dvdInfo = GetInterface(typeof(IDvdInfo2)) as IDvdInfo2;

            dvdControl2 = GetInterface(typeof(IDvdControl2)) as IDvdControl2;

            int hr = dvdControl2.SetOption(DvdOptionFlag.HMSFTimeCodeEvents, true);	// use new HMSF timecode format
            WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);

            hr = dvdControl2.SetOption(DvdOptionFlag.ResetOnStop, false);
            WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);

            hr = dvdControl2.SetOption(DvdOptionFlag.AudioDuringFFwdRew, false);
            WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);

            dvdGraphBuilder.GetFiltergraph(out mediaControl);

            if (mediaControl == null)
                throw new RenderingException("Unable to render the file: " + renderMediaName);

            RebuildFilterGraph();

            mediaEvent = mediaControl as IMediaEventEx;
            mediaPosition = mediaControl as IMediaPosition;
            videoWindow = mediaControl as IVideoWindow;
            basicVideo = mediaControl as IBasicVideo;
            basicAudio = mediaControl as IBasicAudio;

            renderRegion.MouseMove -= new MouseEventHandler(renderRegion_MouseMove);
            renderRegion.MouseMove += new MouseEventHandler(renderRegion_MouseMove);
            renderRegion.MouseDown -= new MouseEventHandler(renderRegion_MouseDown);
            renderRegion.MouseDown += new MouseEventHandler(renderRegion_MouseDown);

        }

        private object GetInterface(Type interfaceType)
        {
            object comobj = null;
            dvdGraphBuilder.GetDvdInterface(interfaceType.GUID, out comobj);
            return comobj;
        }

        private void RebuildFilterGraph()
        {
            // Get the graph builder
            IGraphBuilder graphBuilder = (mediaControl as IGraphBuilder);
            if (graphBuilder == null)
                return;

            try
            {
                IEnumFilters enumFilters = null;
                int hr = graphBuilder.EnumFilters(out enumFilters);
                WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);

                IBaseFilter[] filters = new IBaseFilter[1];

                List<IBaseFilter> filtersToRemove = new List<IBaseFilter>();

                IBaseFilter dvdNavigator = null;

                hr = enumFilters.Next(1, filters, IntPtr.Zero);
                while (hr == HRESULT.S_OK)
                {
                    FilterInfo fi;
                    int hr2 = filters[0].QueryFilterInfo(out fi);
                    if (hr2 == HRESULT.S_OK)
                    {
                        if (fi.achName != "DVD Navigator")
                            filtersToRemove.Add(filters[0]);
                        else
                            dvdNavigator = filters[0];
                    }

                    hr = enumFilters.Next(1, filters, IntPtr.Zero);
                }

                filtersToRemove.ForEach((f) => graphBuilder.RemoveFilter(f));

                IEnumPins enumPins;
                hr = dvdNavigator.EnumPins(out enumPins);
                WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);

                IPin[] pins = new IPin[1];
                hr = enumPins.Next(1, pins, IntPtr.Zero);
                while (hr == HRESULT.S_OK)
                {
                    graphBuilder.Render(pins[0]);

                    hr = enumPins.Next(1, pins, IntPtr.Zero);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }


        void renderRegion_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ((dvdControl2 == null) || (menuMode != MenuMode.Buttons))
                return;

            int hr = dvdControl2.ActivateAtPosition(DsPOINT.FromPoint(e.Location));
            WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);
        }

        void renderRegion_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ((dvdControl2 == null) || (menuMode != MenuMode.Buttons))
                return;

            int hr = dvdControl2.SelectAtPosition(DsPOINT.FromPoint(e.Location));
            WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);
        }

        //protected override void  DoStopInternal(object state)
        //{
        //    try
        //    {
        //        if (mediaControl != null)
        //        {
        //            int hr = mediaControl.Stop();
        //            WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);

        //            if (dvdControl2 != null)
        //            {
        //                Marshal.ReleaseComObject(dvdControl2);
        //                dvdControl2 = null;
        //            }

        //            if (dvdInfo != null)
        //            {
        //                Marshal.ReleaseComObject(dvdInfo);
        //                dvdInfo = null;
        //            }

        //            if (dvdGraphBuilder != null)
        //            {
        //                Marshal.ReleaseComObject(dvdGraphBuilder);
        //                dvdGraphBuilder = null;
        //            }

        //            mediaControl = null;
        //            mediaPosition = null;
        //            videoWindow = null;
        //            basicVideo = null;
        //            basicAudio = null;
        //            mediaEvent = null;
        //        }

        //        GC.Collect();
        //    }
        //    catch (Exception ex)
        //    {
        //        // This is running on other thread than the DSRenderer,
        //        // so its exceptions are not caught in MediaRenderer
        //        ErrorDispatcher.DispatchError(ex);
        //    }
        //}

        protected override void DoResumeRenderer(double fromPosition)
        {
            if (mediaControl != null)
            {
                int hr = mediaControl.Run();
                WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);
            }

            if (fromPosition > 0)
                SetMediaPosition(fromPosition);

        }

        protected override double GetMediaLength()
        {
            return _totalTime.TotalSeconds;
        }

        protected override double GetMediaPosition()
        {
            return _currentPosition.TotalSeconds;
        }

        protected override void SetMediaPosition(double pos)
        {
            TimeSpan tsNewPos = TimeSpan.FromSeconds(pos);

            DvdHMSFTimeCode timeCode = new DvdHMSFTimeCode();
            timeCode.bHours = (byte)tsNewPos.TotalHours;
            timeCode.bMinutes = (byte)tsNewPos.Minutes;
            timeCode.bSeconds = (byte)tsNewPos.Seconds;
            timeCode.bFrames = 1;

            int hr = dvdControl2.PlayAtTime(ref timeCode, DvdCmdFlags.None, _lastCmd);
            WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);
        }

        protected override bool IsMediaSeekable()
        {
            return true;
        }

        protected override bool IsEndOfMedia()
        {
            return false;
        }

        protected override void HandleGraphEvent(EventCode code, int p1, int p2)
        {


            switch (code)
            {
                case EventCode.DvdCurrentHmsfTime:
                    byte[] ati = BitConverter.GetBytes(p1);
                    _currentPosition = new TimeSpan(ati[0], ati[1], ati[2]);
                    break;

                case EventCode.DvdDomChange:
                    DvdDomain dom = (DvdDomain)p1;
                    Logger.LogTrace("Currently in domain: {0}", dom);

                    if (dom == DvdDomain.Title)
                    {
                        object comobj = null;
                        dvdGraphBuilder.GetDvdInterface(typeof(IDvdInfo2).GUID, out comobj);

                        dvdInfo = comobj as IDvdInfo2;

                        DvdHMSFTimeCode timeCode;
                        DvdTimeCodeFlags flags;
                        dvdInfo.GetTotalTitleTime(out timeCode, out flags);
                        _totalTime = new TimeSpan(timeCode.bHours, timeCode.bMinutes, timeCode.bSeconds);
                    }
                    break;

                case EventCode.DvdChaptStart:
                case EventCode.DvdTitleChange:
                case EventCode.DvdCmdStart:
                case EventCode.DvdCmdEnd:
                    break;

                case EventCode.DvdStillOn:
                    if (p1 == 0)
                        menuMode = MenuMode.Buttons;
                    else
                        menuMode = MenuMode.Still;
                    break;

                case EventCode.DvdStillOff:
                    if (menuMode == MenuMode.Still)
                        menuMode = MenuMode.No;
                    break;

                case EventCode.DvdButtonChange:
                    if (p1 <= 0)
                        menuMode = MenuMode.No;
                    else
                        menuMode = MenuMode.Buttons;
                    break;

                case EventCode.DvdNoFpPgc:
                    if (dvdControl2 != null)
                    {
                        int hr = dvdControl2.PlayTitle(1, DvdCmdFlags.None, _lastCmd);
                        WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);
                    }
                    break;
            }
        }

        protected override int DoGetSubtitleStream()
        {
            int nStreams = 0;
            int crtStream = 0;
            bool disabled = true;

            dvdInfo.GetCurrentSubpicture(out nStreams, out crtStream, out disabled);

            if (!disabled)
            {
                return crtStream;
            }

            return -1;
        }

        protected override void DoSetSubtitleStream(int sid)
        {
            try
            {
                int hr = dvdControl2.SelectSubpictureStream(sid, DvdCmdFlags.None, _lastCmd);
                WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);

                hr = dvdControl2.SetSubpictureState(true, DvdCmdFlags.None, _lastCmd);
                WorkerException.ThrowForHResult(WorkerError.RenderingError, hr);
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
            }
        }

        protected override double GetDurationScaleFactor()
        {
            return 1;
        }
    }


}
