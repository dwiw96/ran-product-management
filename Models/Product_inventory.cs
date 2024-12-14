using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ran_product_management_net.Models
{
    public class ProductInventoryConfiguration : IEntityTypeConfiguration<ProductInventory>
    {
        public void Configure(EntityTypeBuilder<ProductInventory> builder)
        {
            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("NOW()");
            builder.HasIndex(x => x.ProductID).IsUnique();
            builder.ToTable(t =>
                t.HasCheckConstraint("ck_product_inventories_stock_range", "stock >= 0 AND stock <= 1000000"));
        }
    }

    public enum ProductInventoryStatus
    {
        Ready,
        Preorder
    }

    [Table("product_inventories")]
    public class ProductInventory
    {
        [Column("id")]
        [Required]
        [Key]
        public int ID { get; set; }
        [Column("product_id")]
        [Required]
        public int ProductID { get; set; }
        public Product Product { get; set; } = null!;
        [Column("status")]
        [Required]
        public ProductInventoryStatus Status { get; set; }
        [Column("stock")]
        [Required]
        [Range(0, uint.MaxValue)]
        public int Stock { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("deleted_at", TypeName = "timestamp without time zone")]
        public DateTime? DeletedAt { get; set; }
        
        public void Print()
        {
            Console.WriteLine("status: " + this.Status);
            Console.WriteLine("stock: " + this.Stock);
        }
    }
}