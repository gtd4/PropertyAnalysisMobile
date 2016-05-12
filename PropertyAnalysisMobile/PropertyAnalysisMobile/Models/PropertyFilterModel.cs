using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyAnalysisMobile.Models
{
    public class PropertyFilterModel
    {
        public List<Region> Regions { get; set; }
        public List<District> Districts { get; set; }
        public List<Suburb> Suburbs { get; set; }
        public string Keywords { get; set; }
        public string PropertyType { get; set; }
        public int BedroomNumMax { get; set; }
        public int BedroomNumMin { get; set; }
        public int BathroomNumMax { get; set; }
        public int BathroomNumMin { get; set; }
        public decimal PriceMax { get; set; }
        public decimal PriceMin { get; set; }
    }
}
