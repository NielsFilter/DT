using DesignerTool.AppLogic.ViewModels.Core;
using DesignerTool.Common.Mvvm;
using DesignerTool.Controls;
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

namespace DesignerTool.Pages.Core
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : BaseView
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

        public override void PageLoaded()
        {
            this.ViewModel.Load();
        }

        #endregion

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Login();
        }
    }
}
