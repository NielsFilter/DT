using DesignerTool.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.DataAccess.Repositories
{
    public class BaseRepository
    {
        public IDesignerToolContext Context { get; set; }

        public BaseRepository(IDesignerToolContext ctx)
        {
            this.Context = ctx;
        }

        public int ValidateAndCommit()
        {
            return this.Context.ValidateAndSave();
        }

        public int Commit()
        {
            return this.Context.SaveChanges();
        }
    }
}
