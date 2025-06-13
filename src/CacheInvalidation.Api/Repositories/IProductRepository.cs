using CacheInvalidation.Api.Entities;

namespace CacheInvalidation.Api.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAsync();
        Task CreateAsync(Product product);
        Task UpdateAsync(Guid id, Product product);
    }
}
