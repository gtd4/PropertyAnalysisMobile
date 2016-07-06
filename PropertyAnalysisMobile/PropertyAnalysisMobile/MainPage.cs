using PropertyAnalysisMobile.CustomCells;
using PropertyAnalysisMobile.Helpers;
using PropertyAnalysisMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PropertyAnalysisMobile
{
    public class MainPage : ContentPage
    {

        Picker Regions;
        Picker Districts;
        Picker Suburbs;
        TradeMeHelper tmHelper;
        PickerHelper pckrHelper;
        ListView listView;
        PropertyFilterModel locations;
        int regionId = 0, districtId = 0, suburbId = 0, regionSelected = 0, districtSelected = 0, suburbSelected = 0;
        StackLayout panel;


        public MainPage()
        {
            pckrHelper = new PickerHelper();
            tmHelper = new TradeMeHelper();
            var props = tmHelper.GetProperties();

            InitPickers();

            SetLocalities();

            var scrollView = new ScrollView();

            panel = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Spacing = 20,
            };
            panel.Children.Add(new Label
            {
                Text = "Regions",
                XAlign = TextAlignment.Center
            });
            panel.Children.Add(Regions);

            panel.Children.Add(new Label
            {
                Text = "Districts",
                XAlign = TextAlignment.Center
            });
            panel.Children.Add(Districts);

            panel.Children.Add(new Label
            {
                Text = "Suburbs",
                XAlign = TextAlignment.Center
            });
            panel.Children.Add(Suburbs);

            scrollView.Content = panel;
            this.Content = scrollView;
        }

        /// <summary>
        /// Initializes the 3 pickers used
        /// </summary>
        private void InitPickers()
        {
            Regions = pckrHelper.InitPicker("Regions", regionSelected);         
            Regions.SelectedIndexChanged += Regions_SelectedIndexChanged;

            Districts = pckrHelper.InitPicker("Districts", districtSelected);
            Districts.SelectedIndexChanged += Districts_SelectedIndexChanged;

            Suburbs = pckrHelper.InitPicker("Suburbs", suburbSelected);         
            Suburbs.SelectedIndexChanged += Suburbs_SelectedIndexChanged;
        }

       

        /// <summary>
        /// Populate all 3 location pickers with default data
        /// </summary>
        private void SetLocalities()
        {

            locations = tmHelper.GetLocations(regionId, districtId, suburbId);

            SetLocalities(Regions, (locations.Regions.Cast<TradeMeLocationModel>().ToList()));
            SetLocalities(Districts, locations.Districts.Cast<TradeMeLocationModel>().ToList());
            SetLocalities(Suburbs, locations.Suburbs.Cast<TradeMeLocationModel>().ToList());
        }

        /// <summary>
        /// Load Properties based on filter choices
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Suburbs_SelectedIndexChanged(object sender, EventArgs e)
        {

            var picker = sender as Picker;

            suburbSelected = pckrHelper.SetSelected(picker);

            suburbId = locations.Suburbs.ElementAt(suburbSelected).Id;
            var props = tmHelper.GetProperties(regionId, districtId, suburbId);
        }

        /// <summary>
        /// Reload Suburbs Picker with correct suburb data based on District selection
        /// Load Properties based on filter choices
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Districts_SelectedIndexChanged(object sender, EventArgs e)
        {            
            var picker = sender as Picker;
            districtSelected = pckrHelper.SetSelected(picker);

            districtId = locations.Districts.ElementAt(districtSelected).Id;
            suburbId = 0;

            SetSuburbs();
            var props = tmHelper.GetProperties(regionId, districtId, suburbId);

        }

        /// <summary>
        /// reload district and suburb filters based on Region selections
        /// Load Properties based on filter choices
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Regions_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;

            var regionSelected = picker.SelectedIndex;
            regionId = locations.Regions.ElementAt(regionSelected).Id;
            suburbId = 0;
            districtId = 0;

            SetDistricts();
            var props = tmHelper.GetProperties(regionId, districtId, suburbId);
        }

        private void SetSuburbs()
        {
            locations = tmHelper.GetLocations(regionId, districtId, suburbId);
            SetLocalities(Suburbs, locations.Suburbs.Cast<TradeMeLocationModel>().ToList());
        }

        private void SetDistricts()
        {
            locations = tmHelper.GetLocations(regionId, districtId, 0);

            SetLocalities(Districts, locations.Districts.Cast<TradeMeLocationModel>().ToList());
            SetLocalities(Suburbs, locations.Suburbs.Cast<TradeMeLocationModel>().ToList());

        }

        private void SetLocalities(Picker picker, List<TradeMeLocationModel> tmlm)
        {
            picker.Items.Clear();

            foreach (var location in tmlm)
            {
                picker.Items.Add(location.Name);

            }
            picker.SelectedIndex = 0;
        }

    }
}
