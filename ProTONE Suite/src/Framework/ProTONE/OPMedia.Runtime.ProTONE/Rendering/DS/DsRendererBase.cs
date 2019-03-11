#if HAVE_DSHOW

using System;
using System.Collections.Generic;
using System.Text;
using OPMedia.Runtime.ProTONE.Rendering.Base;

using OPMedia.Core.Logging;
using System.Runtime.InteropServices;
using OPMedia.Core;
using System.Windows.Forms;


using System.Diagnostics;

using DS = OPMedia.Runtime.ProTONE.Rendering.DS;
using System.Threading;
using OPMedia.Runtime.ProTONE.Rendering.DS.BaseClasses;

using System.Linq;
using OPMedia.Runtime.DSP;
using System.Collections.Concurrent;
using OPMedia.Runtime.ProTONE.Configuration;
using NAudio.CoreAudioApi;
using System.IO;

namespace OPMedia.Runtime.ProTONE.Rendering.DS
{
    public abstract class DsRendererBase : StreamRenderer
    {
        public const int MAX_SPECTROGRAM_BANDS = 64;
        const int WAVEFORM_WNDSIZEFACTOR = 128;
        const int VU_WNDSIZEFACTOR = 4096;
        const int FFT_WNDSIZEFACTOR = 16;

        protected IMediaControl mediaControl = null;
        protected IVideoWindow videoWindow = null;
        protected IBasicAudio basicAudio = null;
        protected IBasicVideo basicVideo = null;
        protected IMediaPosition mediaPosition = null;
        protected IMediaEventEx mediaEvent = null;

        protected bool isVideoAvailable = false;
        protected bool isAudioAvailable = false;

        protected double durationScaleFactor = 1;

        internal int VideoWidth
        {
            get
            {
                int w = 0;
                int hr = basicVideo.get_VideoWidth(out w);
                if (hr >= 0)
                    return w;

                return 0;
            }
        }

        internal int VideoHeight
        {
            get
            {
                int h = 0;
                int hr = basicVideo.get_VideoHeight(out h);
                if (hr >= 0)
                    return h;

                return 0;
            }
        }


        public DsRendererBase()
        {
            GraphNotifyWnd.Instance.FilterGraphMessage +=
                new System.Windows.Forms.MethodInvoker(OnFilterGraphMessage);
        }

        private void OnFilterGraphMessage()
        {
            int p1, p2;
            EventCode code = EventCode.None;

            if (mediaEvent == null)
                return;

            while (mediaEvent.GetEvent(out code, out p1, out p2, 0) == 0)
            {
                try
                {
                    int hr = mediaEvent.FreeEventParams(code, p1, p2);
                    DsError.ThrowExceptionForHR(hr);

                    Logger.LogTrace("HandleGraphEvent: code={0} p1={1} p2={2}", code, p1, p2);
                    HandleGraphEvent(code, p1, p2);
                }
                catch (COMException ex)
                {
                    Logger.LogTrace("Filter graph message loop broken because: {0}",
                        ErrorDispatcher.GetErrorMessageForException(ex, false));
                    break;
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                    break;
                }
            }
        }

        protected abstract void HandleGraphEvent(EventCode code, int p1, int p2);

        

        protected override void DoStartRenderer()
        {
            // Leave empty; every particular renderer will do its own startup.
        }

        protected override void DoStartRendererWithHint(RenderingStartHint startHint)
        {
            // Leave empty; every particular renderer will do its own startup.
        }

        protected override void DoPauseRenderer()
        {
            if (mediaControl != null)
            {
                int hr = mediaControl.Pause();
                DsError.ThrowExceptionForHR(hr);
            }
        }

        protected override void DoStopRenderer()
        {
            GC.Collect();

            if (isVideoAvailable)
            {
                renderRegion.Resize -= new EventHandler(renderPanel_Resize);
                DoStopInternal(null);

                // Not nice, but this avoids player freezing
                // sometimes when stopping video media. 
                //ThreadPool.QueueUserWorkItem(AsyncStop);
                //Thread.Sleep(1000);
            }
            else
            {
                DoStopInternal(null);
            }
        }

