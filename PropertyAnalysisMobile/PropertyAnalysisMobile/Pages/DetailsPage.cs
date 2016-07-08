using PropertyAnalysisMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PropertyAnalysisMobile.Pages
{
    class DetailsPage : ContentPage
    {
        StackLayout panel;

        TradeMeHelper tmHelper;
        public DetailsPage(int propertyId)
        {
            tmHelper = new TradeMeHelper();
            var propDeets = tmHelper.GetDetails(propertyId);

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

            scrollV.Content = panel;
            this.Content = scrollV;
        }

        private Image SetImage(PropertyModel propDeets)
        {
            var image = new Image();

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
