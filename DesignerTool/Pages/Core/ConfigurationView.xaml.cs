using DesignerTool.AppLogic.ViewModels.Home;
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

namespace DesignerTool.Pages.Core
{
    /// <summary>
    /// Interaction logic for ConfigurationView.xaml
    /// </summary>
    public partial class ConfigurationView : UserControl
    {
        #region ViewModel

        private ConfigurationViewModel ViewModel
        {
            get
            {
                if (this.DataContext == null || !(this.DataContext is ConfigurationViewModel))
                {
                    return null;
                }
                return (ConfigurationViewModel)this.DataContext;
            }
        }

        #endregion

        #region Load

        public ConfigurationView()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= this.Page_Loaded;
            this.ViewModel.Load();
        }

        #endregion

    }
}
