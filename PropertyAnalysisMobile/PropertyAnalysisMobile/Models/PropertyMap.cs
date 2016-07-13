using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace PropertyAnalysisMobile.Models
{
    public class PropertyMap : Map
    {
        public List<Position> RouteCoordinates { get; set; }

        public PropertyMap()
        {
            RouteCoordinates = new List<Position>();
        }

        public PropertyMap(MapSpan region) : base(region)
        {
        }
    }
}
