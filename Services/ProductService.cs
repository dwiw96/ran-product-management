using AutoMapper;
using ran_product_management_net.Database.Mongodb;
using ran_product_management_net.Database.Mongodb.Models;
using ran_product_management_net.Database.Postgresql;
using ran_product_management_net.Database.Postgresql.Models;
using ran_product_management_net.Models.DTOs.Request;
using ran_product_management_net.Models.DTOs.Response;
using ran_product_management_net.Models.Mapping;
using ran_product_management_net.Repositories;
using ran_product_management_net.Utils;

namespace ran_product_management_net.Services;

public class ProductService(ApplicationDbContext context, MongoDBService mongoDbService, IMapper mapper, IProductRepository<ProductCategory> iCategory, IProductRepository<ProductInventory> iInventory, IProductRepository<ProductDetail> iDetail)
{
    private readonly IMapper _mapper = mapper;
    private readonly IProductRepository<ProductCategory> _categoryRepository = iCategory;
    private readonly IProductRepository<ProductInventory> _inventoryRepository = iInventory;
    private readonly IProductRepository<ProductDetail> _detailRepository = iDetail;

    public async Task<List<ProductResp>> ListAllProducts()
    {
        var productInventory = await _inventoryRepository.ListProductsAsync();
        var productDetail = await _detailRepository.ListProductsAsync();
        
        return ManualMapping.ToProductResponse(productInventory, productDetail, _mapper);
    }

    public async Task<ProductResp?> GetProductById(string productId)
    {
        var uuid = Guid.Parse(productId);
        var productInventory = await _inventoryRepository.GetProductByIdAsync(uuid);
        var productDetail = await _detailRepository.GetProductByIdAsync(uuid);

        var result = ManualMapping.ToProductResponse(productInventory, productDetail, _mapper);
        if (result == null)
            throw new NotFoundException("Product not found");
        return result;
    }

    public async Task<string> CreateProduct(CreateProductReq arg)
    {
        // get the product category
        var category = await _categoryRepository.GetProductByIdAsync(arg.CategoryId);
        if (category == null)
            throw new NotFoundException($"Category with id {arg.CategoryId} was not found");
        
        // check if the product inventory has created
        var getDetailRes = await _detailRepository.GetAllProductByNameAsync(arg.ProductName);
        if (getDetailRes != null)
        {
            if (getDetailRes.DeletedAt != null)
                throw new DuplicateDataException("product with name: " + arg.ProductName + " already deleted");
            if (getDetailRes.DeletedAt == null)
                throw new DuplicateDataException("product with name: " + arg.ProductName + " already exists");
        }

        // create the uuid for product
        var uuid = Guid.NewGuid();
        Console.WriteLine("uuid: " + uuid);
        
        // prepare arg for product detail
        var productDetail = _mapper.Map<ProductDetail>(arg);
        productDetail.Id = uuid;
        productDetail.Category = category.Name;

        ProductDetailBase specificDetail = new();
        switch (category.Name)
        {
            case "Smartphone":
                specificDetail = ManualMapping.ToSpecificDetail<Smartphone>(arg.Details);
                break;
            case "Fashion":
                specificDetail = ManualMapping.ToSpecificDetail<Fashion>(arg.Details);
                break;
            case "Electronic":
                specificDetail = ManualMapping.ToSpecificDetail<Electronic>(arg.Details);
                break;
        }
        productDetail.SpecificDetails = specificDetail;
        
        // add product details and inventory
        await _detailRepository.AddProductAsync(productDetail);
        
        // add product inventory
        var productInventory = _mapper.Map<ProductInventory>(arg);
        productInventory.Id = uuid;
        await _inventoryRepository.AddProductAsync(productInventory);

        return uuid.ToString();
    }
}