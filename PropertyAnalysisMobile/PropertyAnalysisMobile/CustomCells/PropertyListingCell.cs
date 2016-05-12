using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PropertyAnalysisMobile.CustomCells
{
    public class PropertyListingCell : ViewCell
    {
        //Image View for Property Image
        //Label for Property Title

        public PropertyListingCell()
        {
            //instantiate each of our views
            //var image = new Image();
            StackLayout cellWrapper = new StackLayout();
            StackLayout horizontalLayout = new StackLayout();
            Label title = new Label();


            //set bindings
            title.SetBinding(Label.TextProperty, "Title");
            
            //image.SetBinding(Image.SourceProperty, "PictureHref");

            

            //Set properties for desired design

            

            //add views to the view hierarchy
            //horizontalLayout.Children.Add(image);
            horizontalLayout.Children.Add(title);

            cellWrapper.Children.Add(horizontalLayout);
            View = cellWrapper;
        }
    }
}
