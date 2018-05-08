using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using OPMedia.Core.Logging;
using System.Reflection;
using OPMedia.Runtime.ProTONE.RemoteControl;
using OPMedia.Core.InstanceManagement;
//using OPMedia.Runtime.UdpCommunication;
using OPMedia.Core;
using OPMedia.Runtime.InterProcessCommunication;
using OPMedia.Core.Configuration;

namespace OPMedia.Runtime.ProTONE.RemoteControl
{
    #region RemoteControllableApplication
    public sealed class RemoteControllableApplication : SingleInstanceApplication
    {
        #region Members
        private WmCopyDataReceiver _wcdReceiver = null;
        private IPCRemoteControlHost _ipcReceiver = null;

        #endregion

        #region Methods
        public new static void Start(string appName, bool allowRealtimeGUISetup)
        {
            if (appInstance == null)
            {
                appInstance = new RemoteControllableApplication(appName);
                (appInstance as RemoteControllableApplication).DoStart(appName, allowRealtimeGUISetup);
            }
            else
            {
                Logger.LogError(string.Format("Only one instance of OpMediaApplication (or derived) can be started per process !!"));
            }
        }

        protected override void DoInitialize()
        {
            base.DoInitialize();

            _wcdReceiver = new WmCopyDataReceiver(_appName);
            _wcdReceiver.DataReceived += new DataReceivedHandler(_wcdReceiver_DataReceived);

            try
            {
                _ipcReceiver = new IPCRemoteControlHost(_appName);
                _ipcReceiver.OnSendRequest += new OnSendRequestHandler(_receiver_OnSendRequest);
                _ipcReceiver.OnPostRequest += new OnPostRequestHandler(_receiver_OnPostRequest);
            }
            catch
            {
                _ipcReceiver = null;
            }
        }

        protected override void DoTerminate()
        {
            if (_ipcReceiver != null)
            {
                _ipcReceiver.OnSendRequest -= new OnSendRequestHandler(_receiver_OnSendRequest);
            	_ipcReceiver = null;
            }

            if (_wcdReceiver != null)
            {
                _wcdReceiver.DataReceived -= new DataReceivedHandler(_wcdReceiver_DataReceived);
            	_wcdReceiver = null;
            }
            
            base.DoTerminate();
        }
        #endregion

        #region Construction
        private RemoteControllableApplication(string appName)
        {
        }

        void _wcdReceiver_DataReceived(string data)
        {
            BasicCommand cmd = BasicCommand.Create(data);
            EventDispatch.DispatchEvent(BasicCommand.EventName, cmd);
        }

        void _receiver_OnPostRequest(string data)
        {
            _wcdReceiver_DataReceived(data);
        }

        string _receiver_OnSendRequest(string data)
        {
            _wcdReceiver_DataReceived(data);
            return "ACK\r\n";
        }
        #endregion
    }
    #endregion
}
