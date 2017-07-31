using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace OPMedia.DeezerInterop.RestApi
{
    public class Track : DeezerEntity
    {
        public uint Id { get; set; }
        public string Title { get; set; }

        [JsonProperty("duration")]
        private double InternalDuration
        {
            get
            {
                return Duration.TotalSeconds;
            }
            set
            {
                Duration = TimeSpan.FromSeconds(value);
            }
        }

        [JsonIgnore]
        public TimeSpan Duration { get; set; }
        public Uri Preview { get; set; }
        public Artist Artist { get; set; }
        public Album Album { get; set; }
    }
}
