﻿using DesignerTool.AppLogic.ViewModels.Core;
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
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : BaseView
    {
        #region ViewModel

        private HomeViewModel ViewModel
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
        
        public override void PageLoaded()
        {
            this.ViewModel.Load();
        }

        #endregion
    }
}
