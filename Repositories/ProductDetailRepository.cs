using AutoMapper;
using MongoDB.Driver;
using ran_product_management_net.Database.Mongodb;
using ran_product_management_net.Database.Mongodb.Models;
using ran_product_management_net.Utils;
using NotImplementedException = ran_product_management_net.Utils.NotImplementedException;

namespace ran_product_management_net.Repositories;

public class ProductDetailRepository(MongoDBService mongoDbService, IMapper mapper) : IProductRepository<ProductDetail>
{
    private readonly IMongoCollection<ProductDetail>? _productDetailCollection = mongoDbService.Database?.GetCollection<ProductDetail>("product_details");
    private readonly IMapper _mapper = mapper;

    public async Task<List<ProductDetail>> ListProductsAsync()
    {
        var filter = Builders<ProductDetail>.Filter
            .Eq(t => t.DeletedAt, null);

        var results = await _productDetailCollection.Find(filter)
            .SortBy(t => t.Id)
            .ToListAsync();
        if (results == null)
        {
            Console.WriteLine("There are no products details in the database.");
            throw new NotFoundException("products are empty");
        }

        return results;
    }

    public async Task<ProductDetail> GetProductByIdAsync(Guid uuid)
    {
        var filter = Builders<ProductDetail>.Filter.And(
            Builders<ProductDetail>.Filter.Eq(c => c.Id, uuid),
            Builders<ProductDetail>.Filter.Eq(c => c.DeletedAt, null));
        var result = await _productDetailCollection.Find(filter).FirstOrDefaultAsync();
        if (result == null)
        {
            Console.WriteLine("There is no products details in the mongodb database.");
            throw new NotFoundException($"Product with id {uuid} was not found");
        }
        
        return result;
    }

    public async Task<ProductDetail> AddProductAsync(ProductDetail arg)
    {
        if (_productDetailCollection == null)
        {
            Console.WriteLine("not connected with mongodb database");
            throw new ServerErrorException("not connected with mongodb database");
        }

        await _productDetailCollection.InsertOneAsync(arg);

        return arg;
    }

    public async Task<ProductDetail?> GetAllProductByNameAsync(string name)
    {
        var filter = Builders<ProductDetail>.Filter.And(
            Builders<ProductDetail>.Filter.Eq(c => c.ProductName, name));
        
        var result = await _productDetailCollection.Find(filter).FirstOrDefaultAsync();
        
        return result;
    }

    public async Task UpdateProductAsync(ProductDetail arg)
    {
        if (_productDetailCollection == null)
        {
            Console.WriteLine("not connected with mongodb database");
            throw new ServerErrorException("not connected with mongodb database");
        }
        
        // 1. Create an update definition builder for Product type
        var updateDefBuilder = Builders<ProductDetail>.Update;

        // 2. Get all properties of Product class using reflection
        var updates = typeof(ProductDetail)
            // Get all properties
            .GetProperties()
            // Filter out "Id" property and null values
            .Where(p => p.Name != "Id" && p.GetValue(arg) != null)
            // Create Set operations for each non-null property
            .Select(p => updateDefBuilder.Set(p.Name, p.GetValue(arg)));

        // 3. If there are any updates, combine them and execute
        if (updates.Any())
        {
            var combinedUpdate = updateDefBuilder.Combine(updates);
            await _productDetailCollection.UpdateOneAsync(
                p => p.Id == arg.Id,
                combinedUpdate
            );
        }
        
        // var filter = Builders<ProductDetail>.Filter.Eq(c => c.Id, arg.Id);
        // var update = Builders<ProductDetail>.Update
        //     .Set(c => c.ProductName, arg.ProductName)
        //     .Set(c => c.Desc, arg.Desc)
        //     .Set(c => c.Brand, arg.Brand)
        //     .Set(c => c.Model, arg.Model)
        //     .Set(c => c.Category, arg.Category);
        // var res = await _productDetailCollection.UpdateOneAsync(filter, update);
    }
    public Task<ProductDetail> GetProductByIdAsync(int? id)
    {
        throw new NotImplementedException("Method not created yet.");
    }
}
