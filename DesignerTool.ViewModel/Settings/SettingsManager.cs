using DesignerTool.Common.Global;
using DesignerTool.Common.Logging;
using DesignerTool.Common.Settings;
using DesignerTool.DataAccess.Data;
using DesignerTool.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.AppLogic.Settings
{
    public class SettingsManager
    {
        private static SystemSettingsRepository _rep;
        public static void Initialize(IDesignerToolContext ctx)
        {
            _rep = new SystemSettingsRepository(ctx);

            // Load Database Settings
            Database = new DatabaseSettings();
            Local = new LocalSettings();

            loadDatabaseSettings();

            // Now that the database settings are loaded, hookup the settings changed event which will save a settings when it is changed.
            Database.SettingChanged += updateDatabaseSetting;
        }

        #region Database Settings

        public static DatabaseSettings Database { get; set; }

        private static void loadDatabaseSettings()
        {
            if (Database == null)
            {
                Database = new DatabaseSettings();
            }

            // Load all settings from database
            var settings = _rep.Context.SystemSettings;

            foreach (var setting in settings)
            {
                if (setting.Setting == DatabaseSettings.SettingNames.CLIENTCODE)
                {
                    int clientCode = 0;
                    Int32.TryParse(setting.Value, out clientCode);
                    Database.ClientCode = clientCode;
                }
                else if (setting.Setting == DatabaseSettings.SettingNames.LASTLOGINDATETIME)
                {
                    // We save this DateTime as Ticks. First we read the ticks into a long date type.
                    long lastLoginDateTimeTicks = 0;
                    Int64.TryParse(setting.Value, out lastLoginDateTimeTicks);
                    if (lastLoginDateTimeTicks == 0)
                    {
                        Database.LastLoginDateTime = null;
                    }
                    else
                    {
                        try
                        {
                            // Try cast the ticks back into DateTime
                            Database.LastLoginDateTime = new DateTime(lastLoginDateTimeTicks);
                        }
                        catch (Exception)
                        {
                            // Cannot convert the ticks to a valid DateTime, default to null.
                            Database.LastLoginDateTime = null;
                        }
                    }
                }
                else if (setting.Setting == DatabaseSettings.SettingNames.LICENSEEXPIRYWARNINGDAYS)
                {
                    int warningDays = 30;
                    Int32.TryParse(setting.Value, out warningDays);
                    Database.LicenseExpiryWarningDays = warningDays;
                }
                else
                {
                    // Setting not found int settings file.
                    Logger.Log(String.Format("A setting in database is not implemented in DatabaseSettings file. Setting: '{0}'", setting.Setting));
                }
            }
        }

        /// <summary>
        /// Update a system setting with it's current value.
        /// </summary>
        /// <remarks>
        /// Reason for explicit if...else if... implementation instead of dynamic, is to allow for custom manipulation or logic applied before saving to database.
        /// </remarks>
        /// <param name="settingName">Name of the Database Setting to update.</param>
        private static void updateDatabaseSetting(string settingName)
        {
            string value = null;
            if (settingName == DatabaseSettings.SettingNames.CLIENTCODE)
            {
                value = Database.ClientCode.ToString();
            }
            else if (settingName == DatabaseSettings.SettingNames.LASTLOGINDATETIME)
            {
                if (Database.LastLoginDateTime.HasValue)
                {
                    value = Database.LastLoginDateTime.Value.Ticks.ToString();
                }
            }
            else if (settingName == DatabaseSettings.SettingNames.LICENSEEXPIRYWARNINGDAYS)
            {
                value = Database.LicenseExpiryWarningDays.ToString();
            }
            else
            {
                // Setting not found int settings file.
                throw new ArgumentNullException(String.Format("Could not update database setting. It is not implemented in DatabaseSettings file. Setting: '{0}'", settingName));
            }

            _rep.UpdateValue(settingName, value);
            _rep.ValidateAndCommit();
        }

        #endregion

        #region Local Settings

        public static LocalSettings Local { get; set; }

        #endregion
    }
}
