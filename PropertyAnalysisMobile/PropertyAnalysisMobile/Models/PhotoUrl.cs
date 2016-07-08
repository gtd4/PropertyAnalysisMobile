﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyAnalysisMobile.Models
{
    public class PhotoUrl
    {
        [JsonProperty("Thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("Large")]
        public string Large { get; set; }
    }
}
