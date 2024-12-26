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
        var resp = await _service.ListCategories();

        return Ok(resp);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProductCategoryById([FromRoute]int id)
    {
        var resp = await _service.GetCategoryById(id);
        if (resp == null)
            return NotFound();
        
        return Ok(resp);
    }
    
    [HttpPost]
    public async Task <IActionResult> CreateProductCategory([FromBody]CreateProductCategoryReq req)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var resp = await _service.CreateProductCategory(req);

        return CreatedAtAction(nameof(GetProductCategoryById), new { id = resp.Id }, resp);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProductCategory([FromRoute] int id, [FromBody] UpdateProductCategoryReq req)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        ProductCategoryResp? resp;
        try
        {
            await _service.GetCategoryById(id);
            await _service.UpdateCategory(id, req);
            resp = await _service.GetCategoryById(id);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (DatabaseCrudFailedException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }

        return Ok(resp);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProductCategory([FromRoute] int id)
    {
        try
        {
            await _service.GetCategoryById(id);
            await _service.DeleteCategory(id);
        }
        catch (NotFoundException ex)
        {
            return NotFound("data not found or has deleted");
        }
        catch (DatabaseCrudFailedException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }

        return Ok();
    }
}
