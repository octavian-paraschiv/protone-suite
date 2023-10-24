namespace OPMedia.Runtime.ProTONE.RemoteControl
{
    /*
    #region RemoteControllableApplication
    public sealed class RemoteControllableApplication : SingleInstanceApplication
    {
        #region Members
        private RemoteControlServer _rmtServer = null;
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
                Logger.LogWarning(string.Format("Only one instance of OpMediaApplication (or derived) can be started per process !!"));
            }
        }

        protected override void DoInitialize()
        {
            base.DoInitialize();

            _rmtServer = new RemoteControlServer();
            _rmtServer.TextLineReceived = (connId, line) => OnDataReceived(line);
        }

        protected override void DoTerminate()
        {
            _rmtServer?.Stop();
            _rmtServer?.Dispose();
            _rmtServer = null;
            
            base.DoTerminate();
        }
        #endregion

        #region Construction
        private RemoteControllableApplication(string appName)
        {
        }

        void OnDataReceived(string data)
        {
            BasicCommand cmd = BasicCommand.Create(data);
            Logger.LogTrace("OnDataReceived: {0} => cmd: {1}", data, cmd);
            EventDispatch.DispatchEvent(BasicCommand.EventName, cmd);
        }

        void _receiver_OnPostRequest(string data)
        {
            OnDataReceived(data);
        }

        string _receiver_OnSendRequest(string data)
        {
            OnDataReceived(data);
            return "ACK\r\n";
        }
        #endregion
    }
    #endregion
    */
}
