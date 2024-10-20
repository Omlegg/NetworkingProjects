using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using SharedLib.Models.Entities;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace ServerApp.Context
{
    public class CafeDbContext : DbContext
    {
        
        private const string connectionString = "Server=localhost;Database=CafeDb;Intefrated Security = True;TrustServerCertificate=True;";
        public DbSet<User> Users{get;set;}
        public DbSet<Item> Items { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItemGroup> CartItemGroup { get; set; }

        public DbSet<Check> Checks {get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasKey(user => user.Id);

            modelBuilder
                .Entity<User>()
                .Property(user => user.UserName).IsRequired(true);

            modelBuilder
                .Entity<User>()
                .Property(user => user.Password).IsRequired(true);

            //modelBuilder
            //    .Entity<User>()
            //    .ToTable(tb => tb.HasCheckConstraint("MY_CHECK_CONSTRAINT", "len([Firstname]) > 5"));

            base.OnModelCreating(modelBuilder);
        }
    }
}