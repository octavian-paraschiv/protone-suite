using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Transactions;
using OPMedia.Core;
using OPMedia.Core.Logging;
using System.Threading;
using OPMedia.Runtime.ProTONE.Galaxy;
using OPMedia.DeezerInterop.RestApi;
using System.IO;
using OPMedia.Runtime.ProTONE.FileInformation;
using System.ServiceModel.Web;

namespace OPMedia.GalaxyService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class GalaxyServiceImpl : IGalaxyService
    {
        static readonly string file = @"C:\personal\MyMusic\Chillout Lounge vol.1\10 Sade - Kiss Of Life.mp3";
        static readonly ID3FileInfo tag = new ID3FileInfo(file, false);

        public ResponseData Browse(QueryData data)
        {
            var rsp = new ResponseData
            {
                Tracks = new Track[]
                {
                    new Track
                    {
                        Album = new Album
                        {
                            Artist = new Artist
                            {
                                Id = 234567,
                                Name = tag.Artist,
                            },

                            Id = 515151,
                            Title = tag.Album,
                        },

                        Id = 65478914,
                        Title = tag.Title,
                    }

                }.ToList()
            };

            return rsp;
        }

        public Stream GetMedia(string id)
        {
            try
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "audio/mpeg";
                byte[] data = File.ReadAllBytes(file);
                return new MemoryStream(data);
            }
            catch(Exception ex)
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
                return new MemoryStream(Encoding.ASCII.GetBytes(ex.Message));
            }
        }
    }
}