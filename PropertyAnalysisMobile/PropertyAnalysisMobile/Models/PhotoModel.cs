using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyAnalysisMobile.Models
{
    public class PhotoModel
    {
        [JsonProperty("Key")]
        public int PhotoId { get; set; }

        [JsonProperty("Value")]
        public PhotoUrl PhotoUrl { get; set; }
    }
}
