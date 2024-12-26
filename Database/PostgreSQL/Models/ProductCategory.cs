using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ran_product_management_net.Database.Postgresql.Models;

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
    public int Id { get; set; }
    
    [Column("name")]
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    
    [Column("desc")]
    public string? Desc { get; set; }
    
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    
    [Column("deleted_at", TypeName = "timestamp without time zone")]
    public DateTime? DeletedAt { get; set; }
    
    public ICollection<ProductInventory> ProductInventories { get; set; } = [];

    public void Print()
    {
        Console.WriteLine("Product Category");
        Console.WriteLine("Id: " + this.Id);
        Console.WriteLine("Name: " + this.Name);
        Console.WriteLine("Desc: " + this.Desc);
        Console.WriteLine("CreatedAt: " + this.CreatedAt);
    }
}
