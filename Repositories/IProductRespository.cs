using ran_product_management_net.Database.Mongodb.Models;
using ran_product_management_net.Database.Postgresql.Models;
using ran_product_management_net.Models.Integration;

namespace ran_product_management_net.Repositories;

public interface IProductRepository<T> where T : AuditableEntity
{
    Task<List<T>> ListProductsAsync();
    Task<T> GetProductByIdAsync(Guid uuid);
    Task<T> AddProductAsync(T product);
    Task<T?> GetAllProductByNameAsync(string name);
    Task<T> GetProductByIdAsync(int id);
}
