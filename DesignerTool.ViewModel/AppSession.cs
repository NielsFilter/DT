using DesignerTool.Common.Enums;
using DesignerTool.Common.Logging;
using DesignerTool.Common.Mvvm.ViewModels;
using DesignerTool.Common.ViewModels;
using DesignerTool.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DesignerTool.AppLogic
{
    public abstract class AppSession : NotifyPropertyChangedBase
    {
        #region Singleton Context Instance

        public AppSession()
        {
            if (Current == null)
            {
                Current = this;
            }
        }

        public static AppSession Current { get; protected set; } // Singleton context instance

        #endregion

        #region Properties

        #region Navigate

        public event Action<ViewModelBase> ViewModelNavigated;

        private ViewModelBase _currentViewModel;
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
                    base.NotifyPropertyChanged("CanGoBack");
                }
            }
        }

        public virtual void Navigate(ViewModelBase viewModel)
        {
            if(this.ViewModelNavigated != null)
            {
                this.ViewModelNavigated(viewModel);
            }
        }

        #endregion

        public IParentViewModel ParentViewModel { get; set; }

        #region User

        private User _loggedInUser;
        public User LoggedInUser
        {
            get
            {
                return this._loggedInUser;
            }
            set
            {
                if (value != this._loggedInUser)
                {
                    this._loggedInUser = value;
                    base.NotifyPropertyChanged("LoggedInUser");
                }
            }
        }

        #endregion

        #region License and Activation

        public DateTime? LicenseExpiry { get; set; }

        public bool IsValidLicense
        {
            get
            {
                if (!this.LicenseExpiry.HasValue)
                {
                    return false;
                }

                return this.LicenseExpiry.Value >= DateTime.Today;
            }
        }

        #endregion

        #endregion

        #region Dialogs & Messages

        public abstract UserMessageResults ShowMessage(string message, string caption = null, ResultType msgType = ResultType.Information, UserMessageButtons buttons = UserMessageButtons.OK);

        #endregion

        #region Database Context

        public virtual IDesignerToolContext CreateContext()
        {
            return new DesignerToolDbEntities();
        }

        #endregion
    }
}
