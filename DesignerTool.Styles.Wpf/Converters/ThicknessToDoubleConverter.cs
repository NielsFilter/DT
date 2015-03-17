using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DesignerTool.Styles.Wpf.Converters
{
    /// <summary>
    /// Converts Thickness to double value.
    /// </summary>
    [ValueConversion(typeof(Thickness), typeof(double))]
    public class ThicknessToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var thickness = (Thickness)value;
            return thickness.Left;
        }

        /// <summary>
        /// Convert back, but its not implemented as it's not yet needed
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}