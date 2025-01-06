using System.ComponentModel.DataAnnotations;
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
    [Required(ErrorMessage = "product_name is required")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Product name must be between 1 and 255 characters")]
    public string ProductName { get; set; } = string.Empty;
    [Required(ErrorMessage = "price is required")]
    [Range(1, 1000000000, ErrorMessage = "price must be between $1 and $2")]
    public int Price { get; set; }
    [Required(ErrorMessage = "stock is required")]
    [Range(1, 1000000000, ErrorMessage = "stock must be between $1 and $2")]
    public int Stock { get; set; }
    [Required(ErrorMessage = "min_buy is required")]
    [Range(1, 1000000, ErrorMessage = "min_buy must be between $1 and $2")]
    public int MinBuy { get; set; }
    [Required(ErrorMessage = "condition is required")]
    public ProductCondition Condition { get; set; }
    [Required(ErrorMessage = "status is required")]
    public ProductStatus Status { get; set; }
    [Required(ErrorMessage = "category is required")]
    public int CategoryId { get; set; }
    public string? Desc { get; set; }
    [Required(ErrorMessage = "brand is required")]
    public string Brand { get; set; } = null!;
    [Required(ErrorMessage = "models is required")]
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
