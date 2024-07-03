using Newtonsoft.Json;
using OPMedia.Core.InterProcessCommunication;
using OPMedia.Core.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace OPMedia.Core.Persistence
{
    public abstract class GenericPDU
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty(PropertyName = "PersistenceId")]
        public string NodeId { get; set; }

        [JsonProperty(PropertyName = "PersistenceContext")]
        public string Context { get; set; }

        [JsonProperty(PropertyName = "ObjectContent")]
        public string Content { get; set; }

        [JsonIgnore]
        public abstract bool IsValid { get; }
    }

    public class NotificationPDU : GenericPDU
    {
        public NotificationType ChangeType { get; set; }
        public override bool IsValid => (ChangeType != NotificationType.None);
    }

    public class PersistencePDU : GenericPDU
    {
        public PersistenceActionType ActionType { get; set; }
        public override bool IsValid => (ActionType != PersistenceActionType.None);

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string AppName { get; set; }
    }

    public class ServicePDU : GenericPDU
    {
        public ServiceActionType SvcActionType { get; set; }
        public override bool IsValid => (SvcActionType != ServiceActionType.None);
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
        ReadNode,
        SaveNode,
        DeleteNode,

        ReadAll
    }

    public static class PduFactory
    {
        private enum PduTypeIndicator
        {
            ChangeType,
            ActionType,
            SvcActionType
        }

        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        private static readonly Dictionary<PduTypeIndicator, Type> _pduTypeMap = new Dictionary<PduTypeIndicator, Type>()
        {
            { PduTypeIndicator.ActionType, typeof(PersistencePDU) },
            { PduTypeIndicator.ChangeType, typeof(NotificationPDU) },
            { PduTypeIndicator.SvcActionType, typeof(ServicePDU) },
        };

        public static GenericPDU Decode(string line)
        {
            Logging.Logger.LogToConsole(line);

            GenericPDU gpdu = default;

            if (!string.IsNullOrEmpty(line))
            {
                var pduTypeIndicators = Enum.GetValues(typeof(PduTypeIndicator)).OfType<PduTypeIndicator>();
                foreach (var pduType in pduTypeIndicators)
                {
                    if (line.ToUpperInvariant().Contains($"\"{pduType.ToString().ToUpperInvariant()}\"") &&
                        _pduTypeMap.ContainsKey(pduType))
                    {

                        var pdu = JsonConvert.DeserializeObject(line, _pduTypeMap[pduType]) as GenericPDU;
                        if ((pdu?.IsValid).GetValueOrDefault())
                            return pdu;
                    }
                }
            }

            return gpdu;
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
            ThreadPool.QueueUserWorkItem(_ => Send(line));
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
                            _recvPdus.Remove(req.Id);
                            return rpdu;
                        }
                    }
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                Logger.LogToConsole(ex.Message);
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
