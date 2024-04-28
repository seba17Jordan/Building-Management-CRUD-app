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
        public DbSet<Session> Sessions { get; set; }

        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Category> Categories { get; set; }
        public Context(DbContextOptions options) : base(options) { }
    }
}
