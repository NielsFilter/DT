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

        #region Get

        public T GetValue<T>(string settingCode)
        {
            var setting = this.GetSetting(settingCode);
            if (setting == null)
            {
                return default(T);
            }

            return (T)Convert.ChangeType(setting.Value, typeof(T));
        }

        public SystemSetting GetSetting(string settingCode)
        {
            return base.Context.SystemSettings
                .FirstOrDefault(x => x.Setting == settingCode);
        }

        #endregion
    }
}
