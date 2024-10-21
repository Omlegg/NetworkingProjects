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
        
        private const string connectionString = "Server=localhost;Database=CafeDb;Integrated Security = True;TrustServerCertificate=True;";
        public DbSet<User> Users{get;set;}
        public DbSet<Log> Logs{get;set;}
        public DbSet<Item> Items { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public DbSet<Card> Cards { get; set; }

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
                .HasKey(user => user.id);

            modelBuilder
                .Entity<Cart>()
                .HasKey(user => user.Id);
            
            modelBuilder
                .Entity<Item>()
                .HasKey(user => user.Id);
            
            modelBuilder
                .Entity<Cart>()
                .HasKey(user => user.Id);

            modelBuilder
                .Entity<CartItemGroup>()
                .HasKey(user => user.Id);
            
            modelBuilder
                .Entity<Log>()
                .HasKey(user => user.Id);

            modelBuilder
                .Entity<User>()
                .Property(user => user.userName).IsRequired(true);

            modelBuilder
                .Entity<User>()
                .Property(user => user.password).IsRequired(true);

            modelBuilder
                .Entity<Check>()
                .HasKey(user => user.Id);
            modelBuilder
                .Entity<Card>()
                .ToTable(tb => tb.HasCheckConstraint("CHECK_CONSTRAINT", "[AmountOfCoffee] <= 12"));

            base.OnModelCreating(modelBuilder);
        }
    }
}