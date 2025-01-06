using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ran_product_management_net.Models.Integration;

namespace ran_product_management_net.Database.Postgresql.Models;

public class ProductConfiguration : IEntityTypeConfiguration<ProductInventory>
{
    public void Configure(EntityTypeBuilder<ProductInventory> builder)
    {
        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("NOW()");
        builder.ToTable(t =>
            t.HasCheckConstraint("ck_product_inventory_price_range", "price > 0 AND price <= 1000000000"));
        builder.ToTable(t => 
            t.HasCheckConstraint("ck_product_inventory_min_buy_range", "min_buy > 0 AND min_buy <= 1000000"));
    }
}

public enum ProductCondition
{
    New,
    Used
}

public enum ProductStatus
{
    Ready,
    Preorder
}

[Table("product_inventory")]
public class ProductInventory : AuditableEntity
{
    [Column("id")]
    [Required]
    [Key]
    public Guid Id { get; set; }
    
    [Column("price")]
    [Required]
    [Range(1, int.MaxValue)]
    public int Price { get; set; }
    
    [Column("stock")]
    [Required]
    [Range(0, uint.MaxValue)]
    public int Stock { get; set; }
    
    [Column("min_buy")]
    [Required]
    [Range(1, uint.MaxValue)]
    public int MinBuy { get; set; }
    
    [Column("condition")]
    [Required]
    public ProductCondition Condition { get; set; }
    
    [Column("status")]
    [Required]
    public ProductStatus Status { get; set; }
    
    [Column("category_id")]
    [Required]
    public int CategoryId { get; set; }
    public ProductCategory Category { get; set; } = null!;
    
    // [Column("created_at", TypeName = "timestamp without time zone")]
    // [Required]
    // public DateTime CreatedAt { get; set; } = DateTime.Now;
    //
    // [Column("modified_at", TypeName = "timestamp without time zone")]
    // public DateTime? ModifiedAt { get; set; }
    //
    // [Column("deleted_at", TypeName = "timestamp without time zone")]
    // public DateTime? DeletedAt { get; set; }

    public void Print()
    {
        Console.WriteLine("Product Inventory");
        Console.WriteLine("Price: " + this.Price);
        Console.WriteLine("Stock: " + this.Stock);
        Console.WriteLine("MinBuy: " + this.MinBuy);
        Console.WriteLine("Condition: " + this.Condition);
        Console.WriteLine("Status: " + this.Status);
        Console.WriteLine("Category.Name: " + this.Category?.Name);
        Console.WriteLine("Category.Desc: " + this.Category?.Desc);
    }
}
