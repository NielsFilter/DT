using DesignerTool.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.DataAccess.Repositories
{
    public class UnitTypeRepository : BaseRepository
    {
        public UnitTypeRepository(IDesignerToolContext ctx)
            : base(ctx)
        {

        }

        #region CRUD

        public void AddNew(UnitType item)
        {
            base.Context.UnitTypes.Add(item);
        }

        public void Delete(UnitType item)
        {
            base.Context.UnitTypes.Remove(item);
        }

        #endregion

        #region Get

        public UnitType GetById(long id)
        {
            return base.Context.UnitTypes
                .FirstOrDefault(ut => ut.UnitTypeID == id && ut.IsActive == true);
        }

        #endregion

        #region List

        public IQueryable<UnitType> ListAll()
        {
            return base.Context.UnitTypes.Where(ut => ut.IsActive == true);
        }

        public IQueryable<UnitType> Search_Paged(string searchText, int pageStartIndex, int pageSize)
        {
            return base.Context.UnitTypes.Where(ut => ut.Name.Contains(searchText))
                   .OrderBy(ut => ut.Name)
                   .Skip(pageStartIndex)
                   .Take(pageSize);
        }

        #endregion
    }
}
