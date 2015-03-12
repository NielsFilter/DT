using DesignerTool.Common.Mvvm.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for UserActivation.xaml
    /// </summary>
    public partial class UserActivation : BaseView
    {
        #region ViewModel

        private UserActivationViewModel ViewModel
        {
            get
            {
                if (this.DataContext == null || !(this.DataContext is UserActivationViewModel))
                {
                    return null;
                }
                return (UserActivationViewModel)this.DataContext;
            }
        }

        #endregion

        #region Load

        public UserActivation()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= this.Page_Loaded;
            this.ViewModel.Load();
        }

        #endregion

        private void Activate_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.ActivateLicense();
        }
    }
}
