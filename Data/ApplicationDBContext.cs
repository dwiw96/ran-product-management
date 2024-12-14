using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ran_product_management_net.Models;

namespace ran_product_management_net.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions){}

        public DbSet<Product> ?Products { get; set; }
        public DbSet<ProductCategory> ?ProductCategories { get; set; }
        public DbSet<ProductInventory> ?ProductInventories {get; set; }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     base.OnConfiguring(optionsBuilder);
        //     optionsBuilder.ConfigureWarnings(warning =>
        //         warning.Ignore(RelationalEventId.PendingModelChangesWarning));
        // }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductInventoryConfiguration());
        }
    }
}