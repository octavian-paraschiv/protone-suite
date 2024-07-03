﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace OPMedia.UI.ApplicationUpdate
{
    [JsonObject]
    public class BuildInfo
    {
        public string Title { get; set; }

        [JsonConverter(typeof(VersionConverter))]
        public Version Version { get; set; }

        public DateTime BuildDate { get; set; }

        public bool IsRelease { get; set; }

        public string Comment { get; set; }

        public string URL { get; set; }
    }
}
