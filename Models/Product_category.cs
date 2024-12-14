using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ran_product_management_net.Models
{
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("NOW()");
            builder.HasIndex(x => x.Name).IsUnique();
        }
    }

    [Table("product_categories")]
    public class ProductCategory
    {
        [Column("id")]
        [Required]
        [Key]
        public int ID { get; set; }
        [Column("name")]
        [Required]
        [MaxLength(255)]
        public string? Name { get; set; }
        [Column("desc")]
        public string Desc { get; set; } = string.Empty;
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("deleted_at", TypeName = "timestamp without time zone")]
        public DateTime? DeletedAt { get; set; }
        public ICollection<Product> Products { get; set; } = [];
    }
}
