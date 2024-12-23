using ran_product_management_net.Database.Postgresql.Models;
using ran_product_management_net.Database.Mongodb.Models;

namespace ran_product_management_net.Models.Integration;

public class Product
{
    public ProductDetail ProductDetails { get; set; } = null!;
    public ProductInventory ProductInventories { get; set; } = null!;

    public void Print()
    {
        Console.WriteLine("Product");
        Console.WriteLine("ProductInventories.Id: " + this.ProductInventories.Id);
        Console.WriteLine("ProductInventories.Price: " + this.ProductInventories.Price);
        Console.WriteLine("ProductInventories.Stock: " + this.ProductInventories.Stock);
        Console.WriteLine("ProductInventories.MinBuy: " + this.ProductInventories.MinBuy);
        Console.WriteLine("ProductInventories.Condition: " + this.ProductInventories.Condition);
        Console.WriteLine("ProductInventories.Status: " + this.ProductInventories.Status);
        Console.WriteLine("ProductInventories.CategoryId: " + this.ProductInventories.CategoryId);
        Console.WriteLine("ProductInventories.Price: " + this.ProductInventories.Price);

        Console.WriteLine("ProductDetails.Id: " + this.ProductDetails.Id);
        Console.WriteLine("ProductDetails.ProductName: " + this.ProductDetails.ProductName);
        Console.WriteLine("ProductDetails.Desc: " + this.ProductDetails.Desc);
        Console.WriteLine("ProductDetails.Brand: " + this.ProductDetails.Brand);
        Console.WriteLine("ProductDetails.Model: " + this.ProductDetails.Model);
        Console.WriteLine("ProductDetails.Category: " + this.ProductDetails.Category);
        Console.WriteLine("ProductDetails.SpecificDetails: " + this.ProductDetails.SpecificDetails);
        
    }
}
