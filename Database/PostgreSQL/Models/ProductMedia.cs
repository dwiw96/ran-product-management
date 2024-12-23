using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ran_product_management_net.Database.Postgresql.Models;

public class ProductMediaConfiguration : IEntityTypeConfiguration<ProductMedia>
{
    public void Configure(EntityTypeBuilder<ProductMedia> builder)
    {
        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}

public enum ProductMediaType
{
    Image,
    Video,
    File
}

[Table("product_media")]
public class ProductMedia
{
    [Column("id")]
    [Required]
    [Key]
    public int Id { get; set; }

    [Column("product_id")]
    [Required]
    public int ProductID { get; set; }
    public ProductInventory ProductInventory { get; set; } = null!;

    [Column("type")]
    [Required]
    public ProductMediaType Type { get; set; }

    [Column("url")]
    [Required]
    public string Url { get; set; } = null!;

    [Column("created_at", TypeName = "timestamp without time zone")]
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    
    [Column("deleted_at", TypeName = "timestamp without time zone")]
    public DateTime? DeletedAt { get; set; }
}
