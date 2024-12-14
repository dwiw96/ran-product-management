namespace ran_product_management_net.Models.DTOs.Request;

public class CreateProductReq
{
    public string Name { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public int Price { get; set; }
    public ProductCondition Condition { get; set; }
    public int MinBuy { get; set; }
    public int CategoryID { get; set; }
    public ProductInventoryStatus Status { get; set; }
    public int Stock { get; set; }

    public void Print()
    {
        Console.WriteLine("ProductReq");
        Console.WriteLine("Name: " + this.Name);
        Console.WriteLine("Desc: " + this.Desc);
        Console.WriteLine("Price: " + this.Price);
        Console.WriteLine("Condition: " + this.Condition);
        Console.WriteLine("MinBuy: " + this.MinBuy);
        Console.WriteLine("Category.Id: " + this.CategoryID);
        Console.WriteLine("Inventory.Status: " + this.Status);
        Console.WriteLine("Inventory.Stock: " + this.Stock);
    }
}

public class ReqMapper
{
    public static Product ToProduct(CreateProductReq arg)
    {
        var product = new Product()
        {
            Name = arg.Name,
            Desc = arg.Desc,
            Price = arg.Price,
            Condition = arg.Condition,
            MinBuy = arg.MinBuy,
            Category = new ProductCategory
            {
                ID = arg.CategoryID
            }
        };

        return product;
    }

    public static ProductInventory ToProductInventory(CreateProductReq productReq, int productID)
    {
        var inventory = new ProductInventory()
        {
            ProductID = productID,
            Status = productReq.Status,
            Stock = productReq.Stock
        };

        return inventory;
    }
}
