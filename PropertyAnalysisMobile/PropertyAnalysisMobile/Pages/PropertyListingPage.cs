﻿using PropertyAnalysisMobile.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PropertyAnalysisMobile.Pages
{
    /// <summary>
    /// Contains a list of propertys pulled from trade me
    /// </summary>
    class PropertyListingPage : ContentPage
    {
        TradeMeHelper tmHelper;
        public PropertyListingPage(int regionId, int districtId, int suburbId)
        {
            Label header = new Label
            {
                Text = "Properties",

                HorizontalOptions = LayoutOptions.Center
            };

            var ic = new ImageCell();

            var cell = new DataTemplate(typeof(ImageCell));
            cell.SetBinding(ImageCell.TextProperty, "Title");
            cell.SetBinding(ImageCell.ImageSourceProperty, "Image");

            var vals = cell.Values;
            
            

            tmHelper = new TradeMeHelper();

            var props = tmHelper.GetProperties(regionId, districtId, suburbId).Select(
                x => new PropertyListItem
                {
                    Id = x.ListingId,
                    Title = x.Title,
                    Image = x.PictureHref,

                });



            var scroll = new ScrollView();
            var propList = new ListView
            {
                ItemsSource = props,
                ItemTemplate = cell,
                RowHeight = 60
            };

            propList.ItemSelected += OnSelection;



            scroll.Content = propList;

            this.Content = new StackLayout
            {
                Children =
                {
                    header,
                    propList,
                }
            };

        }

        void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            var prop = sender as ListView;
            var item = prop.SelectedItem as PropertyListItem;
            Debug.WriteLine(item.Title);
            Navigation.PushAsync(new DetailsPage(item.Id));
        }

        //private async Task<List<PropertyModel>> GetProperties(int regionId, int districtId, int suburbId)
        //{
        //    //var props = tmHelper.GetProperties(regionId, districtId, suburbId);

        //    //var properties = await props;

        //    //return properties;
        //}
    }
}
