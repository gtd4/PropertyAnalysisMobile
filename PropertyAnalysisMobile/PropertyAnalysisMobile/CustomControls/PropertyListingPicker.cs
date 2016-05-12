using PropertyAnalysisMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PropertyAnalysisMobile.CustomControls
{
    public class PropertyListingPicker : Picker
    {
        public IList<TradeMeLocationModel> Locations { get; }
    }
}
