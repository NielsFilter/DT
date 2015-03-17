using DesignerTool.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.DataAccess.Repositories
{
    public class SupplierRepository : BaseRepository
    {
        public SupplierRepository(IDesignerToolContext ctx)
            : base(ctx)
        {

        }

        #region CRUD

        public void AddNew(Supplier item)
        {
            base.Context.Suppliers.Add(item);
        }

        public void Delete(Supplier item)
        {
            base.Context.Suppliers.Remove(item);
        }

        #endregion

        #region Get

        public Supplier GetById(long id)
        {
            return base.Context.Suppliers
                .FirstOrDefault(s => s.SupplierID == id && s.IsActive == true);
        }

        #endregion

        #region List

        public IQueryable<Supplier> ListAll()
        {
            return base.Context.Suppliers.Where(s => s.IsActive == true);
        }

        public IQueryable<Supplier> Search_Paged(string searchText, int pageStartIndex, int pageSize)
        {
            return base.Context.Suppliers.Where(s => s.Name.Contains(searchText))
                   .OrderBy(s => s.Name)
                   .Skip(pageStartIndex)
                   .Take(pageSize);
        }

        #endregion
    }
}
