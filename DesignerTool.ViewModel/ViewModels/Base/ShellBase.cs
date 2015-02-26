using DesignerTool.Common.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DesignerTool.Common.Mvvm.ViewModels;
using DesignerTool.Common.Enums;
using DesignerTool.Pages.Shell;
using DesignerTool.Common.ViewModels;

namespace DesignerTool.ViewModels
{
    public class ShellBase : ViewModelBase, IParentViewModel
    {
        #region Constructors

        public ShellBase()
            : base()
        {
        }

        #endregion

        #region Properties

        private ViewModelBase _currentViewModel;
        /// <summary>
        /// The Current View is bound to the frame. So changing by changing this property you can "navigate" between pages.
        /// </summary>
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                if (value != this._currentViewModel)
                {
                    this._currentViewModel = value;
                    base.NotifyPropertyChanged("CurrentViewModel");
                }
            }
        }

        private ViewModelBase _previousViewModel;
        /// <summary>
        /// The Previous ViewModel that was bound to the "Main frame". This is needed for navigating back and forth.
        /// </summary>
        public ViewModelBase PreviousViewModel
        {
            get
            {
                return this._previousViewModel;
            }
            set
            {
                if (value != this._previousViewModel)
                {
                    this._previousViewModel = value;
                    base.NotifyPropertyChanged("PreviousViewModel");
                }
            }
        }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get
            {
                return this._isLoading;
            }
            set
            {
                this._isLoading = value;
                base.NotifyPropertyChanged("IsLoading");
            }
        }

        private string _loadingMessage;
        public string LoadingMessage
        {
            get
            {
                return this._loadingMessage;
            }
            set
            {
                if (value != this._loadingMessage)
                {
                    this._loadingMessage = value;
                    base.NotifyPropertyChanged("LoadingMessage");
                }
            }
        }

        #endregion

        #region Public Methods

        public void GoBack()
        {
            if (this.PreviousViewModel != null)
            {
                this.CurrentViewModel = this.PreviousViewModel;
                this.PreviousViewModel = null;

                base.Refresh();
            }
        }

        public bool CanGoBack()
        {
            return this.PreviousViewModel != null && this.CurrentViewModel != this.PreviousViewModel;
        }

        #endregion
    }
}
