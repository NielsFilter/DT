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
        DbSet<Person> People { get; set; }
        DbSet<SystemSetting> SystemSettings { get; set; }
        DbSet<User> Users { get; set; }

        int ValidateAndSave();
        int SaveChanges();
    }
}
