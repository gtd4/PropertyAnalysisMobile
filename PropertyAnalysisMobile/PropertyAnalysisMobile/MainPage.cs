using PropertyAnalysisMobile.CustomCells;
using PropertyAnalysisMobile.Helpers;
using PropertyAnalysisMobile.Models;
using PropertyAnalysisMobile.Pages;
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
        Button searchBtn;
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

            searchBtn = new Button();
            searchBtn.Text = "Search";
            searchBtn.Clicked += HandleTouchUpInside;

            panel.Children.Add(searchBtn);

        }

        void HandleTouchUpInside(object sender, EventArgs ea)
        {
            Navigation.PushAsync(new PropertyListingPage(regionId,districtId,suburbId));
        }

        /// <summary>
        /// Initializes the 3 pickers used
        /// </summary>
        private void InitPickers()
        {
            using (var profiler = new Profiler("Init Pickers: "))
            {
                Regions = pckrHelper.InitPicker("Regions", regionSelected);
                Regions.SelectedIndexChanged += Regions_SelectedIndexChanged;

                Districts = pckrHelper.InitPicker("Districts", districtSelected);
                Districts.SelectedIndexChanged += Districts_SelectedIndexChanged;

                Suburbs = pckrHelper.InitPicker("Suburbs", suburbSelected);
                Suburbs.SelectedIndexChanged += Suburbs_SelectedIndexChanged;
            }
        }

       

        /// <summary>
        /// Populate all 3 location pickers with default data
        /// </summary>
        private async void SetLocalities()
        {

            Task<PropertyFilterModel> locales = tmHelper.GetLocations(regionId, districtId, suburbId);
            locations = await locales;

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


        }

        /// <summary>
        /// reload district and suburb filters based on Region selections
        /// Load Properties based on filter choices
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Regions_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var prof = new Profiler("Regions_SelectedIndexChanged"))
            {
                var picker = sender as Picker;

                var regionSelected = picker.SelectedIndex;
                regionId = locations.Regions.ElementAt(regionSelected).Id;
                suburbId = 0;
                districtId = 0;

                SetDistricts();
            }

        }

        private async void SetSuburbs()
        {
            Task<PropertyFilterModel> locales = tmHelper.GetLocations(regionId, districtId, suburbId);
            locations = await locales;
            SetLocalities(Suburbs, locations.Suburbs.Cast<TradeMeLocationModel>().ToList());
        }

        private async void SetDistricts()
        {
            using (var prof = new Profiler("SetDistricts"))
            {
                Task<PropertyFilterModel> locales = tmHelper.GetLocations(regionId, districtId, suburbId);
                locations = await locales;

                SetLocalities(Districts, locations.Districts.Cast<TradeMeLocationModel>().ToList());
                //SetLocalities(Suburbs, locations.Suburbs.Cast<TradeMeLocationModel>().ToList());
            }

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