        protected virtual void DoStopInternal(object state)
        {
            try
            {
                if (mediaControl != null)
                {
                    mediaControl.Stop();

                    mediaControl = null;
                    mediaPosition = null;
                    videoWindow = null;
                    basicVideo = null;
                    basicAudio = null;
                    mediaEvent = null;
                }

                GC.Collect();
            }
            catch (Exception ex)
            {
                // This is running on other thread than the DSRenderer,
                // so its exceptions are not caught in MediaRenderer
                Logger.LogException(ex);
            }
        }

        protected void InitAudioAndVideo()
        {
            try
            {
                //ResetVolumeLevels();

                //int val = 0;

                int w = 0;
                int hr = basicVideo.get_VideoWidth(out w);
                isVideoAvailable = (hr >= 0 && w > 0);

                // Setup the video window
                SetupVideoWindow();

                FitVideoInPanel();

                renderRegion.Visible = true;

                renderRegion.Resize -= new EventHandler(renderPanel_Resize);
                renderRegion.Resize += new EventHandler(renderPanel_Resize);
            }
            catch
            {
                isVideoAvailable = false;
            }

            try
            {
                int hr = basicAudio.put_Volume((int)VolumeRange.Minimum);
                isAudioAvailable = (hr >= 0);
            }
            catch
            {
                isAudioAvailable = false;
            }
        }

        private void SetupVideoWindow()
        {
            int hr = 0;

            if (renderRegion != null && videoWindow != null)
            {
                hr = videoWindow.put_Owner(renderRegion.Handle);
                DsError.ThrowExceptionForHR(hr);

                hr = videoWindow.put_MessageDrain(renderRegion.Handle);
                DsError.ThrowExceptionForHR(hr);
            }

            if (videoWindow != null)
            {
                hr = videoWindow.put_WindowStyle((int)(WindowStyle.Child |
                    WindowStyle.ClipSiblings |
                    WindowStyle.ClipChildren));
                DsError.ThrowExceptionForHR(hr);
            }

            if (mediaEvent != null)
            {
                hr = mediaEvent.SetNotifyWindow(GraphNotifyWnd.Instance.Handle, (int)Messages.WM_GRAPH_EVENT, IntPtr.Zero);
                DsError.ThrowExceptionForHR(hr);
            }
        }


        protected void renderPanel_Resize(object sender, EventArgs e)
        {
            if (videoWindow != null)
            {
                FitVideoInPanel();
            }
        }

        protected void FitVideoInPanel()
        {
            int width, height;
            int left = 0, top = 0;

            double videoAspectRatio =
                (double)(VideoWidth) / (double)(VideoHeight);
            double panelAspectRatio = (double)renderRegion.Width / (double)renderRegion.Height;

            if (videoAspectRatio >= panelAspectRatio)
            {
                // "wide" video, use width as basis
                width = renderRegion.Width;
                height = (int)(width / videoAspectRatio);
                left = 0;
                top = (renderRegion.Height - height) / 2;
            }
            else
            {
                // "tall" video, use height as basis
                height = renderRegion.Height;
                width = (int)(videoAspectRatio * height);
                left = (renderRegion.Width - width) / 2;
                top = 0;
            }

            renderRegion.SuspendLayout();

            int hr = videoWindow.SetWindowPosition(left, top, width, height);
            DsError.ThrowExceptionForHR(hr);

            renderRegion.ResumeLayout();
        }

        protected override void DoResumeRenderer(double fromPosition)
        {
            if (fromPosition > 0)
                SetMediaPosition(fromPosition);

            if (mediaControl != null)
            {
                int hr = mediaControl.Run();
                DsError.ThrowExceptionForHR(hr);
            }
        }

        protected override double GetMediaLength()
        {
            double val = 0;

            if (mediaPosition != null)
            {
                int hr = mediaPosition.get_Duration(out val);
                if (hr >= 0)
                    return (val + double.Epsilon);
            }

            return double.Epsilon;
        }

