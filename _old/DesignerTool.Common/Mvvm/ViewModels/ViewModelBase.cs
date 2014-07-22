using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using DesignerTool.Common.Mvvm.Interfaces;
using DesignerTool.Common.Mvvm.Mapping;
using DesignerTool.Common.Mvvm.Services;

namespace DesignerTool.Common.Mvvm.ViewModels
{
    /// <summary>
    /// This is the base ViewModel which holds core ViewModel functionality, not specific to a model or a facade.
    /// You will generally use this for the shell ViewModel, and any other viewModels that do not need to communicate with facades.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged, IViewModel
    {
        #region Constructors

        public ViewModelBase()
            : this(false)
        {
        }

        public ViewModelBase(bool isPopup)
        {
            this.IsPopup = isPopup;
            this.OnWireCommands();
        }

        #endregion

        #region Properties

        public bool IsPopup { get; set; }

        #endregion

        #region Public Methods

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// This method is virtual (Overridable in VB.NET) so that ViewModels implementing this class can simply override this method and initialize 
        /// all the relevant commands they need to use. Reason why overidable is used instead of just allowing initialization to happen anywhere,
        /// is that it is now forced to happen at the same time \ right time as well as base commands can be done here and view models can access them
        /// without having to create them every time.
        /// </summary>
        public virtual void OnWireCommands()
        {
        }

        public void RefreshViewModel()
        {
            this.OnRefresh();
        }

        public virtual void OnRefresh()
        {
        }
        
        public virtual bool CanNavigate()
        {
            return true;
        }

        #endregion
    }
}
