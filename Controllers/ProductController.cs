using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ran_product_management_net.Database.Mongodb;
using ran_product_management_net.Database.Mongodb.Models;
using ran_product_management_net.Database.Postgresql;
using ran_product_management_net.Database.Postgresql.Models;
using ran_product_management_net.Models.DTOs.Request;
using ran_product_management_net.Models.DTOs.Response;
using ran_product_management_net.Services;
using ran_product_management_net.Utils;
using System.ComponentModel.DataAnnotations;
using ran_product_management_net.Repositories;

namespace ran_product_management_net.Controllers;

[Route("api/v1/product")]
[ApiController]
public class ProductController(ApplicationDbContext context, MongoDBService mongoDbService, IMapper mapper, IProductRepository<ProductCategory> iCategory, IProductRepository<ProductInventory> iInventory, IProductRepository<ProductDetail> iDetail) : ControllerBase
{
    private readonly IMongoCollection<ProductDetail>? _productDetailCollection = mongoDbService.Database?.GetCollection<ProductDetail>("product_details");
    private readonly IMapper _mapper = mapper;
    private readonly ProductService _service = new(context, mongoDbService, mapper, iCategory, iInventory, iDetail);

    [HttpGet]
    public async Task<IActionResult> ListProducts()
    {
        SuccessWithData<List<ProductResp>> response;
        try
        {
           var results = await _service.ListAllProducts();
           response = new SuccessWithData<List<ProductResp>>(200, "List of products", results);
        }
        catch (NotFoundException e)
        {
            SuccessWithData<List<ProductResp>> emptyResp = new(200, "there are no products data in the database", []);
            return Ok(emptyResp);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            FailedResponse errResp = new(500,  "there is an error in the server", 
                new Dictionary<string, List<string>>()
                {
                    {"server", [e.Message] } 
                });
            return StatusCode(500, errResp);
        }
        
        return Ok(response);
    }

    [HttpGet("{id}")]
    
    public  async Task<IActionResult> GetById([FromRoute] [StringLength(36, MinimumLength = 36, ErrorMessage = "ID must be a valid GUID (36 characters).")] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        SuccessWithData<ProductResp> response;
        try
        {
            var result = await _service.GetProductById(id);
            
            response = new SuccessWithData<ProductResp>(200, "Product found", result);
        }
        catch (NotFoundException e)
        {
            FailedResponse errResp = new(404,  $"there is no product with id {id}", 
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
                    {"server", [e.Message] } 
                });
            return StatusCode(500, errResp);
        }
        
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductReq productReq)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        string productId;
        ProductResp? productCreated;

        try
        {
            productId = await _service.CreateProduct(productReq);
            productCreated = await _service.GetProductById(productId);
        }
        catch (NotFoundException e)
        {
            FailedResponse errResp = new(400, "product or category was not found",
                new Dictionary<string, List<string>>()
                {
                    { "message", [e.Message] }
                });

            return new BadRequestObjectResult(errResp);
        }
        catch (DuplicateDataException e)
        {
            FailedResponse errResp = new(400, "product with the same name is already in use",
                new Dictionary<string, List<string>>()
                {
                    { "product_name", [e.Message] }
                });
            return new BadRequestObjectResult(errResp);
        }
        catch (KeyNotFoundException e)
        {
            FailedResponse errResp = new(400, "unit must be either 'Metric' or 'Imperial'",
                new Dictionary<string, List<string>>()
                {
                    { "unit", [e.Message] }
                });
            return new BadRequestObjectResult(errResp);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            FailedResponse errResp = new(500,  "there is an error in the server", 
                new Dictionary<string, List<string>>()
                {
                    {"server", [e.Message] } 
                });
            return StatusCode(500, e.Message);
        }
        
        return CreatedAtAction(nameof(GetById), new { id = productId }, productCreated);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id)
    {
        var uuid = Guid.Parse(id);
        var filter = Builders<ProductDetail>.Filter.Eq(f => f.Id, uuid);
        
        if (_productDetailCollection == null)
            return StatusCode(500);
        
        await _productDetailCollection.ReplaceOneAsync(filter, _mapper.Map<ProductDetail>(uuid));

        return AcceptedAtAction(nameof(GetById), new { id = uuid }, _mapper.Map<ProductDetail>(uuid));
    }
}
