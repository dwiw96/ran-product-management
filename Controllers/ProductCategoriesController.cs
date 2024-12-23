using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ran_product_management_net.Database.Postgresql;
using ran_product_management_net.Database.Postgresql.Models;
using ran_product_management_net.Models.DTOs.Request;
using ran_product_management_net.Models.DTOs.Response;

namespace ran_product_management_net.Controllers;

[Route("api/v1/product_category")]
[ApiController]
public class ProductCategoryController(ApplicationDbContext context, IMapper mapper) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<IActionResult> ListProductCategory()
    {
        var categories = await _context.ProductCategories.ToListAsync();

        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductCategoryById([FromRoute]int id)
    {
        var category = await _context.ProductCategories.FindAsync(id);
        if (category == null)
            return NotFound();
        
        return Ok(category);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProductCategory([FromBody]CreateProductCategoryReq req)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var category = _mapper.Map<ProductCategory>(req);

        await _context.ProductCategories.AddAsync(category);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProductCategoryById), new { id = category.Id }, _mapper.Map<ProductCategoryResp>(category));
    }

    // [HttpPut("{id}")]
    // public async Task<IActionResult> UpdateProductCategory([FromBody]int c_id, CreateProductCategoryReq req)
    // {
    //     if (!ModelState.IsValid)
    //         return BadRequest(ModelState);

    //     var categoryToUpdate = await _context.ProductCategories.FirstOrDefaultAsync(c => c.ID == c_id);

    //     if (await TryUpdateModelAsync<ProductCategory>(
    //         categoryToUpdate,
    //         "",
    //         c => c.Name, c => c.Desc))
    //     {
    //         try
    //         {
    //             await _context.SaveChangesAsync();
    //             return RedirectToAction(nameof(GetProductCategoryById), new { id =  }, _mapper.Map<ProductCategoryResp>(categoryToUpdate));
    //         }
    //     }
    // }
}
