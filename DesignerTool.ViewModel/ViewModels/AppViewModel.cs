using DesignerTool.AppLogic.Security;
using DesignerTool.AppLogic.Settings;
using DesignerTool.Common.Enums;
using DesignerTool.Common.Global;
using DesignerTool.Common.Licensing;
using DesignerTool.Common.Logging;
using DesignerTool.Common.Utils;
using DesignerTool.DataAccess.Connection;
using DesignerTool.DataAccess.Data;
using DesignerTool.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;

namespace DesignerTool.AppLogic.ViewModels
{
    public class AppViewModel
    {
        private IDesignerToolContext _ctx;
        private DatabaseManagerRepository repDbMan;
        private SystemSettingsRepository repSettings;

        public AppViewModel(IDesignerToolContext ctx)
        {
            this._ctx = ctx;
            this.repDbMan = new DatabaseManagerRepository(ctx);
            this.repSettings = new SystemSettingsRepository(ctx);

            LicenseManager.Current = new LicenseManager(ctx);
        }

        #region Startup

        public void Start()
        {
            // 1. Test database connection
            if (!this.repDbMan.TestConnection())
            {
                AppSession.Current.ShowMessage("Could not establish database connection.", "Database connection failed", ResultType.Error);
                return;
            }

            // 2. Load Settings
            SettingsManager.Initialize(this._ctx);

            // 4. Setup ClientInfo
            this.validateClientInfo();

            // 5. Evaluate user license.
            LicenseManager.Current.Evaluate();
        }

        private void validateClientInfo()
        {
            try
            {
                AppSession.Current.IsNewInstallation = SettingsManager.Database.ClientCode == 0;
                if (AppSession.Current.IsNewInstallation)
                {
                    //This is a new Installation. So we need to create a new license (demo license).
                    // 1. Generate a new ClientCode. Client code is 9 digits long {000 000 000}.
                    SettingsManager.Database.ClientCode = new Random().Next(100000000, 999999999);
                   
                    // 2. Apply Demo license.
                    LicenseManager.Current.ApplyDemoLicense(SettingsManager.Database.ClientCode);
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
