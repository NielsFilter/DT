using DesignerTool.AppLogic.Security;
using DesignerTool.Common.Enums;
using DesignerTool.Common.Global;
using DesignerTool.Common.Licensing;
using DesignerTool.Common.Logging;
using DesignerTool.Common.Utils;
using DesignerTool.DataAccess.Data;
using DesignerTool.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.AppLogic.ViewModels
{
    public class AppViewModel
    {
        private DatabaseManagerRepository repDbMan;
        private SystemSettingsRepository repSettings;

        public AppViewModel(IDesignerToolContext ctx)
        {
            this.repDbMan = new DatabaseManagerRepository(ctx);
            this.repSettings = new SystemSettingsRepository(ctx);

            LicenseManager.Current = new LicenseManager(ctx);
        }

        #region Startup

        public void Start()
        {
            // 1. Set up application paths.
            ApplicationPaths.CreateAppDirectories();

            // 2. Test database connection
            if (!this.repDbMan.TestConnection())
            {
                AppSession.Current.ShowMessage("Could not establish database connection.", "Database connection failed", ResultType.Error);
                return;
            }

            // 3. Setup ClientInfo
            this.validateClientInfo();

            // 4. Evaluate user license.
            LicenseManager.Current.Evaluate();
        }

        private void validateClientInfo()
        {
            try
            {
                string clientCodeSetting = "ClientCode";

                ClientInfo.Code = this.repSettings.GetValue<int>(clientCodeSetting);
                ClientInfo.IsNewInstallation = ClientInfo.Code == 0;
                if (ClientInfo.IsNewInstallation)
                {
                    //This is a new Installation. So we need to create a new license (demo license).
                    // 1. Create and Save a ClientCode for the new user.
                    int code = new Random().Next(100000000, 999999999); // Client code is 9 digits long {000 000 000}
                    var setting = this.repSettings.GetSetting(clientCodeSetting);
                    if (setting == null)
                    {
                        throw new ApplicationException(String.Format("Could not find the system setting. Setting: {0}", clientCodeSetting));
                    }
                    setting.Value = code.ToString();
                    this.repSettings.ValidateAndCommit();
                    ClientInfo.Code = code;

                    // 2. Apply Demo license.
                    LicenseManager.Current.ApplyDemoLicense();
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Invalid Client Code.", ex);
                AppSession.Current.ShowMessage("Database does not have a valid Client Code.", "Invalid Client Code.", ResultType.Error);
            }
        }

        #endregion
    }
}
