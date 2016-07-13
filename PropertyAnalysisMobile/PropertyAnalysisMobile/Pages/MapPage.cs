using PropertyAnalysisMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace PropertyAnalysisMobile.Pages
{
    public class MapPage : ContentPage
    {
        PropertyMap PropertyMap;
        Geocoder geoCoder;

        public MapPage(PropertyMap map)
        {
            PropertyMap = map;
            var pin = map.Pins.ElementAt(0);

            this.Content = map;
        }

        

       
    }
}
