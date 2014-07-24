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

        public static User LoggedInUser { get; set; }

        public static DateTime? LicenseExpiry { get; set; }

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
                        _clientCode = cc.ToString();
                    }
                }
                return _clientCode;
            }
        }

        #endregion
    }
}
