using ran_product_management_net.Database.Mongodb.Models;
using ran_product_management_net.Database.Postgresql.Models;

namespace ran_product_management_net.Models.DTOs.Response;

public class ProductCategoryResponse
{
    public string Name { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}

public class ProductInventoryResp
{
    public int Price { get; set; }
    public int Stock { get; set; }
    public int MinBuy { get; set; }
    public string Condition { get; set; } = null!;
    public string Status { get; set; } = null!;
}

public class ProductDetailResp
{
    public string? Desc { get; set; }

    public string Brand { get; set; } = null!;

    public string Model { get; set; } = null!;

    public ProductDetailBase SpecificDetails { get; set; } = null!;
}

public class ProductResp
{
    public Guid Id { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public ProductInventoryResp Inventory { get; set; } = null!;
    public ProductDetailResp Details { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
