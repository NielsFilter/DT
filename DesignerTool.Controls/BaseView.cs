using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DesignerTool.Common.Mvvm.Commands;

namespace DesignerTool.Controls
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

        #region Events

        public virtual void PageLoaded()
        {

        }

        private void baseView_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= this.baseView_Loaded;
            this.PageLoaded();
        }

        #endregion
    }
}
