using System.ComponentModel.DataAnnotations;

namespace ran_product_management_net.Models.DTOs.Request;

public class CreateProductCategoryReq
{
    [Required(ErrorMessage = "enter the name field")]
    public required string Name { get; set; }
    public string? Desc { get; set; }
}