        protected override double GetMediaPosition()
        {
            double val = 0;

            if (mediaPosition != null)
            {
                int hr = mediaPosition.get_CurrentPosition(out val);
                DsError.ThrowExceptionForHR(hr);
            }

            return val * durationScaleFactor;
        }

        protected override void SetMediaPosition(double pos)
        {
            if (mediaPosition != null)
            {
                int hr = mediaPosition.put_CurrentPosition(pos / durationScaleFactor);
                DsError.ThrowExceptionForHR(hr);
            }
        }

        protected override int GetAudioVolume()
        {
            int val = -1;
            if (isAudioAvailable)
            {
                int hr = basicAudio.get_Volume(out val);
                DsError.ThrowExceptionForHR(hr);
            }

            return val;
        }

        protected override void SetAudioVolume(int vol)
        {
            if (isAudioAvailable)
            {
                if (vol < -10000)
                    vol = -10000;
                else if (vol > 0)
                    vol = 0;

                int hr = basicAudio.put_Volume(vol);
                DsError.ThrowExceptionForHR(hr);
            }
        }

        protected override int GetAudioBalance()
        {
            int val = 0;
            if (isAudioAvailable)
            {
                int hr = basicAudio.get_Balance(out val);
                DsError.ThrowExceptionForHR(hr);
            }

            return val;
        }

        protected override void SetAudioBalance(int b)
        {
            if (isAudioAvailable)
            {
                if (b < -10000)
                    b = -10000;
                else if (b > 10000)
                    b = 10000;

                int hr = basicAudio.put_Balance(b);
                DsError.ThrowExceptionForHR(hr);
            }
        }

        protected override bool IsVideoMediaAvailable()
        {
            return isVideoAvailable;
        }

        protected override bool IsAudioMediaAvailable()
        {
            return isAudioAvailable;
        }

        protected override bool IsMediaSeekable()
        {
            OABool seekFwd = OABool.False, seekBwd = OABool.False;
            if (mediaPosition != null)
            {
                int hr = mediaPosition.CanSeekForward(out seekFwd);
                DsError.ThrowExceptionForHR(hr);

                hr = mediaPosition.CanSeekBackward(out seekBwd);
                DsError.ThrowExceptionForHR(hr);
            }

            return (seekFwd != OABool.False && seekBwd != OABool.False);
        }

        protected override BaseClasses.FilterState GetFilterState()
        {
            BaseClasses.FilterState fs = BaseClasses.FilterState.Stopped;
            if (mediaControl != null)
            {
                int hr = mediaControl.GetState(0, out fs);
                DsError.ThrowExceptionForHR(hr);
            }

            return fs;
        }

        protected override bool IsCursorVisible()
        {
            bool retVal = false;
            if (isVideoAvailable && videoWindow != null)
            {
                int hidden = (int)OABool.False;

                int hr = videoWindow.IsCursorHidden(out hidden);
                DsError.ThrowExceptionForHR(hr);

                retVal = (hidden == (int)OABool.False);
            }

            return retVal;
        }

        protected override void DoShowCursor(bool show)
        {
            if (isVideoAvailable && videoWindow != null)
            {
                int hidden = (show) ? (int)OABool.False : (int)OABool.True;
                int hr = videoWindow.HideCursor(hidden);
                DsError.ThrowExceptionForHR(hr);
            }
        }
        protected override void DoAdjustVideoSize(VideoSizeAdjustmentDirection direction, VideoSizeAdjustmentAction action)
        {
        }

        protected override bool IsEndOfMedia()
        {
            long nMediaPos = (long)GetMediaPosition();
            long nMediaLen = (long)GetMediaLength();
            long nElapsed = base._elapsedSeconds;

            return (nElapsed >= nMediaLen || nMediaPos >= nMediaLen);
        }

        protected override bool IsFullScreen()
        {
            //if (videoWindow != null)
            //{
            //    return videoWindow.FullScreenMode == DsConstants.DsTrue;
            //}

            return false;
        }

