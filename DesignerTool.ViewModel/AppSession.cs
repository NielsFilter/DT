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

        public abstract void Navigate(ViewModelBase viewModel);

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

        public abstract UserMessageResults ShowMessage(string message, string caption = null, UserMessageType msgType = UserMessageType.Information, UserMessageButtons buttons = UserMessageButtons.OK);

        #endregion

        #region Database Context

        public virtual IDesignerToolContext CreateContext()
        {
            return new DesignerToolDbEntities();
        }

        #endregion
    }
}
