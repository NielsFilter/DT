using DesignerTool.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.DataAccess.Repositories
{
    public class DebtorRepository : BaseRepository
    {
        public DebtorRepository(IDesignerToolContext ctx)
            : base(ctx)
        {

        }

        #region CRUD

        public void AddNew(Debtor debtor)
        {
            base.Context.Debtors.Add(debtor);
        }

        public void Delete(Debtor debtor)
        {
            base.Context.Debtors.Remove(debtor);
        }

        #endregion

        #region Get

        public Debtor GetById(long id)
        {
            return base.Context.Debtors
                .FirstOrDefault(d => d.DebtorID == id && d.IsActive == true);
        }

        #endregion

        #region List

        public IQueryable<Debtor> ListAll()
        {
            return base.Context.Debtors.Where(d => d.IsActive == true);
        }

        public IQueryable<Debtor> Search_Paged(string searchText, int pageStartIndex, int pageSize)
        {
            return base.Context.Debtors.Where(d => d.Name.Contains(searchText))
                   .OrderBy(d => d.Name)
                   .Skip(pageStartIndex)
                   .Take(pageSize);
        }

        #endregion
    }
}
