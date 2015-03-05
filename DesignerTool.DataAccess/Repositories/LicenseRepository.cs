using DesignerTool.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.DataAccess.Repositories
{
    public class LicenseRepository : BaseRepository
    {
        public LicenseRepository(IDesignerToolContext ctx)
            : base(ctx)
        {

        }

        #region CRUD

        public void AddNew(License license)
        {
            base.Context.Licenses.Add(license);
        }

        #endregion

        #region Get

        public License GetFirstActive()
        {
            return base.Context.Licenses.FirstOrDefault(l => l.IsActive);
        }

        #endregion

        #region Other

        public IEnumerable<string> GetUsedLicenseCodes()
        {
            return base.Context.ActiveLicenses.Select(al => al.Code.ToUpper());
        }

        #endregion

        #region Used Licenses

        public void AddUsedLicense(ActiveLicense usedLicense)
        {
            base.Context.ActiveLicenses.Add(usedLicense); // Add the license Keys to the used list. Prevents reuse.
        }

        #endregion
    }
}
