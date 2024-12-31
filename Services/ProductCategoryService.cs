using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Npgsql;
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
        if (dbRes.Count == 0)
            throw new NotFoundException("category is empty");
        
        return _mapper.Map<List<ProductCategoryResp>>(dbRes);
    }

    public async Task<ProductCategoryResp> GetCategoryById(int id)
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
        EntityEntry<ProductCategory> dbRes;
        try
        {
            var category = _mapper.Map<ProductCategory>(arg);
            dbRes = await _context.ProductCategories
                .AddAsync(category);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException p)
        {
            switch (p.SqlState)
            {
                case "23502":
                    throw new NotNullException(p.MessageText);
                case "23505":
                    throw new DuplicateDataException(p.MessageText);
                case "22001":
                    throw new LengthException(p.MessageText);
                default:
                    throw;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("exception: " + e.Message);
            throw;
        }

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
    
    public async Task<(bool isDeleted, int categoryId)> GetAllCategoryByName(string name)
    {
        var dbRes = await _context.ProductCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Name == name);
        if (dbRes == null)
            return (false, 0);
        if (dbRes.DeletedAt == null)
            throw new DuplicateDataException("category name must be unique to each other category");
        
        return (true, dbRes.Id);
    }
    
    public async Task RecreateCategory(CreateProductCategoryReq arg)
    {
        try
        {
            var rowsUpdatedNumber = await _context.ProductCategories
                .Where(t => t.Name == arg.Name)
                .Where(t => t.DeletedAt != null)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(t => t.Desc, arg.Desc)
                    .SetProperty(t => t.DeletedAt, (DateTime?)null)
                    .SetProperty(t => t.ModifiedAt, DateTime.Now));
            
            await _context.SaveChangesAsync();
            
            if (rowsUpdatedNumber == 0)
            {
                throw new DatabaseCrudFailedException("failed to create category");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}
