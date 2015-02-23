using DesignerTool.Common.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace DesignerTool.Common.Mvvm.ViewModels
{
    /// <summary>
    /// This is the base ViewModel which holds core ViewModel functionality, not specific to a model or a facade.
    /// You will generally use this for the shell ViewModel, and any other viewModels that do not need to communicate with facades.
    /// </summary>
    public class ViewModelBase : NotifyPropertyChangedBase
    {
        #region ctors

        public ViewModelBase()
        {

        }

        #endregion

        #region Load & Refresh

        public void Load()
        {
            this.OnLoad();
        }

        public void Refresh()
        {
            this.OnRefresh();
        }

        // Virtual methods
        public virtual void OnLoad() { }
        public virtual void OnRefresh() { }

        #endregion
    }
}
