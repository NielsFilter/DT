using DesignerTool.AppLogic.ViewModels.Admin;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesignerTool.Pages.Admin
{
    /// <summary>
    /// Interaction logic for UserDetail.xaml
    /// </summary>
    public partial class UserDetail : BaseView
    {
        #region ViewModel

        private UserDetailViewModel ViewModel
        {
            get
            {
                if (this.DataContext == null || !(this.DataContext is UserDetailViewModel))
                {
                    return null;
                }
                return (UserDetailViewModel)this.DataContext;
            }
        }

        #endregion

        #region Load

        public UserDetail()
        {
            InitializeComponent();
        }

        public override void PageLoaded()
        {
            this.ViewModel.Load();
        }

        #endregion

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Save();
        }
    }
}
