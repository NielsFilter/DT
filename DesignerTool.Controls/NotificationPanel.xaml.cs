using System;
using System.Collections.Generic;
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
using DesignerTool.Common.Enums;

namespace DesignerTool.Controls
{
    /// <summary>
    /// Interaction logic for NotificationPanel.xaml
    /// </summary>
    public partial class NotificationPanel : UserControl
    {
        // Notification styles
        private const string SUCCESS_STYLE = "BrdSuccessNotification";
        private const string WARNING_STYLE = "BrdWarningNotification";
        private const string ERROR_STYLE = "BrdErrorNotification";
        private const string INFORMATION_STYLE = "BrdInformationNotification";

        // Vector styles
        private const string SUCCESS_VECTOR = "vcTick";
        private const string WARNING_VECTOR = "vcWarning";
        private const string ERROR_VECTOR = "vcWarning";
        private const string INFORMATION_VECTOR = "vcInfo";


        public NotificationPanel()
        {
            InitializeComponent();
        }

        #region Dependency Properties

        #region NotificationType

        public ResultType NotificationType
        {
            get { return (ResultType)GetValue(NotificationTypeProperty); }
            set { SetValue(NotificationTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NotificationType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotificationTypeProperty =
            DependencyProperty.Register("NotificationType", typeof(ResultType), typeof(NotificationPanel), new PropertyMetadata(ResultType.Information, notificationTypeChanged));

        private static void notificationTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NotificationPanel currentPanel = d as NotificationPanel;
            if (currentPanel == null)
            {
                return;
            }

            ResultType notificationType = (ResultType)e.NewValue;

            string styleName = INFORMATION_STYLE;
            string vectorName = INFORMATION_VECTOR;
            switch (notificationType)
            {
                case ResultType.Success:
                    styleName = SUCCESS_STYLE;
                    vectorName = SUCCESS_VECTOR;
                    break;
                case ResultType.Warning:
                    styleName = WARNING_STYLE;
                    vectorName = WARNING_VECTOR;
                    break;
                case ResultType.Error:
                    styleName = ERROR_STYLE;
                    vectorName = ERROR_VECTOR;
                    break;
            }

            // Notification Style
            Style notificationStyle = Application.Current.FindResource(styleName) as Style;
            if (notificationStyle != null)
            {
                currentPanel.brdPanel.Style = notificationStyle;
            }

            // Vector Style
            Style vectorStyle = Application.Current.FindResource(vectorName) as Style;
            if (vectorStyle != null)
            {
                currentPanel.vcIcon.Style = vectorStyle;
            }
        }

        #endregion

        #region MainText

        public string MainText
        {
            get { return (string)GetValue(MainTextProperty); }
            set { SetValue(MainTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MainText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MainTextProperty =
            DependencyProperty.Register("MainText", typeof(string), typeof(NotificationPanel), new PropertyMetadata(mainTextChanged));

        private static void mainTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NotificationPanel currentPanel = d as NotificationPanel;
            if (currentPanel == null)
            {
                return;
            }

            currentPanel.txbMain.Text = e.NewValue as string;
        }

        #endregion

        #region ExtraInfo

        public string ExtraInfo
        {
            get { return (string)GetValue(ExtraInfoProperty); }
            set { SetValue(ExtraInfoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ExtraInfo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExtraInfoProperty =
            DependencyProperty.Register("ExtraInfo", typeof(string), typeof(NotificationPanel), new PropertyMetadata(extraInfoChanged));

        private static void extraInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NotificationPanel currentPanel = d as NotificationPanel;
            if (currentPanel == null)
            {
                return;
            }

            string newValue = e.NewValue as string;
            currentPanel.txbExtra.Visibility = !string.IsNullOrWhiteSpace(newValue) ? Visibility.Visible : Visibility.Collapsed;
            currentPanel.txbExtra.Text = newValue;
        }

        #endregion

        #region IsExtraInfoShow

        public bool IsExtraInfoShow
        {
            get { return (bool)GetValue(IsExtraInfoShowProperty); }
            set { SetValue(IsExtraInfoShowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsExtraInfoShow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsExtraInfoShowProperty =
            DependencyProperty.Register("IsExtraInfoShow", typeof(bool), typeof(NotificationPanel), new PropertyMetadata(isExtraInfoShowChanged));

        private static void isExtraInfoShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NotificationPanel currentPanel = d as NotificationPanel;
            if (currentPanel == null)
            {
                return;
            }

            currentPanel.txbExtra.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion

        #region IsPanelShow

        public bool IsPanelShow
        {
            get { return (bool)GetValue(IsPanelShowProperty); }
            set { SetValue(IsPanelShowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPanelShow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPanelShowProperty =
            DependencyProperty.Register("IsPanelShow", typeof(bool), typeof(NotificationPanel), new PropertyMetadata(isPanelShowChanged));

        private static void isPanelShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NotificationPanel currentPanel = d as NotificationPanel;
            if (currentPanel == null)
            {
                return;
            }

            currentPanel.brdPanel.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion

        #endregion

        #region Events

        public event RoutedEventHandler CloseClicked;

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            // Close this panel
            this.IsPanelShow = false;

            // Allow consumer to hook into event as well.
            if (this.CloseClicked != null)
            {
                this.CloseClicked(sender, e);
            }
        }

        private void ShowHideExtra_Click(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if(element != null)
            {
                if(this.IsExtraInfoShow)
                {
                    this.IsExtraInfoShow = false;
                }
                else if(!string.IsNullOrWhiteSpace(this.ExtraInfo))
                {
                    this.IsExtraInfoShow = true;
                }
            }
        }

        #endregion
    }
}
