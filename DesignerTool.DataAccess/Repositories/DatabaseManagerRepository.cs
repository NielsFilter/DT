using DesignerTool.Common.Logging;
using DesignerTool.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.DataAccess.Repositories
{
    public class DatabaseManagerRepository : BaseRepository
    {
        public DatabaseManagerRepository(IDesignerToolContext ctx)
            : base(ctx)
        {

        }

        #region Db Connections

        public bool TestConnection()
        {
            try
            {
                var test = base.Context.SystemSettings.FirstOrDefault();
                return test != null;
            }
            catch (Exception ex)
            {
                Logger.Log("Database Test Connection Failed.", ex);
                return false;
            }
        }

        #endregion
    }
}
