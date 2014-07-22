using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Controls;

namespace DesignerTool.Controls.Panorama
{
    public class PanoramaGroupWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double itemBox = double.Parse(values[0].ToString());
            double groupHeight = double.Parse(values[1].ToString());

            double ratio = groupHeight / itemBox;
            ListBox list = (ListBox)values[2];


            double width = Math.Ceiling(list.Items.Count / ratio);
            width *= itemBox;
            if (width < itemBox)
                return itemBox;
            else
                return width;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
