using Microsoft.OpenApi.Extensions;

namespace ran_product_management_net.Models.DTOs.Response;

public class ProductCategoryResponse
{
    public string Name { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ModifiedAt { get; set; }
    public DateTime DeletedAt { get; set; }
}

public class ProductInventoryResponse
{
    public string? Status { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ModifiedAt { get; set; }
    public DateTime DeletedAt { get; set; }
}

public class CreateProductResponse
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public int Price { get; set; }
    public string? Condition { get; set; }
    public int MinBuy { get; set; }
    public ProductCategoryResponse? Category { get; set; }
    public ProductInventoryResponse? Inventory { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ModifiedAt { get; set; }
    public DateTime DeletedAt { get; set; }
}

public class ResMapper
{
    public static CreateProductResponse ToProductResponse(Product arg)
    {
        if (arg == null)
            return new CreateProductResponse();
            
        var product = new CreateProductResponse()
        {
            ID = arg.ID,
            Name = arg.Name,
            Desc = arg.Desc ?? string.Empty,
            Price = arg.Price,
            Condition = arg.Condition.ToString(),
            MinBuy = arg.MinBuy,
            Category = new ProductCategoryResponse
            {
                Name = arg.Category?.Name ?? string.Empty,
                Desc = arg.Category?.Desc ?? string.Empty
            },
            Inventory = new ProductInventoryResponse
            {
                Status = arg.Inventory?.Status.ToString(),
                Stock = arg.Inventory?.Stock ?? 0,
                CreatedAt = arg.Inventory?.CreatedAt ?? DateTime.Now,
                ModifiedAt = arg.Inventory?.ModifiedAt ?? DateTime.MinValue
            }
        };

        return product;
    }
}
