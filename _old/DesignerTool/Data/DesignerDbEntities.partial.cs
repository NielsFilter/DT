using DesignerTool.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.Data
{
    public partial class DesignerDbEntities
    {
        /// <summary>
        /// Override the SaveChanges method to add a validation check.
        /// </summary>
        /// <param name="options">Specifies the behavior of the object context when the System.Data.Objects.ObjectContext.SaveChanges(System.Data.Objects.SaveOptions) method is called</param>
        /// <returns>
        /// The number of objects in an System.Data.EntityState.Added, System.Data.EntityState.Modified, or System.Data.EntityState.Deleted state
        /// when System.Data.Objects.ObjectContext.SaveChanges() was called.</returns>
        public override int SaveChanges(System.Data.Objects.SaveOptions options)
        {
            this.Validate();
            return base.SaveChanges(options);
        }

        public void Validate()
        {
            var entriesToValidate = ObjectStateManager.GetObjectStateEntries(
                                        System.Data.EntityState.Added |
                                        System.Data.EntityState.Modified |
                                        System.Data.EntityState.Deleted)
                                            .Where(x => x.Entity is IValidatable);

            foreach (var entry in entriesToValidate)
            {
                var entity = entry.Entity as IValidatable;
                entity.Validate(entry.State);
            }
        }
    }
}
