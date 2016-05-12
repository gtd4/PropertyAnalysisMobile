using PropertyAnalysisMobile.CustomCells;
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
        ListView listView;
        PropertyFilterModel locations;
        int regionId = 0, districtId = 0, suburbId = 0, regionSelected = 0, districtSelected = 0, suburbSelected = 0;
        StackLayout panel;


        public MainPage()
        {
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

            AddProperties(props, panel);

            scrollView.Content = panel;
            this.Content = scrollView;
        }

        private void InitPickers()
        {
            Regions = new Picker();
            Regions.SelectedIndex = regionSelected;
            Regions.Title = "Regions";
            Regions.SelectedIndexChanged += Regions_SelectedIndexChanged;

            Districts = new Picker();
            Districts.SelectedIndex = districtSelected;
            Districts.Title = "Districts";
            Districts.SelectedIndexChanged += Districts_SelectedIndexChanged;

            Suburbs = new Picker();
            Suburbs.SelectedIndex = suburbSelected;
            Suburbs.Title = "Suburbs";
            Suburbs.SelectedIndexChanged += Suburbs_SelectedIndexChanged;
        }

        private void AddProperties(List<PropertyModel> props, StackLayout panel)
        {
            if (panel != null)
            {
                panel.Children.Remove(listView);

                listView = new ListView
                {
                    ItemsSource = props,
                    ItemTemplate = new DataTemplate(typeof(PropertyListingCell)),

                };

                var g = "gf";
                panel.Children.Add(listView);
            }
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

            if (picker.SelectedIndex >= 0)
            {
                suburbSelected = picker.SelectedIndex;
                
            }
            else
            {
                suburbSelected = 0;
            }

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

            if (picker.SelectedIndex >= 0)
            {
                districtSelected = picker.SelectedIndex;            
            }
            else
            {
                districtSelected = 0;
            }
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
            var picker = sender as Picker;

            var regionSelected = picker.SelectedIndex;
            regionId = locations.Regions.ElementAt(regionSelected).Id;
            suburbId = 0;
            districtId = 0;

            SetDistricts();
            var props = tmHelper.GetProperties(regionId, districtId, suburbId);
            AddProperties(props, panel);
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



        private Picker GetLocationPicker()
        {
            var picker = new Picker();

            picker.Items.Add("All");
            picker.Items.Add("Wellington");

            return picker;


        }

    }
}
