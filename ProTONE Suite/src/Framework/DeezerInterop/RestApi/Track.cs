﻿using Newtonsoft.Json;
using System;

namespace OPMedia.DeezerInterop.RestApi
{
    public class Track : DeezerEntity
    {
        public UInt64 Id { get; set; }
        public string Title { get; set; }

        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

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

        [JsonIgnore]
        public bool HasDetails
        {
            get
            {
                return (this.Artist != null && this.Album != null);
            }
        }


        public override string ToString()
        {
            return string.Format("[ID={0}, Artist={1}, Album={2}, Title={3}, Duration={4}]",
                this.Id, this.Artist, this.Album, this.Title, (int)this.Duration.TotalSeconds);
        }

    }
}
