using DesignerTool.AppLogic.ViewModels.Tools;
using DesignerTool.Common.Mvvm.Views;
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

namespace DesignerTool.Pages.Tools
{
    /// <summary>
    /// Interaction logic for ActivationKeyGenerator.xaml
    /// </summary>
    public partial class ActivationKeyGenerator : BaseView
    {
        #region ViewModel

        private ActivationKeyGeneratorViewModel ViewModel
        {
            get
            {
                if (this.DataContext == null || !(this.DataContext is ActivationKeyGeneratorViewModel))
                {
                    return null;
                }
                return (ActivationKeyGeneratorViewModel)this.DataContext;
            }
        }

        #endregion

        #region Load

        public ActivationKeyGenerator()
        {
            InitializeComponent();
        }

        public override void PageLoaded()
        {
            this.ViewModel.Load();
        }

        #endregion

        private void GenerateCode_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.GenerateCode();
        }
    }
}
