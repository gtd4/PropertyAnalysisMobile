using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PropertyAnalysisMobile.Helpers
{
    public class PickerHelper
    {

        /// <summary>
        /// initialises a new picker and sets the title of it
        /// ToDo: Work out how we can remove more duplication of code in the init pickers method above
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Picker InitPicker(string title, int selectedIndex)
        {
            var picker = new Picker();
            picker.Title = title;
            picker.SelectedIndex = selectedIndex;

            return picker;
        }

        public int SetSelected(Picker picker)
        {

            if (picker.SelectedIndex >= 0)
            {
                return picker.SelectedIndex;

            }

            return 0;
            
        }
    }
}
