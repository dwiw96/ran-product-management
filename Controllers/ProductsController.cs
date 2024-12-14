using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ran_product_management_net.Data;
using ran_product_management_net.Models;
using ran_product_management_net.Models.DTOs.Request;
using ran_product_management_net.Models.DTOs.Response;

namespace ran_product_management_net.Controllers;

[Route("api/v1/product")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public IActionResult GetByID([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
                
        var product = _context.Products?
            .Include(p => p.Category)
            .Include(p => p.Inventory)
            .FirstOrDefaultAsync(p => p.ID == id);
        
        if (product?.Result == null)
        {
            return NotFound();
        }

        return Ok(ResMapper.ToProductResponse(product.Result));
    }

    [HttpGet]
    public IActionResult ListProducts()
    {
        if(!ModelState.IsValid)
            BadRequest(ModelState);
        
        var products = _context.Products?.ToList();

        return Ok(products);
    }

    [HttpPost]
    public IActionResult CreateProduct([FromBody] CreateProductReq productReq)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var category = _context.ProductCategories?.FirstOrDefault(c => c.ID == productReq.CategoryID);
        if (category == null)
        {
            return BadRequest("product category is not found");
        }

        var product = ReqMapper.ToProduct(productReq);
        product.Category = category;

        var inventory = ReqMapper.ToProductInventory(productReq, product.ID);
        product.Inventory = inventory;

        _context.Products?.Add(product);

        _context.SaveChanges();

        return CreatedAtAction(nameof(GetByID), new { id = product.ID}, ResMapper.ToProductResponse(product));
    }
}

