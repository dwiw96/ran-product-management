namespace ran_product_management_net.Models.DTOs.Response;

public class ProductCategoryResp
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Desc { get; set; }
    public string CreatedAt { get; set; } = null!;
}
