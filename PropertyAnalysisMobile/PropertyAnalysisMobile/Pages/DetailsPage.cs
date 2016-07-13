using PropertyAnalysisMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace PropertyAnalysisMobile.Pages
{
    class DetailsPage : ContentPage
    {
        StackLayout panel;
        double propLat, propLong;
        int propNorthing, propEasting;
        PropertyMap PropertyMap;

        TradeMeHelper tmHelper;
        public DetailsPage(int propertyId)
        {
            tmHelper = new TradeMeHelper();
            var propDeets = tmHelper.GetDetails(propertyId);
            var propLocation = propDeets.GeographicLocation;
            

            SetGeoLocation(propLocation);
            SetMap(propDeets.Title, propDeets.FullAddress);

            var scrollV = new ScrollView();

            panel = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Spacing = 20,
                Padding = 15,
            };

            var image = SetImage(propDeets);

            panel.Children.Add(image);

            panel.Children.Add(new Label
            {
                Text = propDeets.Title
            });

            panel.Children.Add(new Label
            {
                Text = propDeets.ListedDate
            });

            panel.Children.Add(new Label
            {
                Text = propDeets.FullAddress
            });

            if (!string.IsNullOrEmpty(propDeets.Bedrooms) || !string.IsNullOrEmpty(propDeets.Bathrooms))
            {
                panel.Children.Add(new Label
                {
                    Text = string.Format("Room: {0} : {1}", propDeets.Bathrooms, propDeets.Bedrooms)
                });
            }

            panel.Children.Add(new Label
            {
                Text = propDeets.Description
            });

            var searchBtn = new Button();
            searchBtn.Text = "Map";
            searchBtn.Clicked += ButtonClicked;

            panel.Children.Add(searchBtn);

            scrollV.Content = panel;
            this.Content = scrollV;
        }

        private void SetMap(string title, string address)
        {
            PropertyMap = new PropertyMap(MapSpan.FromCenterAndRadius(new Position(propLat, propLong), Distance.FromKilometers(1)))
            {

            };

            var propPin = new Pin
            {
                Type = PinType.Generic,
                Position = new Position(propLat, propLong),
                Label = title,
                Address = address
            };

            PropertyMap.Pins.Add(propPin);
        }

        private void SetGeoLocation(GeographicLocation propLocation)
        {
            propLat = propLocation.Latitude;
            propLong = propLocation.Longitude;
            propNorthing = propLocation.Northing;
            propEasting = propLocation.Easting;
        }

        void ButtonClicked(object sender, EventArgs ea)
        {
            Navigation.PushAsync(new MapPage(PropertyMap));
        }

        private Image SetImage(PropertyModel propDeets)
        {
            var image = new Image
            {
                Aspect = Aspect.Fill,
            };
            
            if (propDeets.Photos != null && propDeets.Photos.Any())
            {
                image.Source = ImageSource.FromUri(new Uri(propDeets.Photos.First().PhotoUrl.Large));
            }
            else
            {
                image.Source = ImageSource.FromUri(new Uri("http://placehold.it/350x150"));
            }

            return image;
        }

    }
}
