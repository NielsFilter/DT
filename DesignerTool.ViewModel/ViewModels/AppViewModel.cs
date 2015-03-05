using DesignerTool.AppLogic.Security;
using DesignerTool.Common.Enums;
using DesignerTool.Common.Global;
using DesignerTool.Common.Logging;
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
        }

        #region Startup

        public void Start()
        {
            // 1. Set up application paths.
            ApplicationPaths.CreateAppDirectories();

            // 2. Test database connection
            if (!this.repDbMan.TestConnection())
            {
                AppSession.Current.ShowMessage("Could not establish database connection.", "Database connection failed", UserMessageType.Error);
                return;
            }

            // 3. Setup ClientInfo
            if (!this.getClientInfo())
            {
                return;
            }

            // 4. Evaluate user license.
            LicenseManager.Evaluate();
        }

        private bool getClientInfo()
        {
            try
            {
                ClientInfo.Code = this.repSettings.GetValue<int>("ClientCode");
                return ClientInfo.Code > 0;
            }
            catch (Exception ex)
            {
                Logger.Log("Invalid Client Code.", ex);
                AppSession.Current.ShowMessage("Database does not have a valid Client Code.", "Invalid Client Code.", UserMessageType.Error);
                return false;
            }
        }

        #endregion
    }
}
