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

namespace DesignerTool.Pages.Shell
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        #region VM

        private HomeViewModel VM
        {
            get
            {
                if (this.DataContext == null || !(this.DataContext is HomeViewModel))
                {
                    return null;
                }
                return (HomeViewModel)this.DataContext;
            }
        }

        #endregion

        #region Load

        public HomeView()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= this.Page_Loaded;
        }

        #endregion
    }
}
