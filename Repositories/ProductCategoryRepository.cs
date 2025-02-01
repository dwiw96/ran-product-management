using Microsoft.EntityFrameworkCore;
using ran_product_management_net.Database.Postgresql;
using ran_product_management_net.Database.Postgresql.Models;
using ran_product_management_net.Utils;
using NotImplementedException = ran_product_management_net.Utils.NotImplementedException;

namespace ran_product_management_net.Repositories;

public class ProductCategoryRepository(ApplicationDbContext context) : IProductRepository<ProductCategory>
{
    private readonly ApplicationDbContext _context = context;
    
    public async Task<ProductCategory> GetProductByIdAsync(int? id)
    {
        var result = await _context.ProductCategories
            .AsNoTracking()
            .Where(t => t.DeletedAt == null)
            .FirstOrDefaultAsync(t => t.Id == id);
        if (result == null)
            throw new NotFoundException("category not found");
        
        return result;
    }
    public Task<ProductCategory> GetProductByIdAsync(Guid uuid)
    {
        throw new NotImplementedException("still not created.");
    }
    public Task<List<ProductCategory>> ListProductsAsync()
    {
        throw new NotImplementedException("still not created.");
    }
    public Task<ProductCategory> AddProductAsync(ProductCategory productCategory)
    {
        throw new NotImplementedException("still not created.");
    }
    public Task<ProductCategory> GetAllProductByNameAsync(string categoryName)
    {
        throw new NotImplementedException("still not created.");
    }
    
    public Task UpdateProductAsync(ProductCategory arg)
    {
        throw new NotImplementedException("still not created.");
    }
}