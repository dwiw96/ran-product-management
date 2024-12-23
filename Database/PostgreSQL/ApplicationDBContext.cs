using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ran_product_management_net.Database.Postgresql.Models;

namespace ran_product_management_net.Database.Postgresql;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    public required DbSet<ProductInventory> ProductInventories { get; set; }
    public required DbSet<ProductCategory> ProductCategories { get; set; }
    public required DbSet<ProductMedia> ProductMedias { get; set; }

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
        modelBuilder.ApplyConfiguration(new ProductMediaConfiguration());
    }
}
