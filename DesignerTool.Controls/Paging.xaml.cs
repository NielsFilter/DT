using DesignerTool.Common.Mvvm.Paging;
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

namespace DesignerTool.Controls
{
    /// <summary>
    /// Interaction logic for Paging.xaml
    /// </summary>
    public partial class Paging : UserControl
    {
        public Paging()
        {
            InitializeComponent();
        }

        #region Dependency Properties

        #region Pager

        public PagingViewModel Pager
        {
            get { return (PagingViewModel)GetValue(PageProperty); }
            set { SetValue(PageProperty, value); }
        }

        public static readonly DependencyProperty PageProperty =
            DependencyProperty.Register("Pager", typeof(PagingViewModel), typeof(Paging), new PropertyMetadata(null));

        #endregion

        #endregion
    }
}
