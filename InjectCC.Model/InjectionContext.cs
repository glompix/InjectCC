using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace InjectCC.Model
{
    public class InjectionContext : DbContext
    {
        public DbSet<Location> Locations { get; set; }
        public DbSet<Injection> Injections { get; set; }
        public DbSet<LocationSet> LocationSets { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Injection>().HasRequired(i => i.Location).WithMany().WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);
        }  
    }
}
