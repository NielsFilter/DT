using DesignerTool.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesignerTool.Controls
{
    /// <summary>
    /// Interaction logic for LicenseIndicator.xaml
    /// </summary>
    public partial class LicenseIndicator : UserControl
    {
        // License styles
        private const string VALID_BRUSH = "SuccessBrush";
        private const string EXPIRESSOON_BRUSH = "WarningBrush";
        private const string EXPIRED_BRUSH = "ErrorBrush";
        private const string DEMO_BRUSH = "InformationBrush";

        #region Ctors

        public LicenseIndicator()
        {
            InitializeComponent();
        }

        #endregion

        #region Dependency Properties

        #region LicenseDisplay

        public string LicenseDisplay
        {
            get { return (string)GetValue(LicenseDisplayProperty); }
            set { SetValue(LicenseDisplayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LicenseDisplay.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LicenseDisplayProperty =
            DependencyProperty.Register("LicenseDisplay", typeof(string), typeof(LicenseIndicator), new PropertyMetadata(licenseDisplay_Changed));

        private static void licenseDisplay_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LicenseIndicator currentCtrl = d as LicenseIndicator;
            if (currentCtrl == null)
            {
                return;
            }

            string display = string.Empty;
            if (e.NewValue != null)
            {
                display = e.NewValue.ToString();
            }

            currentCtrl.tbLicenseDisplay.Text = display;
        }

        #endregion

        #region LicenseState

        public LicenseStateTypes LicenseState
        {
            get { return (LicenseStateTypes)GetValue(LicenseStateProperty); }
            set { SetValue(LicenseStateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LicenseState.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LicenseStateProperty =
            DependencyProperty.Register("LicenseState", typeof(LicenseStateTypes), typeof(LicenseIndicator), new PropertyMetadata(licenseState_Changed));

        private static void licenseState_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LicenseIndicator currentCtrl = d as LicenseIndicator;
            if (currentCtrl == null)
            {
                return;
            }

            LicenseStateTypes state = LicenseStateTypes.Demo;
            if(e.NewValue is LicenseStateTypes)
            {
                state = (LicenseStateTypes)e.NewValue;
            }

            string styleName = DEMO_BRUSH;
            switch (state)
            {
                case LicenseStateTypes.Valid:
                    styleName = VALID_BRUSH;
                    break;
                case LicenseStateTypes.Demo:
                    styleName = DEMO_BRUSH;
                    break;
                case LicenseStateTypes.ExpiresSoon:
                    styleName = EXPIRESSOON_BRUSH;
                    break;
                case LicenseStateTypes.Expired:
                    styleName = EXPIRED_BRUSH;
                    break;
            }

            // Set Background according to license state
            Brush background = Application.Current.FindResource(styleName) as Brush;
            if (background != null)
            {
                currentCtrl.brdLicense.Background = background;
            }
        }

        #endregion

        #region ClientCode

        public int ClientCode
        {
            get { return (int)GetValue(ClientCodeProperty); }
            set { SetValue(ClientCodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ClientCode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClientCodeProperty =
            DependencyProperty.Register("ClientCode", typeof(int), typeof(LicenseIndicator), new PropertyMetadata(clientCode_Changed));

        private static void clientCode_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LicenseIndicator currentCtrl = d as LicenseIndicator;
            if (currentCtrl == null)
            {
                return;
            }

            int display = 0;
            if (e.NewValue != null)
            {
                Int32.TryParse(e.NewValue.ToString(), out display);
            }

            currentCtrl.txtClientCode.Text = display.ToString("### ### ###");
        }



        #endregion

        #endregion
    }
}
