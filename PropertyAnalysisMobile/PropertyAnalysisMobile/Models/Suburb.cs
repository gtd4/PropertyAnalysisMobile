using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyAnalysisMobile.Models
{
    public class Suburb : TradeMeLocationModel
    {
        [JsonProperty("SuburbId")]
        public override int Id { get; set; }

        [JsonProperty("Name")]
        public override string Name { get; set; }

        [JsonProperty("AdjacentSuburbs")]
        List<int> AdjacentSuburbs { get; set; }

        [JsonProperty("Count")]
        public int Count { get; set; }
    }
}
