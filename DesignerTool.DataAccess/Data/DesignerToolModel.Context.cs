﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DesignerTool.DataAccess.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DesignerToolDbEntities : DbContext
    {
        public DesignerToolDbEntities()
            : base("name=DesignerToolDbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ActiveLicense> ActiveLicenses { get; set; }
        public virtual DbSet<License> Licenses { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<SystemSetting> SystemSettings { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
