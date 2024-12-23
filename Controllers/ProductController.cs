using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using ran_product_management_net.Database.Mongodb;
using ran_product_management_net.Database.Mongodb.Models;
using ran_product_management_net.Database.Postgresql;
using ran_product_management_net.Database.Postgresql.Models;
using ran_product_management_net.Models.DTOs.Request;
using ran_product_management_net.Models.Integration;
using System.Text.Json;
using ran_product_management_net.Models.Mapping;
using ran_product_management_net.Services;

namespace ran_product_management_net.Controllers;

[Route("api/v1/product")]
[ApiController]
public class ProductController(ApplicationDbContext context, MongoDBService mongoDBService, IMapper mapper) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMongoCollection<ProductDetail>? _productDetailCollection = mongoDBService.Database?.GetCollection<ProductDetail>("product_details");
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<IActionResult> ListProducts()
    {
        var productInventory = await _context.ProductInventories.ToListAsync();
        if (productInventory == null)
            return NotFound();

        var productDetail = await _productDetailCollection.Find(FilterDefinition<ProductDetail>.Empty).ToListAsync();
        if (productDetail == null)
            return NotFound();

        var jsonOptions = new JsonSerializerOptions
        {
            Converters = { new JsonPolymorphicConverter<ProductDetailBase>() },
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            WriteIndented = true
        };

        var productResp = ManualMapping.ToProductResponse(productInventory, productDetail, _mapper);

        var response = JsonSerializer.Serialize(productResp, jsonOptions);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public  async Task<IActionResult> GetByID([FromRoute] string id)
    {
        // if (!ModelState.IsValid)
        //     return BadRequest(ModelState);
        
        var uuid = Guid.Parse(id);
        var productInventory = await _context.ProductInventories
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == uuid);

        if (productInventory == null)
        {
            return NotFound();
        }

        var filter = Builders<ProductDetail>.Filter.Eq(f => f.Id, uuid);
        var productDetail = await _productDetailCollection.Find(filter).FirstOrDefaultAsync();
        if (productDetail == null)
            return NotFound();

        var jsonOptions = new JsonSerializerOptions
        {
            Converters = { new JsonPolymorphicConverter<ProductDetailBase>() },
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            WriteIndented = true
        };

        var productResp = ManualMapping.ToProductResponse(productInventory, productDetail, _mapper);

        var response = JsonSerializer.Serialize(productResp, jsonOptions);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductReq productReq)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var category = await _context.ProductCategories.FirstOrDefaultAsync(c => c.Id == productReq.CategoryId);
        if (category == null)
        {
            return BadRequest("product category is not found");
        }

        var uuid = Guid.NewGuid();

        var productInventory = _mapper.Map<ProductInventory>(productReq);
        productInventory.Id = uuid;
        await _context.ProductInventories.AddAsync(productInventory);

        var productDetail = _mapper.Map<ProductDetail>(productReq);
        productDetail.Id = uuid;
        productDetail.Category = category.Name;

        ProductDetailBase specificDetail;

        switch (category.Name)
        {
            case "Smartphone":
            specificDetail = ManualMapping.ToSpecificDetail<Smartphone>(productReq.Details);
            productDetail.SpecificDetails = specificDetail;
            break;
            case "Fashion":
            specificDetail = ManualMapping.ToSpecificDetail<Fashion>(productReq.Details);
            productDetail.SpecificDetails = specificDetail;
            break;
            case "Electronic":
            specificDetail = ManualMapping.ToSpecificDetail<Electronic>(productReq.Details);
            productDetail.SpecificDetails = specificDetail;
            break;
        }
        if (_productDetailCollection == null)
            return StatusCode(500);

        await _productDetailCollection.InsertOneAsync(productDetail);

        await _context.SaveChangesAsync();

        Product product= new();

        return CreatedAtAction(nameof(GetByID), new { id = uuid }, product);
    }
}