        protected override void DoSetFullScreen(bool fullScreen)
        {
            //if (videoWindow != null)
            //{
            //    if (fullScreen)
            //    {
            //        _bakMessageDrain = videoWindow.MessageDrain;
            //        videoWindow.MessageDrain = (int)(MainThread.MainWindow.Handle);
            //        videoWindow.FullScreenMode = DsConstants.DsTrue;
            //    }
            //    else
            //    {
            //        videoWindow.FullScreenMode = OABool.False;
            //        videoWindow.MessageDrain = (int)(_bakMessageDrain);
            //        videoWindow.SetWindowForeground(DsConstants.DsTrue);

            //    }
            //}
        }

        protected override object DoGetGraphFilter()
        {
            return mediaControl;
        }

        public static IMediaControl BuildMediaControl()
        {
            Guid filterGraphGuid =  ProTONEConfig.FilterGraphGuid;

            Type mediaControlType = Type.GetTypeFromCLSID(filterGraphGuid, true);

            return Activator.CreateInstance(mediaControlType) as IMediaControl;
        }
    }

    internal class GraphNotifyWnd : NativeWindow
    {
        public event MethodInvoker FilterGraphMessage;

        private static GraphNotifyWnd _instance = new GraphNotifyWnd();

        public static GraphNotifyWnd Instance
        {
            get
            {
                return _instance;
            }
        }

        private GraphNotifyWnd()
        {
            CreateParams cp = new CreateParams();
            cp.Style = (int)WindowStyle.Disabled;
            cp.ExStyle = (int)WindowExtendedStyles.WS_EX_TRANSPARENT;
            CreateHandle(cp);
        }

        protected override void WndProc(ref Message m)
        {
            Logger.LogTrace("GraphNotifyWnd: " + m);

            if (m.Msg == (int)Messages.WM_GRAPH_EVENT &&
                FilterGraphMessage != null)
            {
                FilterGraphMessage();
                return;
            }

            base.WndProc(ref m);
        }
    }

    public class DsCustomRenderer : DsRendererBase
    {
        protected override void DoStartRenderer()
        {
            DoStartRendererWithHint(DvdRenderingStartHint.Beginning);
        }

        protected override void DoStopInternal(object state)
        {
            IFilterGraph graph = mediaControl as IFilterGraph;
            if (graph != null)
            {
                if (mediaControl != null)
                {
                    int hr = mediaControl.Stop();
                    DsError.ThrowExceptionForHR(hr);
                    mediaControl = null;
                }

                IEnumFilters pEnum = null;
                if (COMHelper.SUCCEEDED(graph.EnumFilters(out pEnum)) && pEnum != null)
                {
                    List<IBaseFilter> allFilters = new List<IBaseFilter>();

                    IBaseFilter[] aFilters = new IBaseFilter[1];
                    while (COMHelper.S_OK == pEnum.Next(1, aFilters, IntPtr.Zero))
                    {
                        allFilters.Add(aFilters[0]);
                    }
                    Marshal.ReleaseComObject(pEnum);

                    foreach (var f in allFilters)
                    {
                        if (f != null)
                        {
                            graph.RemoveFilter(f);
                        }
                    }
                }
            }

            base.DoStopInternal(state);
        }

        protected override void HandleGraphEvent(EventCode code, int p1, int p2)
        {
            Logger.LogTrace("GraphEvent: {0} : {1} : {2}", code, p1, p2);
        }

        protected override double GetDurationScaleFactor()
        {
            return 1;
        }

        protected override int DoGetSubtitleStream()
        {
            // Not required at this point for file renderers.
            return -1;
        }

        protected override void DoSetSubtitleStream(int sid)
        {
            // Not required at this point for file renderers.
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class AudioSample
    {
        public double SampleTime;
        public byte[] RawSamples;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SampleTime: {0}...", SampleTime);

            //if (RawSamples != null)
            //{
            //    for (int i = 0; i < RawSamples.Length; i++)
            //        sb.AppendFormat("sample[{0}]={1}...", i, RawSamples[i]);
            //}

            return sb.ToString();
        }
    }
}

#endif