using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace ran_product_management_net.Models.Integration;

public abstract class AuditableEntity
{
    [Column("created_at", TypeName = "timestamp without time zone")]
    [Required]
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    [Column("modified_at", TypeName = "timestamp without time zone")]
    [BsonElement("modified_at")]
    public DateTime? ModifiedAt { get; set; }
    
    [Column("deleted_at", TypeName = "timestamp without time zone")]
    [BsonElement("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}
