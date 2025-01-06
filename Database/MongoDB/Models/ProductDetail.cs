using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ran_product_management_net.Models.Integration;
using ran_product_management_net.Utils;


namespace ran_product_management_net.Database.Mongodb.Models;

[BsonDiscriminator("ProductDetailBase", RootClass = true)]
[BsonKnownTypes(typeof(Smartphone), typeof(Fashion), typeof(Electronic))]
[JsonConverter(typeof(JsonPolymorphicConverter<ProductDetailBase>))]
public class ProductDetailBase {}

public class ProductDetail : AuditableEntity
{
    [BsonId]
    [BsonElement("_id")]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }

    [BsonElement("product_name")]
    public string ProductName { get; set; } = null!;

    [BsonElement("desc")]
    public string? Desc { get; set; }

    [BsonElement("brand")]
    public string Brand { get; set; } = null!;

    [BsonElement("model")]
    public string Model { get; set; } = null!;

    [BsonElement("category")]
    public string Category { get; set; } = null!;

    [BsonElement("specific_details")]
    public ProductDetailBase SpecificDetails { get; set; } = null!;

    // [BsonElement("created_at")]
    // public DateTime CreatedAt { get; set; } = DateTime.Now;
    //
    // [BsonElement("modified_at")]
    // public DateTime? ModifiedAt { get; set; }
    //
    // [BsonElement("deleted_at")]
    // public DateTime? DeletedAt { get; set; }


    // public void Display()
    // {
    //     Console.WriteLine("---------------------------------------------");
    //     Console.WriteLine("Product Detail:");
    //     Console.WriteLine("Id: " + this.Id);
    //     Console.WriteLine("ProductName: " + this.ProductName);
    //     Console.WriteLine("Desc: " + this.Desc);
    //     Console.WriteLine("Brand: " + this.Brand);
    //     Console.WriteLine("Model: " + this.Model);
    //     Console.WriteLine("Category: " + this.Category);
    //     Console.WriteLine("Specific Details: " + this.SpecificDetails);
    //     Console.WriteLine("---------------------------------------------");
    // }
}

[BsonDiscriminator("Smartphone")]
public class Smartphone : ProductDetailBase
{
    [BsonElement("operating_system")]
    public string OperatingSystem { get; set; } = null!;

    [BsonElement("cpu")]
    public string Cpu { get; set; } = null!;

    [BsonElement("storage")]
    public string Storage { get; set; } = null!;

    [BsonElement("ram")]
    public string Ram { get; set; } = null!;

    [BsonElement("screen_size")]
    public string ScreenSize { get; set; } = null!;

    [BsonElement("battery")]
    public string Battery { get; set; } = null!;

    [BsonElement("color")]
    public string? Color { get; set; }

    // public void Display()
    // {
    //     Console.WriteLine("---------------------------------------------");
    //     Console.WriteLine("Smartphone");
    //     Console.WriteLine("OperatingSystem: " + this.OperatingSystem);
    //     Console.WriteLine("CPU: " + this.CPU);
    //     Console.WriteLine("Storage: " + this.Storage);
    //     Console.WriteLine("Ram: " + this.Ram);
    //     Console.WriteLine("ScreenSize: " + this.ScreenSize);
    //     Console.WriteLine("Battery: " + this.Battery);
    //     Console.WriteLine("Color: " + this.Color);
    //     Console.WriteLine("---------------------------------------------");
    // }
}

[BsonDiscriminator("Fashion")]
public class Fashion : ProductDetailBase
{
    [BsonElement("fabric_type")]
    public string? FabricType { get; set; }

    [BsonElement("size")]
    public string Size { get; set; } = null!;

    [BsonElement("color")]
    public string? Color { get; set; }
}

[BsonDiscriminator("Electronic")]
public class Electronic : ProductDetailBase
{
    public enum DimensionUnit
    {
        Metric,
        Imperial
    }
    [BsonElement("unit")]
    public DimensionUnit Unit { get; set; }
    
    [BsonElement("dimension")]
    public string? Dimension { get; set; }
    
    [BsonElement("power")]
    public string? Power { get; set; }
    
    [BsonElement("color")]
    public string? Color { get; set; }
}
