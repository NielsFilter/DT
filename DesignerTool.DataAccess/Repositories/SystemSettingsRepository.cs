using DesignerTool.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.DataAccess.Repositories
{
    public class SystemSettingsRepository : BaseRepository
    {
        public SystemSettingsRepository(IDesignerToolContext ctx)
            : base(ctx)
        {

        }

        #region CRUD

        public void UpdateValue(string settingName, string value)
        {
            var setting = base.Context.SystemSettings.First(s => s.Setting == settingName);
            setting.Value = value;
        }

        #endregion
    }
}
