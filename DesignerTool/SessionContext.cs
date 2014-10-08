using DesignerTool.Common.Base;
using DesignerTool.Common.Logging;
using DesignerTool.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DesignerTool
{
    public class SessionContext : NotifyPropertyChangedBase
    {
        private static SessionContext _current;
        public static SessionContext Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new SessionContext();
                }
                return _current;
            }
        }

        private const string CLIENT_CODE_VALUE = "ClientCode";
        private const string REGISTRY_PATH = "HKEY_LOCAL_MACHINE\\Software\\DT";

        #region Properties

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

        private string _clientCode = null;
        public string ClientCode
        {
            get
            {
                if (String.IsNullOrEmpty(this._clientCode))
                {
                    var cc = Microsoft.Win32.Registry.GetValue(REGISTRY_PATH, CLIENT_CODE_VALUE, null);
                    if (cc != null)
                    {
                        var clientCode = cc.ToString();
                        if (!clientCode.StartsWith("CL"))
                        {
                            //TODO: Logging - Invalid client code.
                            this._clientCode = null;
                        }
                        this._clientCode = clientCode;
                    }
                }
                return this._clientCode;
            }
        }

        #endregion

        #endregion
    }
}
