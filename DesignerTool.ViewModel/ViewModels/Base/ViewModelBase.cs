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

        #region Properties

        public virtual bool CanGoBack
        {
            get { return false; }
        }

        public virtual string Heading
        {
            get { return string.Empty; }
        }

        #endregion

        #region Load & Refresh

        // Virtual methods
        public virtual void Load() { }
        public virtual void Refresh() { }
        public virtual void GoBack() { }

        #endregion
    }
}
