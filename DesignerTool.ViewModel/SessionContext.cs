using DesignerTool.AppLogic.Data;
using DesignerTool.Common.Enums;
using DesignerTool.Common.Logging;
using DesignerTool.Common.Mvvm.ViewModels;
using DesignerTool.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DesignerTool.AppLogic
{
    public abstract class SessionContext : NotifyPropertyChangedBase
    {
        #region Singleton Context Instance

        public SessionContext()
        {
            if (Current == null)
            {
                Current = this;
            }
        }

        public static SessionContext Current { get; protected set; } // Singleton context instance

        #endregion

        private const string CLIENT_CODE_VALUE = "ClientCode";
        private const string REGISTRY_PATH = "HKEY_LOCAL_MACHINE\\Software\\DT";

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

        private int _clientCode = 0;
        public int ClientCode
        {
            get
            {
                if (this._clientCode == 0)
                {
                    using (DesignerToolDbEntities ctx = new DesignerToolDbEntities())
                    {
                        int clientCode;
                        if (Int32.TryParse(ctx.SystemSettings.First(ss => ss.Setting == "ClientCode").Value, out clientCode))
                        {
                            this._clientCode = clientCode;
                        }
                        else
                        {
                            this._clientCode = 0;
                        }
                    }
                }
                return this._clientCode;
            }
        }

        #endregion

        #endregion

        #region Dialogs & Messages

        public abstract UserMessageResults ShowMessage(string message, string caption = null, UserMessageType msgType = UserMessageType.Information, UserMessageButtons buttons = UserMessageButtons.OK);

        #endregion
    }
}
