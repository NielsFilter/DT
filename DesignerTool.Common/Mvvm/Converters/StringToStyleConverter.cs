using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace DesignerTool.Common.Mvvm.Converters
{
    [ValueConversion(typeof(string), typeof(Style))]
    public class StringToStyleConverter : IValueConverter
    {
        /// <summary>
        /// Converts Boolean to Visibility.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.ToString().Trim() == null)
            {
                return null;
            }

            // Find the style in the application's resources
            var style = Application.Current.TryFindResource(value.ToString());
            if (style != null && style is Style)
            {
                return (Style)style;
            }

            return null;
        }

        /// <summary>
        /// Convert back, but its not implemented as it's not yet needed
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
