using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.Common.Settings
{
    public class DatabaseSettings
    {
        #region Setting Names

        public static class SettingNames
        {
            public const string CLIENTCODE = "ClientCode";
            public const string LASTLOGINDATETIME = "LastLoginDateTime";
            public const string LICENSEEXPIRYWARNINGDAYS = "LicenseExpiryWarningDays";
        }

        #endregion

        #region Ctors

        public DatabaseSettings()
        {
        }

        #endregion

        #region Events

        public delegate void SettingChangedHandler(string settingName);
        public event SettingChangedHandler SettingChanged;

        private void settingChanged(string settingName)
        {
            // Check if anyone cares about the setting being changed.
            if(this.SettingChanged != null)
            {
                // Notify event listeners telling about the change.
                this.SettingChanged(settingName);
            }
        }

        #endregion

        #region Settings

        private int _clientCode;
        public int ClientCode
        {
            get { return this._clientCode; }
            set
            {
                if (this._clientCode != value)
                {
                    this._clientCode = value;
                    this.settingChanged(SettingNames.CLIENTCODE);
                }
            }
        }

        private DateTime? _lastLoginDateTime;
        public DateTime? LastLoginDateTime
        {
            get { return this._lastLoginDateTime; }
            set
            {
                if (this._lastLoginDateTime != value)
                {
                    this._lastLoginDateTime = value;
                    this.settingChanged(SettingNames.LASTLOGINDATETIME);
                }
            }
        }

        private int _licenseExpiryWarningDays;
        public int LicenseExpiryWarningDays
        {
            get
            {
                return this._licenseExpiryWarningDays;
            }
            set
            {
                if (value != this._licenseExpiryWarningDays)
                {
                    this._licenseExpiryWarningDays = value;
                    this.settingChanged(SettingNames.LICENSEEXPIRYWARNINGDAYS);
                }
            }
        }

        #endregion
    }
}
