using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ran_product_management_net.Database.Postgresql;
using ran_product_management_net.Models.DTOs.Request;
using ran_product_management_net.Models.DTOs.Response;
using ran_product_management_net.Services;
using ran_product_management_net.Utils;

namespace ran_product_management_net.Controllers;

[Route("api/v1/product_category")]
[ApiController]
public class ProductCategoryController(ApplicationDbContext context, IMapper mapper) : ControllerBase
{
    private readonly ProductCategoryService _service = new(context, mapper);

    [HttpGet]
    public async Task<IActionResult> ListProductCategory()
    {
        List<ProductCategoryResp> result;

        try
        {
            result = await _service.ListCategories();
        }
        catch (NotFoundException e)
        {
            SuccessWithData<List<ProductCategoryResp>> emptyResp = new(200, "there are no category in the database", []);
            return Ok(emptyResp);
        }
        catch (Exception e)
        {
            FailedResponse errResp = new(500,  "there is an error in the server", 
                new Dictionary<string, List<string>>()
                {
                    {"cserver", [e.Message] } 
                });
            return StatusCode(500, errResp);
        }
        
        SuccessWithData<List<ProductCategoryResp>> response = new(200, "Product Category Data", result); 

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProductCategoryById([FromRoute]int id)
    {
        ProductCategoryResp category;
        try
        {
            category = await _service.GetCategoryById(id);
        }
        catch (NotFoundException e)
        {
            FailedResponse errResp = new(404,  "there are no category with this id", 
                new Dictionary<string, List<string>>()
                {
                    {"category id", [e.Message] } 
                });
            return new NotFoundObjectResult(errResp);
        }
        catch (Exception e)
        {
            FailedResponse errResp = new(500,  "there is an error in the server", 
                new Dictionary<string, List<string>>()
                {
                    {"category name", [e.Message] } 
                });
            return StatusCode(500, errResp);
        }
        
        SuccessWithData<ProductCategoryResp> response = new(200, "Product Category Data", category); 
        
        return new OkObjectResult(response);
    }
    
    [HttpPost]
    public async Task <IActionResult> CreateProductCategory([FromBody]CreateProductCategoryReq req)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        ProductCategoryResp categoryData;
        try
        {
            var getResult = await _service.GetAllCategoryByName(req.Name);
            if (getResult.isDeleted)
            {
                await _service.RecreateCategory(req);
                categoryData = await _service.GetCategoryById(getResult.categoryId);
            }
            else
            {
                categoryData = await _service.CreateProductCategory(req);
            }
        }
        catch (NotNullException e)
        {
            FailedResponse errResp = new(400,  "category name is required", 
                new Dictionary<string, List<string>>()
                {
                    {"category name", [e.Message] } 
                });
            
            return new BadRequestObjectResult(errResp);
        }
        catch (DuplicateDataException e)
        {
            FailedResponse errResp = new(400,  "category name is already in use", 
                new Dictionary<string, List<string>>()
                {
                    {"category name", [e.Message] } 
                });
            return new BadRequestObjectResult(errResp);
        }
        catch (LengthException e)
        {
            FailedResponse errResp = new(400,  "category name must be between 1 and 255 characters", 
                new Dictionary<string, List<string>>()
                {
                    {"category name", [e.Message] } 
                });
            return new BadRequestObjectResult(errResp);
        }
        catch (Exception e)
        {
            FailedResponse errResp = new(500,  "there is an error in the server", 
                new Dictionary<string, List<string>>()
                {
                    {"server", [e.Message] } 
                });
            return StatusCode(500, e.Message);
        }
        
        SuccessWithData<ProductCategoryResp> response = new(201, "new product category created", categoryData);

        return CreatedAtAction(nameof(GetProductCategoryById), new { id = categoryData.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProductCategory([FromRoute] int id, [FromBody] UpdateProductCategoryReq req)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        ProductCategoryResp? categoryData;
        try
        {
            await _service.GetCategoryById(id);
            await _service.UpdateCategory(id, req);
            categoryData = await _service.GetCategoryById(id);
        }
        catch (NotFoundException e)
        {
            FailedResponse errResp = new(404,  "there are no category with this id", 
                new Dictionary<string, List<string>>()
                {
                    {"category id", [e.Message] } 
                });
            return new NotFoundObjectResult(errResp);
        }
        catch (DatabaseCrudFailedException e)
        {
            FailedResponse errResp = new(400,  "new data should be different with old data", 
                new Dictionary<string, List<string>>()
                {
                    {"category", [e.Message] } 
                });
            return new BadRequestObjectResult(errResp);
        }
        catch (Exception e)
        {FailedResponse errResp = new(500,  "there is an error in the server", 
                new Dictionary<string, List<string>>()
                {
                    {"server", [e.Message] } 
                });
            return StatusCode(500, errResp);
        }
        
        SuccessWithData<ProductCategoryResp> response = new(200, "Updated Product Category Data", categoryData);

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProductCategory([FromRoute] int id)
    {
        try
        {
            await _service.GetCategoryById(id);
            await _service.DeleteCategory(id);
        }
        catch (NotFoundException e)
        {
            FailedResponse errResp = new(404,  "there are no category with this id", 
                new Dictionary<string, List<string>>()
                {
                    {"category id", [e.Message] } 
                });
            return new NotFoundObjectResult(errResp);
        }
        catch (DatabaseCrudFailedException e)
        {
            FailedResponse errResp = new(400,  "delete is failed", 
                new Dictionary<string, List<string>>()
                {
                    {"category", [e.Message] } 
                });
            return new BadRequestObjectResult(errResp);
        }
        catch (Exception e)
        {
            FailedResponse errResp = new(500,  "there is an error in the server", 
                new Dictionary<string, List<string>>()
                {
                    {"server", [e.Message] } 
                });
            return StatusCode(500, errResp);
        }
        
        SuccessWithMessage response = new SuccessWithMessage("success deleted category");

        return Ok(response);
    }
}
