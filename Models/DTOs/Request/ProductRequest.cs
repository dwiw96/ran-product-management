using System.Reflection;
using MongoDB.Bson.Serialization.Attributes;
using ran_product_management_net.Database.Mongodb.Models;
using ran_product_management_net.Database.Postgresql.Models;

namespace ran_product_management_net.Models.DTOs.Request;

public class ListProductDto
{
    public List<ProductInventory> Inventory { get; set; } = null!;
    public List<ProductDetail> Detail { get; set; } = null!;
}

public class GetProductDto
{
    public ProductInventory Inventory { get; set; } = null!;
    public ProductDetail Detail { get; set; } = null!;
}

public class CreateProductReq
{
    public string ProductName { get; set; } = string.Empty;
    public int Price { get; set; }
    public int Stock { get; set; }
    public int MinBuy { get; set; }
    public ProductCondition Condition { get; set; }
    public ProductStatus Status { get; set; }
    public int CategoryId { get; set; }
    public string? Desc { get; set; }
    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public Dictionary<string, string> Details { get; set; } = null!;

    public void Print()
    {
        Console.WriteLine("ProductReq");
        Console.WriteLine("Name: " + this.ProductName);
        Console.WriteLine("Price: " + this.Price);
        Console.WriteLine("Stock: " + this.Stock);
        Console.WriteLine("MinBuy: " + this.MinBuy);
        Console.WriteLine("Condition: " + this.Condition);
        Console.WriteLine("Status: " + this.Status);
        Console.WriteLine("CategoryId: " + this.CategoryId);
        Console.WriteLine("Desc: " + this.Desc);
        Console.WriteLine("Brand: " + this.Brand);
        Console.WriteLine("Model: " + this.Model);
        Console.WriteLine("Details: ");
        foreach (KeyValuePair<string, string> d in this.Details)
        {
            Console.WriteLine("{0}: {1}", d.Key, d.Value);
        }
    }
}

// public class ManualMapping2
// {
//     public Dictionary<string, string>? Details { get; set; }

//     public static object ToSpecificDetail<T>(Dictionary<string,string> details)
//     {
//         object result = typeof(T);
//         var properties = typeof(T).GetProperties()
//             .Where(p => p.GetCustomAttributes(typeof(BsonElementAttribute), false).Any());

//         foreach (var prop in properties)
//         {
//             var bsonAttribute = prop.GetCustomAttribute<BsonElementAttribute>();
//             if (bsonAttribute != null && details.TryGetValue(bsonAttribute.ElementName, out string? value))
//             {
//                 prop.SetValue(result, value);
//             }
//         }

//         return result;
//     }
//     public static Smartphone ToSmartphone(Dictionary<string,string> details)
//     {
//         var smartphone = new Smartphone();
//         var properties = typeof(Smartphone).GetProperties()
//             .Where(p => p.GetCustomAttributes(typeof(BsonElementAttribute), false).Any());

//         foreach (var prop in properties)
//         {
//             var bsonAttribute = prop.GetCustomAttribute<BsonElementAttribute>();
//             if (bsonAttribute != null && details.TryGetValue(bsonAttribute.ElementName, out string? value))
//             {
//                 prop.SetValue(smartphone, value);
//             }
//         }

//         return smartphone;
//     }
// }
