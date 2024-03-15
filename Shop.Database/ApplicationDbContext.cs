using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Database
{
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stock {  get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderStock> OrderStocks { get; set; }

        public DbSet<StockOnHold> StockOnHolds { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Stripe> Stripes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<OrderStock>()
                .HasKey(x => new { x.StockId, x.OrderId });

            modelbuilder.Entity<Product>()
            .HasOne(p => p.image)
            .WithOne(i => i.Product)
            .HasForeignKey<Image>(i => i.ProductId);

           
            modelbuilder.Entity<Product>()
                .HasOne(p => p.Stripe)
                .WithOne(s => s.Product)
                .HasForeignKey<Stripe>(s => s.ProductId);
        
    }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}
