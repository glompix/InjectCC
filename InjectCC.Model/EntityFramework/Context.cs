using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using InjectCC.Model.Domain;

namespace InjectCC.Model.EntityFramework
{
    public interface IUnitOfWork
    {
        int SaveChanges();
    }

    public class Context : DbContext, IUnitOfWork
    {
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LocationModifier> LocationModifiers { get; set; }
        public virtual DbSet<Injection> Injections { get; set; }
        public virtual DbSet<Medication> Medications { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Injection>().HasRequired(i => i.Location).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Injection>().HasRequired(i => i.User).WithMany().WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);
        }
    }
}
