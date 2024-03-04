using OPMedia.Core.Logging;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace OPMedia.Core.InterProcessCommunication
{
    public abstract class CommunicationClient : CommunicationBase, IDisposable
    {
        protected TcpClient _client = null;

        protected int _port;
        protected string _address;

        public int ReconnectTime { get; set; } = 10000;

        public CommunicationClient(int port, string address = null)
        {
            _port = port;
            _address = address ?? Localhost;

            Logger.LogInfo($"Attempt to connect to {_address}:{_port}");
            Connect(false);
        }

        public void Dispose()
        {
            Disconnect();
        }

        public void Connect(bool reconnect)
        {
            if (_client != null)
            {
                _client.Close();
                _client = null;
            }

            ThreadPool.QueueUserWorkItem(_ =>
            {
                _client = new TcpClient(_address, _port);
                bool gracefulClose = false;

                if (_client.Connected)
                {
                    var endpoint = _client.Client.RemoteEndPoint as IPEndPoint;
                    string connId = $"{endpoint.Address}:{endpoint.Port}";

                    try
                    {
                        Logger.LogInfo($"Connected to {connId}");
                        base.ConnectionOpen?.Invoke(connId, reconnect);

                        using (NetworkStream ns = _client.GetStream())
                            gracefulClose = HandleClientStream(connId, ns);
                    }
                    catch (EndOfStreamException)
                    {
                        Logger.LogWarning($"Connection to {connId} closed by peer.");
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWarning($"Error on connection {connId}: {ex.Message}");
                    }
                    finally
                    {
                        ConnectionClosed?.Invoke(connId, gracefulClose);
                    }
                }
                else
                {
                    Logger.LogWarning($"Not connected to {_address}:{_port}");
                }

                if (ReconnectTime > 0 && !gracefulClose)
                {
                    ThreadPool.QueueUserWorkItem(__ =>
                    {
                        Thread.Sleep(ReconnectTime);
                        Logger.LogInfo($"Attempt to reconnect to {_address}:{_port}");
                        Connect(true);
                    });
                }
            });
        }

        public void Disconnect()
        {
            _needStop.Set();
        }
    }

    public abstract class TextCommunicationClient : CommunicationClient
    {
        public void Send(string line)
        {
            byte[] data = Encoding.UTF8.GetBytes($"{line}{Environment.NewLine}");
            _client?.GetStream().Write(data, 0, data.Length);
        }

        public void SendGracefulEnd() => Send(GracefulEndMarker);

        protected TextCommunicationClient(int port, string address = null) : base(port, address)
        {
        }
    }
}
