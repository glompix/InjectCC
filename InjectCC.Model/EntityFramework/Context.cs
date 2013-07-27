using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using InjectCC.Model.Domain;

namespace InjectCC.Model.EntityFramework
{
    interface IUnitOfWork
    {
        int SaveChanges();
    }

    public class Context : DbContext, IUnitOfWork
    {
        internal DbSet<Location> Locations { get; set; }
        internal DbSet<LocationModifier> LocationModifiers { get; set; }
        internal DbSet<Injection> Injections { get; set; }
        internal DbSet<Medication> Medications { get; set; }
        internal DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Injection>().HasRequired(i => i.Location).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Injection>().HasRequired(i => i.User).WithMany().WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);
        }
    }
}
