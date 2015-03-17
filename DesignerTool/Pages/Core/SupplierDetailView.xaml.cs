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
    /// Interaction logic for SupplierDetailView.xaml
    /// </summary>
    public partial class SupplierDetailView : BaseView
    {
        #region ViewModel

        private SupplierDetailViewModel ViewModel
        {
            get
            {
                if (this.DataContext == null || !(this.DataContext is SupplierDetailViewModel))
                {
                    return null;
                }
                return (SupplierDetailViewModel)this.DataContext;
            }
        }

        #endregion

        #region Load

        public SupplierDetailView()
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
