using DesignerTool.Common.Logging;
using DesignerTool.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool
{
    public static class SessionContext
    {
        private const string CLIENT_CODE_VALUE = "ClientCode";
        private const string REGISTRY_PATH = "HKEY_LOCAL_MACHINE\\Software\\DT";

        #region Properties

        #region User

        public static User LoggedInUser { get; set; }

        #endregion

        #region License and Activation

        public static DateTime? LicenseExpiry { get; set; }

        public static bool IsValidLicense
        {
            get
            {
                if (!SessionContext.LicenseExpiry.HasValue)
                {
                    return false;
                }

                return SessionContext.LicenseExpiry.Value >= DateTime.Today;
            }
        }

        private static string _clientCode = null;
        public static string ClientCode
        {
            get
            {
                if (String.IsNullOrEmpty(_clientCode))
                {
                    var cc = Microsoft.Win32.Registry.GetValue(REGISTRY_PATH, CLIENT_CODE_VALUE, null);
                    if (cc != null)
                    {
                        var clientCode = cc.ToString();
                        if (!clientCode.StartsWith("CL"))
                        {
                            //TODO: Logging - Invalid client code.
                            _clientCode = null;
                        }
                        _clientCode = clientCode;
                    }
                }
                return _clientCode;
            }
        }

        #endregion

        #endregion
    }
}
