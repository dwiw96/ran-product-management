using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ran_product_management_net.Models
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("NOW()");
            builder.ToTable(t =>
                t.HasCheckConstraint("ck_products_price_range", "price > 0 AND price <= 1000000000"));
            builder.ToTable(t => 
                t.HasCheckConstraint("ck_products_min_buy_range", "min_buy > 0 AND min_buy <= 1000000"));
        }
    }

    public enum ProductCondition
    {
        New,
        Used
    }

    [Table("products")]
    public class Product
    {
        [Column("id")]
        [Required]
        [Key]
        public int ID { get; set; }
        [Column("name")]
        [MaxLength(255)]
        [Required(ErrorMessage = "product name is required")]
        public string Name { get; set; } = string.Empty;
        [Column("desc")]
        public string? Desc { get; set; }
        [Column("price")]
        [Required]
        [Range(1, int.MaxValue)]
        public int Price { get; set; }
        [Column("condition")]
        [Required]
        public ProductCondition Condition { get; set; }
        [Column("min_buy")]
        [Required]
        [Range(1, uint.MaxValue)]
        public int MinBuy { get; set; }
        [Column("category_id")]
        [Required]
        public int CategoryID { get; set; }
        public ProductCategory Category { get; set; } = null!;
        public ProductInventory Inventory { get; set; } = null!;
        [Required]
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime ModifiedAt { get; set; }
        [Column("deleted_at", TypeName = "timestamp without time zone")]
        public DateTime DeletedAt { get; set; }

        public void Print()
        {
            Console.WriteLine("Product");
            Console.WriteLine("Name: " + this.Name);
            Console.WriteLine("Desc: " + this.Desc);
            Console.WriteLine("Price: " + this.Price);
            Console.WriteLine("Condition: " + this.Condition);
            Console.WriteLine("MinBuy: " + this.MinBuy);
            Console.WriteLine("Category.Name: " + this.Category?.Name);
            Console.WriteLine("Category.Desc: " + this.Category?.Desc);
        }
    }
}