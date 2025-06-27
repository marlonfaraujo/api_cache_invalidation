using CacheInvalidation.Api.Domain.Entities;

namespace CacheInvalidation.Api.Infra.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetAsync(CancellationToken cancellationToken = default);
        Task CreateAsync(Product product, CancellationToken cancellationToken = default);
        Task UpdateAsync(Guid id, Product product, CancellationToken cancellationToken = default);
    }
}
