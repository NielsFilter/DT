using DesignerTool.Common.Data;
using DesignerTool.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace DesignerTool.AppLogic.Data
{
    public partial class DesignerToolDbEntities
    {
        public int ValidateAndSave()
        {
            this.ChangeTracker.DetectChanges();
            var validationExceptions = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntries(
                                                                                                        System.Data.Entity.EntityState.Added |
                                                                                                        System.Data.Entity.EntityState.Modified |
                                                                                                        System.Data.Entity.EntityState.Deleted)
                                            .Where(x => x.Entity is IValidatable)
                                            .Select(x => ((IValidatable)x.Entity).ValidateAll());



            if (validationExceptions.Any(e => e.Count > 0))
            {
                // Bundle validation exceptions and throw it.
                ModelValidationExceptions ex = new ModelValidationExceptions();
                ex.ValidationExceptions = new List<string>();
                validationExceptions.ToList().ForEach(v => ex.ValidationExceptions.AddRange(v));

                throw ex;
            }

            // Done with validation. Save changes.
            return base.SaveChanges();
        }
    }
}
