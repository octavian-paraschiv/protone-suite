using OPMedia.DeezerInterop.RestApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace OPMedia.Runtime.ProTONE.Galaxy
{
    [ServiceContract]
    [ServiceKnownType(typeof(QueryData))]
    [ServiceKnownType(typeof(ResponseData))]
    public interface IGalaxyService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ResponseData Browse(QueryData data);

        [OperationContract]
        [WebGet(UriTemplate = "getmedia/{id}")]
        Stream GetMedia(string id);
    }

    [DataContract]
    public class QueryData
    {
        [DataMember]
        public string Artist { get; set; }

        [DataMember]
        public string Album { get; set; }

        [DataMember]
        public string Track { get; set; }

        [DataMember]
        public string Generic { get; set; }
    }

    [DataContract]
    [KnownType(typeof(Track))]
    [KnownType(typeof(Artist))]
    [KnownType(typeof(Album))]
    public class ResponseData
    {
        [DataMember]
        public List<Track> Tracks { get; set; }
    }

    public static class GalaxyConstants
    {
        public const int TcpPort = 10300;

        public static string GalaxyServiceAddress
        {
            get
            {
                return $"http://0.0.0.0:{GalaxyConstants.TcpPort}/galaxy";
            }
        }
    }
}
