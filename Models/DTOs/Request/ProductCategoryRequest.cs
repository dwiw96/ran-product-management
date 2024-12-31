using System.ComponentModel.DataAnnotations;

namespace ran_product_management_net.Models.DTOs.Request;

public class CreateProductCategoryReq
{
    [Required(ErrorMessage = "enter the name field")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "{0} must be between {2} and {1} character")]
    public required string Name { get; set; }
    public string? Desc { get; set; }
}

public class UpdateProductCategoryReq
{
    [StringLength(255, MinimumLength = 1, ErrorMessage = "{0} must be between {2} and {1} character")]
    public string? Name { get; set; }
    public string? Desc { get; set; }
}
