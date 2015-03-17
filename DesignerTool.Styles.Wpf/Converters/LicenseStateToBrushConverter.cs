using DesignerTool.Common.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace DesignerTool.Styles.Wpf.Converters
{
    [ValueConversion(typeof(string), typeof(Brush))]
    public class LicenseStateToBrushConverter : IValueConverter
    {
        // License styles
        private const string VALID_BRUSH = "SuccessBrush";
        private const string EXPIRESSOON_BRUSH = "WarningBrush";
        private const string EXPIRED_BRUSH = "ErrorBrush";
        private const string DEMO_BRUSH = "InformationBrush";

        /// <summary>
        /// Converts Boolean to Visibility.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string brushStyle = DEMO_BRUSH; // Default
            if (value != null)
            {
                LicenseStateTypes stateType;
                if (Enum.TryParse<LicenseStateTypes>(value.ToString(), out stateType))
                {
                    switch (stateType)
                    {
                        case LicenseStateTypes.Valid:
                            brushStyle = VALID_BRUSH;
                            break;
                        case LicenseStateTypes.Demo:
                            brushStyle = DEMO_BRUSH;
                            break;
                        case LicenseStateTypes.ExpiresSoon:
                            brushStyle = EXPIRESSOON_BRUSH;
                            break;
                        case LicenseStateTypes.Expired:
                            brushStyle = EXPIRED_BRUSH;
                            break;
                    }
                }


            }

            // Set brush according to license state
            Brush brush = Application.Current.FindResource(brushStyle) as Brush;
            if (brush != null)
            {
                return brush;
            }
            return null;
        }

        /// <summary>
        /// Convert back, but its not implemented as it's not yet needed
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
