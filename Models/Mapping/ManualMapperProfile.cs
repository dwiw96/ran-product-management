using System.Reflection;
using AutoMapper;
using MongoDB.Bson.Serialization.Attributes;
using ran_product_management_net.Database.Mongodb.Models;
using ran_product_management_net.Database.Postgresql.Models;
using ran_product_management_net.Models.DTOs.Response;

namespace ran_product_management_net.Models.Mapping;

public class ManualMapping
{
    public static T ToSpecificDetail<T>(Dictionary<string,string> details) where T : ProductDetailBase, new()
    {
        T result = Activator.CreateInstance<T>();

        var properties = typeof(T).GetProperties()
            .Where(p => p.GetCustomAttributes(typeof(BsonElementAttribute), false).Any());

        foreach (var prop in properties)
        {
            var bsonAttribute = prop.GetCustomAttribute<BsonElementAttribute>();
            if (bsonAttribute != null && details.TryGetValue(bsonAttribute.ElementName, out string? value))
            {
                prop.SetValue(result, value);
            }
        }

        return result;
    }

    public static List<ProductResp> ToProductResponse(List<ProductInventory> inventory, List<ProductDetail> productDetail, IMapper mapper)
    {
        List<ProductResp> result = [];

        var length = inventory.Count;
        for (var i = 0; i < length; i++)
        {
            ProductResp temp = new()
            {
                Id = inventory[i].Id,
                Category = productDetail[i].Category,
                Name = productDetail[i].ProductName,
                Inventory = mapper.Map<ProductInventoryResp>(inventory[i]),
                Details = mapper.Map<ProductDetailResp>(productDetail[i]),
                CreatedAt = inventory[i].CreatedAt
            };

            result.Add(temp);
        }

        return result;
    }

    public static ProductResp ToProductResponse(ProductInventory inventory, ProductDetail productDetail, IMapper mapper)
    {
        ProductResp result = new()
        {
            Id = inventory.Id,
            Category = productDetail.Category,
            Name = productDetail.ProductName,
            Inventory = mapper.Map<ProductInventoryResp>(inventory),
            Details = mapper.Map<ProductDetailResp>(productDetail),
            CreatedAt = inventory.CreatedAt
        };

        return result;
    }
}
