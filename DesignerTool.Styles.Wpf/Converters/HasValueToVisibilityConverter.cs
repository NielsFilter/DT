using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace DesignerTool.Styles.Wpf.Converters
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class HasValueToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts object's value to Visibility.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool invert = false;
            if (parameter != null)
            {
                bool.TryParse(parameter.ToString(), out invert);
            }

            if (invert)
            {
                // Inverted - No value = "Visible" and Has Value = "Collapsed"
                return value == null ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                // Normal (Not inverted) - No value = "Collapsed" and Has Value = "Visible"
                return value == null ? Visibility.Collapsed : Visibility.Visible;
            }
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
