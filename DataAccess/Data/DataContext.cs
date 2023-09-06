using DataAccess.Data.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Data.Identity;

namespace DataAccess.Data
{
    public class DataContext : IdentityDbContext<IdentityUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Battle>()
                .HasOne(b => b.Attacker) // each battle has on attacker
                .WithMany() // each attacker has many battles.
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Battle>()
                .HasOne(b => b.Opponent)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Battle>()
                .HasOne(b => b.Winner)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Unit> Units { get; set; }
        public DbSet<User> AppUsers { get; set; }
        public DbSet<UserUnit> UserUnits { get; set; }
        public DbSet<Battle> Battles { get; set; }


        

    }
}
