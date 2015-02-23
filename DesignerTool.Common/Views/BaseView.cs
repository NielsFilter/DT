using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DesignerTool.Common.Mvvm.Commands;

namespace DesignerTool.Common.Mvvm.Views
{
    public class BaseView : ContentControl
    {
        #region Constructors

        public BaseView()
            : base()
        {
            this.Loaded += this.baseView_Loaded;
        }

        #endregion

        #region Dependency Properties

        public Command LoadedCommand
        {
            get { return (Command)GetValue(LoadedCommandProperty); }
            set { SetValue(LoadedCommandProperty, value); }
        }

        public static readonly DependencyProperty LoadedCommandProperty =
            DependencyProperty.Register("LoadedCommand", typeof(Command), typeof(BaseView), new PropertyMetadata(null));

        #endregion

        #region Events

        private void baseView_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= this.baseView_Loaded;

            // Make this the default binding, so we can bind LoadedCommand in any base Viewmodel, which saves us from doing this per view / page.
            Binding binding = new Binding("LoadedCommand");
            binding.Source = this.DataContext;
            this.SetBinding(LoadedCommandProperty, binding);

            if (this.LoadedCommand != null)
            {
                this.LoadedCommand.Execute(null);
            }
        }

        #endregion
    }
}
