using OPMedia.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace OPMedia.Core.InterProcessCommunication
{

    public abstract class CommunicationServer : CommunicationBase, IDisposable
    {
        TcpListener _listener = null;

        protected int _port;
        protected bool _localOnly;

        protected object _lock = new object();
        protected Dictionary<string, TcpClient> _clients = new Dictionary<string, TcpClient>();

        public CommunicationServer(int port, bool localOnly = true)
        {
            _port = port;
            _localOnly = localOnly;

            Start();
        }

        public void Dispose()
        {
            Stop();
        }

        public void Start()
        {
            string ipAddr = GetMachineIpV4(_localOnly);
            IPAddress addr = IPAddress.Parse(ipAddr);

            _listener = new TcpListener(addr, _port);
            _listener.Start();

            Logger.LogInfo($"accepting connections on {ipAddr}:{_port}");

            ThreadPool.QueueUserWorkItem(_ =>
            {
                while (_needStop.Wait(0) == false)
                {
                    var client = _listener.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(__ => DoClient(client));
                    Thread.Sleep(400);
                }
            });
        }

        public void Stop()
        {
            _needStop.Set();
        }

        protected static string GetMachineIpV4(bool localOnly)
        {
            if (!localOnly)
            {
                try
                {
                    foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces())
                    {
                        // It has to be a physical NIC: ethernet or WiFi
                        if (netInterface.NetworkInterfaceType != NetworkInterfaceType.Ethernet &&
                            netInterface.NetworkInterfaceType != NetworkInterfaceType.Wireless80211 &&
                            netInterface.NetworkInterfaceType != NetworkInterfaceType.Ppp)
                            continue;

                        IPInterfaceProperties ipProps = netInterface.GetIPProperties();
                        var gateway = ipProps.GatewayAddresses.FirstOrDefault();

                        if (gateway == null)
                            continue;

                        foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
                        {
                            // The NIC needs to be configured with an IPv4 address
                            if (addr.Address.AddressFamily == AddressFamily.InterNetwork)
                                return addr.Address.ToString();
                        }
                    }
                }
                catch
                {
                }
            }

            return Localhost;
        }


        private bool DoClient(TcpClient client)
        {
            if (client.Connected)
            {
                var endpoint = client.Client.RemoteEndPoint as IPEndPoint;
                string connId = $"{endpoint.Address}:{endpoint.Port}";
                bool isGracefulClose = false;

                try
                {
                    Logger.LogInfo($"Accepted connection from {connId}");
                    ConnectionOpen?.Invoke(connId, false);

                    lock (_lock)
                    {
                        if (_clients.ContainsKey(connId))
                            _clients[connId] = client;
                        else
                            _clients.Add(connId, client);
                    }

                    using (NetworkStream ns = client.GetStream())
                        isGracefulClose = HandleClientStream(connId, ns);
                }
                catch (EndOfStreamException)
                {
                    Logger.LogWarning($"Connection from {connId} closed by peer.");
                }
                catch (Exception ex)
                {
                    Logger.LogWarning($"Error on connection {connId}: {ex.Message}");
                }
                finally
                {
                    lock (_lock)
                    {
                        _clients.Remove(connId);
                    }

                    ConnectionClosed?.Invoke(connId, isGracefulClose);
                }
            }

            client.Close();
            return false;
        }
    }

    public abstract class TextCommunicationServer : CommunicationServer
    {
        protected TextCommunicationServer(int port, bool localOnly = true) : base(port, localOnly)
        {
        }

        public void SendTo(string connId, string line)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                if (_clients.ContainsKey(connId))
                {
                    var client = _clients[connId];
                    byte[] data = Encoding.UTF8.GetBytes($"{line}{Environment.NewLine}");
                    client.GetStream().Write(data, 0, data.Length);
                }
            });
        }

        public void SendGracefulEndTo(string connId) => SendTo(connId, GracefulEndMarker);

    }
}
