using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyAnalysisMobile.Models
{
    public class GeographicLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Northing { get; set; }
        public int Easting { get; set; }
    }
}
