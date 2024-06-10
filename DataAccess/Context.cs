using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Context : DbContext
    { 
        public DbSet<User> Users { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Session> Sessions { get; set; }

        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<ConstructionCompany> ConstructionCompanies { get; set; }

        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public Context(DbContextOptions options) : base(options) { }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Apartment>().HasOne(model => model.Owner).WithMany().OnDelete(DeleteBehavior.Restrict);
        }
        */
        
    }
}
