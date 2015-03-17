using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.DataAccess.Data
{
    public partial class Supplier : BaseModel
    { 
        #region Validation

        public override string Validation(string columnName)
        {
            switch (columnName)
            {
                case "Name":
                    if (string.IsNullOrWhiteSpace(this.Name))
                    {
                        return "Name is required.";
                    }
                    break;
            }

            return string.Empty; // No validation exceptions
        }

        #endregion

        public static Supplier New()
        {
            // Set all the defaults.
            Supplier newSupplier = new Supplier();
            newSupplier.IsActive = true;

            return newSupplier;
        }
    }
}
