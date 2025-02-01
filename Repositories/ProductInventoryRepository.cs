using Microsoft.EntityFrameworkCore;
using ran_product_management_net.Database.Postgresql;
using ran_product_management_net.Database.Postgresql.Models;
using ran_product_management_net.Models.DTOs.Request;
using ran_product_management_net.Utils;
using NotImplementedException = ran_product_management_net.Utils.NotImplementedException;

namespace ran_product_management_net.Repositories;

public class ProductInventoryRepository(ApplicationDbContext context) : IProductRepository<ProductInventory>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<ProductInventory>> ListProductsAsync()
    {
        var results = await _context.ProductInventories
            .Where(t => t.DeletedAt == null)
            .OrderBy(t => t.Id)
            .ToListAsync();

        if (results.Count == 0)
        {
            Console.WriteLine("There are no products inventories in the database.");
            throw new NotFoundException("products are empty");
        }

        return results;
    }

    public async Task<ProductInventory> GetProductByIdAsync(Guid uuid)
    {
        var result = await _context.ProductInventories
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == uuid);
        if (result == null)
        {
            Console.WriteLine("There is no products inventories in the database.");
            throw new NotFoundException($"Product with id {uuid} was not found");
        }

        return result;
    }

    public async Task<ProductInventory> AddProductAsync(ProductInventory arg)
    {
        var inventoryResult = await _context.ProductInventories.AddAsync(arg);
        if (inventoryResult == null)
            throw new NotNullException("Product inventories are null");
        
        await _context.SaveChangesAsync();

        return inventoryResult.Entity;
    }

    public async Task UpdateProductAsync(ProductInventory arg)
    {
        var rowsUpdatedNumber = await _context.ProductInventories
            .Where(t => t.Id == arg.Id)
            .ExecuteUpdateAsync(setters =>
                setters
                    .SetProperty(t => t.Price, t => arg.Price ?? t.Price)
                    .SetProperty(t => t.Stock, t => arg.Stock ?? t.Stock)
                    .SetProperty(t => t.MinBuy, t => arg.MinBuy ?? t.MinBuy)
                    .SetProperty(t => t.Condition, t => arg.Condition ?? t.Condition)
                    .SetProperty(t => t.Status, t => arg.Status ?? t.Status)
                    .SetProperty(t => t.CategoryId, t => arg.CategoryId ?? t.CategoryId)
                    .SetProperty(t => t.ModifiedAt, t => DateTime.Now));
        
        await _context.SaveChangesAsync();
    }
    
    public Task<ProductInventory?> GetAllProductByNameAsync(string name)
    {
        throw new NotImplementedException("Method not created yet.");
    }
    public Task<ProductInventory> GetProductByIdAsync(int? id)
    {
        throw new NotImplementedException("Method not created yet.");
    }
}
