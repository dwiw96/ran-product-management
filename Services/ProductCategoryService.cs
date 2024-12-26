using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ran_product_management_net.Database.Postgresql;
using ran_product_management_net.Database.Postgresql.Models;
using ran_product_management_net.Models.DTOs.Request;
using ran_product_management_net.Models.DTOs.Response;
using ran_product_management_net.Utils;

namespace  ran_product_management_net.Services;

public class ProductCategoryService(ApplicationDbContext context, IMapper mapper)
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<List<ProductCategoryResp>> ListCategories()
    {
        var dbRes = await _context.ProductCategories
            .Where(t => t.DeletedAt == null)
            .OrderBy(t => t.Id)
            .ToListAsync();
        
        return _mapper.Map<List<ProductCategoryResp>>(dbRes);
    }

    public async Task<ProductCategoryResp?> GetCategoryById(int id)
    {
        var dbRes = await _context.ProductCategories
            .AsNoTracking()
            .Where(t => t.DeletedAt == null)
            .FirstOrDefaultAsync(t => t.Id == id);
        if (dbRes == null)
            throw new NotFoundException("category not found");
        
        return _mapper.Map<ProductCategoryResp>(dbRes);
    }

    public async Task<ProductCategoryResp> CreateProductCategory(CreateProductCategoryReq arg)
    {
        var category = _mapper.Map<ProductCategory>(arg);
        var dbRes = await _context.ProductCategories
            .AddAsync(category);
        await _context.SaveChangesAsync();
        return _mapper.Map<ProductCategoryResp>(dbRes.Entity);
    }

    public async Task UpdateCategory(int id, UpdateProductCategoryReq reqBody)
    {
        var rowsUpdatedNumber = await _context.ProductCategories
            .Where(t => t.Id == id)
            .ExecuteUpdateAsync(setters =>
                setters
                    .SetProperty(t => t.Name, t => reqBody.Name ?? t.Name)
                    .SetProperty(t => t.Desc, t => reqBody.Desc ?? t.Desc)
                    .SetProperty(t => t.ModifiedAt, t => DateTime.Now));
        await _context.SaveChangesAsync();

        if (rowsUpdatedNumber == 0)
        {
            throw new DatabaseCrudFailedException("no data updated");
        }
    }

    public async Task DeleteCategory(int id)
    {
        try {
            var rowsUpdatedNumber = await _context.ProductCategories
                .Where(t => t.Id == id)
                .Where(t => t.DeletedAt == null)
                .ExecuteUpdateAsync(setters => setters.SetProperty(t =>
                    t.DeletedAt, DateTime.Now));

            await _context.SaveChangesAsync();

            if (rowsUpdatedNumber == 0)
            {
                throw new DatabaseCrudFailedException("no data deleted");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}
