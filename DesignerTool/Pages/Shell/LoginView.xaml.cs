using DesignerTool.Common.Mvvm;
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
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        #region ViewModel

        private LoginViewModel ViewModel
        {
            get
            {
                if (this.DataContext == null || !(this.DataContext is LoginViewModel))
                {
                    return null;
                }
                return (LoginViewModel)this.DataContext;
            }
        }

        #endregion

        #region Load
        public LoginView()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= this.Page_Loaded;
            this.ViewModel.Load();
        }

        #endregion

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Login();
        }
    }
}
