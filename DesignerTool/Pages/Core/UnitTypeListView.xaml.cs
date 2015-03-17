using DesignerTool.AppLogic.ViewModels.Core;
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

namespace DesignerTool.Pages.Core
{
    /// <summary>
    /// Interaction logic for UnitTypeListView.xaml
    /// </summary>
    public partial class UnitTypeListView : BaseView
    {
        #region ViewModel

        private UnitTypeListViewModel ViewModel
        {
            get
            {
                if (this.DataContext == null || !(this.DataContext is UnitTypeListViewModel))
                {
                    return null;
                }
                return (UnitTypeListViewModel)this.DataContext;
            }
        }

        #endregion

        #region Load

        public UnitTypeListView()
        {
            InitializeComponent();
        }

        public override void PageLoaded()
        {
 	         this.ViewModel.Load();
        }

        #endregion

        #region Add, Edit, Delete, Refresh

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.AddNew();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Refresh();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Delete();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Edit();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.ViewModel.Refresh();
        }

        private void dgList_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.ViewModel.Edit();
        }

        #endregion
    }
}