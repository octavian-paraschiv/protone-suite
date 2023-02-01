using Newtonsoft.Json;
using OPMedia.Core.InterProcessCommunication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OPMedia.Core.Persistence
{
    public abstract class GenericPDU
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string PersistenceId { get; set; }
        public string PersistenceContext { get; set; }
        public string ObjectContent { get; set; }
        public bool IsBlob { get; set; }
    }

    public class NotificationPDU : GenericPDU
    {
        public NotificationType ChangeType { get; set; }
    }

    public class PersistencePDU : GenericPDU
    {
        public PersistenceActionType ActionType { get; set; }
    }

    public class ServicePDU : GenericPDU
    {
        public ServiceActionType ActionType { get; set; }
    }

    public enum ServiceActionType
    {
        None = 0,
        Subscribe,
        Unsubscribe,
    }

    public enum PersistenceActionType
    {
        None = 0,
        ReadObject,
        SaveObject,
        DeleteObject,
    }

    public static class PduFactory
    {
        static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        public static GenericPDU Decode(string line)
        {
            Logging.Logger.LogToConsole(line);

            PersistencePDU rpdu = JsonConvert.DeserializeObject<PersistencePDU>(line, _settings);
            if (rpdu.ActionType != PersistenceActionType.None)
                return rpdu;

            NotificationPDU npdu = JsonConvert.DeserializeObject<NotificationPDU>(line, _settings);
            if (npdu.ChangeType != NotificationType.None)
                return npdu;

            ServicePDU spdu = JsonConvert.DeserializeObject<ServicePDU>(line, _settings);
            if (spdu.ActionType != ServiceActionType.None)
                return spdu;

            return null;
        }

        public static string Encode<T>(T pdu) where T : GenericPDU
        {
            try
            {
                return JsonConvert.SerializeObject(pdu, _settings);
            }
            catch
            {
                return "";
            }
        }
    }

    public class PersistenceConstants
    {
        public const int TcpPort = 10200;
    }



    public delegate void PduReceivedHandler(string connId, GenericPDU pdu);

    public class PersistenceClient : TextCommunicationClient
    {
        private object _pduLock = new object();
        private List<string> _pendingPdus = new List<string>();
        private Dictionary<string, PersistencePDU> _recvPdus = new Dictionary<string, PersistencePDU>();

        public PduReceivedHandler PduReceived;

        public void SendPdu(GenericPDU pdu)
        {
            string line = PduFactory.Encode(pdu);
            Task.Factory.StartNew(() => Send(line));
        }

        public PersistencePDU SendPduAndWaitResponse(PersistencePDU req, int timeout = 5000)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                lock (_pduLock)
                {
                    _pendingPdus.Add(req.Id);
                }

                SendPdu(req);

                while (sw.Elapsed.TotalMilliseconds < timeout)
                {
                    lock (_pduLock)
                    {
                        if (_recvPdus.ContainsKey(req.Id))
                        {
                            var rpdu = _recvPdus[req.Id];
                            return rpdu;
                        }
                    }
                    Task.Delay(100).Wait();
                }
            }
            catch
            {
            }

            return null;
        }

        public PersistenceClient() : base(PersistenceConstants.TcpPort)
        {
            TextLineReceived = (connId, line) =>
            {
                var pdu = PduFactory.Decode(line);
                if (pdu != null)
                {
                    if (pdu is PersistencePDU rpdu)
                    {
                        lock (_pduLock)
                        {
                            if (_pendingPdus.Contains(rpdu.Id))
                            {
                                _pendingPdus.Remove(rpdu.Id);
                                _recvPdus.Add(rpdu.Id, rpdu);
                                return;
                            }
                        }
                    }

                    PduReceived?.Invoke(connId, pdu);
                }
            };
        }
    }


    public class PersistenceServer : TextCommunicationServer
    {
        public PersistenceServer() : base(PersistenceConstants.TcpPort)
        {
        }
    }
}
