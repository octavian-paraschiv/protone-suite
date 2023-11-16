using OPMedia.Core.Logging;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace OPMedia.Core.InterProcessCommunication
{
    public delegate void ConnectionOpenHandler(string connId, bool isReconnect);
    public delegate void ConnectionClosedHandler(string connId, bool isGracefully);
    public delegate void TextLineReceivedHandler(string connId, string line);

    public abstract class CommunicationBase
    {
        protected const string Localhost = "127.0.0.1";
        protected const string GracefulEndMarker = "--end--";

        protected ManualResetEventSlim _needStop = new ManualResetEventSlim(false);

        public ConnectionOpenHandler ConnectionOpen;
        public ConnectionClosedHandler ConnectionClosed;
        public TextLineReceivedHandler TextLineReceived;

        protected virtual bool HandleClientStream(string connId, NetworkStream ns)
        {
            using (StreamReader sr = new StreamReader(ns))
            using (StreamWriter sw = new StreamWriter(ns))
            {
                while (_needStop.Wait(0) == false)
                {
                    try
                    {
                        string line = sr.ReadLine();
                        if (string.Compare(line, GracefulEndMarker, true) == 0)
                        {
                            Logger.LogInfo($"The remote party at {connId} asked us to close the connection gracefully.");
                            sw.WriteLine(GracefulEndMarker);
                            return true; // graceful exit
                        }

                        TextLineReceived?.Invoke(connId, line);
                    }
                    catch (IOException ioex)
                    {
                        Logger.LogWarning($"The connection to {connId} closed unexpectedly: {ioex.Message}");
                        break;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWarning($"Error on connection {connId}: {ex.Message}");
                        break;
                    }
                }
            }

            return false;
        }
    }
}
