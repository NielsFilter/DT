using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace DesignerTool.DataAccess.Data
{
    public interface IDesignerToolContext
    {
        DbSet<ActiveLicense> ActiveLicenses { get; set; }
        DbSet<License> Licenses { get; set; }
        DbSet<SystemSetting> SystemSettings { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Contact> Contacts { get; set; }
        DbSet<Debtor> Debtors { get; set; }
        DbSet<DebtorContact> DebtorContacts { get; set; }
        DbSet<Supplier> Suppliers { get; set; }
        DbSet<SupplierContact> SupplierContacts { get; set; }
        DbSet<UnitType> UnitTypes { get; set; }

        int ValidateAndSave();
        int SaveChanges();
    }
}
