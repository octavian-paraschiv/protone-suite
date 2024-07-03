using OPMedia.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace OPMedia.Runtime.ProTONE.FfdShowApi
{
    /// <summary>
    /// FFDShowAPI library class. Use this class to get/set FFDShow live settings
    /// </summary>
    internal class FfdShowLib
    {
        const int FFDSM_SET_SHORTOSD_MSG = 18;
        const int FFDSM_SET_OSD_MSG = 19;
        static string strAppName = "ffdshow_remote_class";
        const uint FFDShowAPIRemoteId = 32786;

        #region Variables
        /// <summary>
        /// Unique identifier of the running instance of FFDShow
        /// </summary>
        protected IntPtr ffDShowInstanceHandle = IntPtr.Zero;
        private int requestTimeout = 2000;
        private FFDShowReceiver receiver = null;
        private string fileName = null;

        #endregion Variables

        #region Properties
        public bool IsActive => (ffDShowInstanceHandle != IntPtr.Zero && User32.IsWindow(ffDShowInstanceHandle));

        /// <summary>
        /// Gets the FFDShow instance handle (number that identifies the FFDShow instance)
        /// </summary>
        public IntPtr FFDShowInstanceHandle
        {
            get
            {
                return ffDShowInstanceHandle;
            }
        }
        // Show/hide OSD
        /// <summary>
        /// Enable or disable subtitles filter
        /// </summary>
        public bool DoShowOSD
        {
            get
            {
                int value = getIntParam(FFDShowConstants.FFDShowDataId.IDFF_isOSD);
                return (value == 1);
            }

            set
            {
                setIntParam(FFDShowConstants.FFDShowDataId.IDFF_isOSD, value ? 1 : 0);
            }
        }

        // Show/hide subtitles
        /// <summary>
        /// Enable or disable subtitles filter
        /// </summary>
        public bool DoShowSubtitles
        {
            get
            {
                int value = getIntParam(FFDShowConstants.FFDShowDataId.IDFF_isSubtitles);
                if (value == 1)
                    return true;
                else return false;
            }
            set
            {
                if (value)
                    setIntParam(FFDShowConstants.FFDShowDataId.IDFF_isSubtitles, 1);
                else
                    setIntParam(FFDShowConstants.FFDShowDataId.IDFF_isSubtitles, 0);
            }
        }

        /// <summary>
        /// Set/get the current external subtitles file
        /// </summary>
        public string CurrentSubtitleFile
        {
            get
            {
                return getCustomParam(FFD_MSG.GET_CURRENT_SUBTITLES, 0);//FFDSM_GET_CURRENT_SUBTITLES);
            }
            set
            {
                setStringParam(FFDShowConstants.FFDShowDataId.IDFF_subTempFilename, value);
                setIntParam(FFDShowConstants.FFDShowDataId.IDFF_isSubtitles, 1);
                setIntParam(FFDShowConstants.FFDShowDataId.IDFF_subShowEmbedded, 0);
            }
        }

        /// <summary>
        /// Returns true if the subtitle filter is enabled, false otherwise
        /// </summary>
        public bool SubtitlesEnabled
        {
            get
            {
                return (getIntParam(FFDShowConstants.FFDShowDataId.IDFF_isSubtitles) == 1) ? true : false;
            }
            set
            {
                setIntParam(FFDShowConstants.FFDShowDataId.IDFF_isSubtitles, (value == true) ? 1 : 0);
            }
        }

        #endregion Properties


        #region Constructors
        /// <summary>
        /// Basic constructor using interprocess communication
        /// </summary>
        private FfdShowLib()
        {
            receiver = new FFDShowReceiver(Thread.CurrentThread);
        }

        /// <summary>
        /// Searches the running FFDShow instance which renders the specified file
        /// </summary>
        public static FfdShowLib findFfdShowInstance(string fileName)
        {
            List<IntPtr> instancesArray = new List<IntPtr>();
            GCHandle gch = GCHandle.Alloc(instancesArray);

            if (User32.EnumWindows(new User32.EnumWindowProc(EnumWindowCallBack), (IntPtr)gch) != 0 &&
                instancesArray?.Count > 0)
            {
                foreach (IntPtr handle in instancesArray)
                {
                    FfdShowLib ffdShowAPI = new FfdShowLib { ffDShowInstanceHandle = handle };
                    bool dispose = true;

                    try
                    {
                        string ffdShowFileName = ffdShowAPI.getFileName();
                        var p1 = Path.GetFullPath(ffdShowFileName ?? "");
                        var p2 = Path.GetFullPath(fileName ?? "");
                        if (string.Compare(p1, p2, true) == 0)
                        {
                            dispose = false;
                            ffdShowAPI.fileName = ffdShowFileName;
                            return ffdShowAPI;
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {
                        if (dispose)
                            ffdShowAPI?.Dispose();
                    }
                }
            }

            return null;
        }

        private static bool EnumWindowCallBack(IntPtr hwnd, IntPtr lParam)
        {
            GCHandle gch = (GCHandle)lParam;
            List<IntPtr> instancesArray = (List<IntPtr>)gch.Target;
            StringBuilder sbc = new StringBuilder(1024);
            User32.GetClassName(hwnd, sbc, sbc.Capacity);
            if (sbc.Length > 0)
            {
                if (sbc.ToString().Equals(strAppName))
                    instancesArray.Add(hwnd);
            }

            return true;
        }
        #endregion 

        #region Commands
        /// <summary>
        /// Retrieve the frame rate
        /// </summary>
        /// <returns>Retrieve the frame rate (float with decimals eventually)</returns>
        public float getFrameRate()
        {
            int fps1000 = SendMessage(FFD_WPRM.GET_FRAMERATE, 0);
            return (float)fps1000 / 1000;
        }


        /// <summary>
        /// Retrieve the file name being played
        /// </summary>
        /// <returns>File name</returns>
        public string getFileName()
        {
            return getCustomParam(FFD_MSG.GET_SOURCEFILE, 0);//FFDSM_GET_FILENAME);
        }

        /// <summary>
        /// Display a short OSD (On Screen Display) message
        /// This message will be displayed a few seconds and will disappear automatically
        /// </summary>
        /// <param name="message">Message to be displayed</param>
        public void displayOSDMessage(string message, bool shortMessage)
        {
            COPYDATASTRUCT cd = new COPYDATASTRUCT();
            cd.dwData = new UIntPtr(shortMessage ? (uint)FFDSM_SET_SHORTOSD_MSG : (uint)FFDSM_SET_OSD_MSG);
            cd.lpData = Marshal.StringToHGlobalUni(message);
            cd.cbData = (uint)Kernel32.GlobalSize(cd.lpData);

            User32.SendMessage(ffDShowInstanceHandle, (int)Messages.WM_COPYDATA, 0, ref cd);
        }

        public void setOsdDuration(int duration)
        {
            SendMessage(FFD_WPRM.SET_WPRM_SET_OSDDURATION, duration);
        }

        public void clearOsd()
        {
            SendMessage(FFD_WPRM.SET_WPRM_SET_OSD_CLEAN, 0);
        }

        #endregion Commands

        #region Base commands
        /// <summary>
        /// Retrieve a parameter from FFDShow. The requested parameter must match to an integer type
        /// </summary>
        /// <param name="param">Parameter to retrieve</param>
        /// <returns>Value of the parameter</returns>
        public int getIntParam(FFDShowConstants.FFDShowDataId param)
        {
            return SendMessage(FFD_WPRM.GET_PARAM_VALUE_INT, (int)param);
        }

        /// <summary>
        /// Set the value of a parameter to FFDShow. The requested parameter must match to an integer type
        /// </summary>
        /// <param name="param">Parameter to set</param>
        /// <param name="value">Value to set</param>
        public void setIntParam(FFDShowConstants.FFDShowDataId param, int value)
        {
            SendMessage(FFD_WPRM.SET_PARAM_NAME, (int)param);
            SendMessage(FFD_WPRM.SET_PARAM_VALUE_INT, value);
        }

        /// <summary>
        /// Retrieve a parameter from FFDShow. The requested parameter must match to a string type
        /// </summary>
        /// <param name="type">Type of parameter to retrieve.</param>
        /// <param name="param">Empty if type is different from FFD_MSG.GETPARAMSTR, otherwise the identifier of the string parameter to retrieve</param>
        /// <returns></returns>
        public string getCustomParam(FFD_MSG type, FFDShowConstants.FFDShowDataId param)
        {
            if (receiver == null)
                receiver = new FFDShowReceiver(Thread.CurrentThread);

            receiver.ReceivedString = null;
            receiver.ReceivedType = 0;

            User32.SendMessageTimeout(ffDShowInstanceHandle, (int)type, receiver.Handle, new IntPtr((int)param),
                SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, requestTimeout, out IntPtr ret);

            if (ret.ToInt32() != 1)
                return null;

            if (receiver.ReceivedType == 0)
            {
                try
                {
                    Thread.Sleep(requestTimeout);
                }
                catch (ThreadInterruptedException)
                {
                };
            }

            // Check that the received string corresponds to the paramId we requested
            if ((param != 0 && receiver.ReceivedType == (int)param) || receiver.ReceivedType == (int)type)
                return receiver.ReceivedString;
            else return null;
        }

        /// <summary>
        /// Retrieve a string parameter from FFDShow.
        /// Same behaviour as getCustomParam(FFD_MSG.GETPARAMSTR, param)
        /// </summary>
        /// <param name="param">Parameter to retrieve</param>
        /// <returns>String value of the parameter</returns>
        public string getStringParam(FFDShowConstants.FFDShowDataId param)
        {
            return getCustomParam(FFD_MSG.GET_PARAMSTR, param);
        }

        /// <summary>
        /// Set a string parameter to FFDShow
        /// </summary>
        /// <param name="param">Identifier of the parameter</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int setStringParam(FFDShowConstants.FFDShowDataId param, string value)
        {
            SendMessage(FFD_WPRM.SET_PARAM_NAME, (int)param);

            COPYDATASTRUCT cd = new COPYDATASTRUCT();
            cd.dwData = new UIntPtr((uint)FFD_WPRM.SET_PARAM_VALUE_STR);
            cd.lpData = Marshal.StringToHGlobalUni(value);
            cd.cbData = (uint)Kernel32.GlobalSize(cd.lpData);

            User32.SendMessageTimeout(ffDShowInstanceHandle, (int)Messages.WM_COPYDATA,
                receiver.Handle, ref cd, SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, requestTimeout,
                out IntPtr returnedValue);

            Marshal.FreeHGlobal(cd.lpData);
            return returnedValue.ToInt32();
        }

        private int SendMessage(FFD_WPRM wParam, int lParam)
        {
            return User32.SendMessage(ffDShowInstanceHandle, (int)FFDShowAPIRemoteId, (int)wParam, lParam);
        }

        public void Dispose()
        {
            receiver?.Dispose();
            receiver = null;
        }

        #endregion Base commands


    }
}