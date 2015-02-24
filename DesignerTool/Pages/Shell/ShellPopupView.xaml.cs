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
using System.Windows.Shapes;

namespace DesignerTool.Pages.Shell
{
    /// <summary>
    /// Interaction logic for ShellPopupView.xaml
    /// </summary>
    public partial class ShellPopupView : Window
    {
        #region ViewModel

        private ShellPopupViewModel ViewModel
        {
            get
            {
                if (this.DataContext == null || !(this.DataContext is ShellPopupViewModel))
                {
                    return null;
                }
                return (ShellPopupViewModel)this.DataContext;
            }
        }

        #endregion

        public ShellPopupView()
        {
            InitializeComponent();
        }

        #region Drag Window

        private MessageBoxResult _result = MessageBoxResult.None;
        private Button _close;
        private FrameworkElement _popup;

        // NF: This code must be in the code behind, as it is UI logic.
        private void ShellPopupView_Loaded(object sender, RoutedEventArgs e)
        {
            this._close = (Button)this.Template.FindName("PART_Close", this);
            if (null != this._close)
            {
                if (false == this._close.IsVisible)
                {
                    this._close.IsCancel = false;
                }
            }

            this._popup = (FrameworkElement)this.Template.FindName("PART_Popup", this);
            if (this._popup != null)
            {
                this._popup.MouseLeftButtonDown += new MouseButtonEventHandler(popup_MouseLeftButtonDown);
            }
        }

        private void popup_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch (Exception) { }
        }

        #endregion
    }
}
