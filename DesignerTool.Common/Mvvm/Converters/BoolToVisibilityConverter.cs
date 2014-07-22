using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace DesignerTool.Common.Mvvm.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts Boolean to Visibility.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false; // Default is false
            if (value != null)
            {
                bool.TryParse(value.ToString(), out result);
            }

            bool invert = false;
            if (parameter != null)
            {
                bool.TryParse(parameter.ToString(), out invert);
            }

            if (invert)
            {
                // Inverted - True = "Collapsed" and False = "Visible"
                return result ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                // Normal (Not inverted) - True = "Visible" and False = "Collapsed"
                return result ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Convert back, but its not implemented as it's not yet needed
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool invert = false;
            if (parameter != null)
            {
                bool.TryParse(parameter.ToString(), out invert);
            }

            if (invert)
            {
                // Inverted - True = "Collapsed" and False = "Visible"
                return Visibility.Collapsed;
            }
            else
            {
                // Normal (Not inverted) - True = "Visible" and False = "Collapsed"
                return Visibility.Visible;
            }
        }
    }

}
