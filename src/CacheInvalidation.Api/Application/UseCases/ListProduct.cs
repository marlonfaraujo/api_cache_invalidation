using CacheInvalidation.Api.Domain.Entities;
using CacheInvalidation.Api.Domain.Repositories;

namespace CacheInvalidation.Api.Application.UseCases
{
    public class ListProduct
    {
        private readonly IProductRepository _repository;

        public ListProduct(IProductRepository repository)
        {
            _repository = repository;  
        }

        public async Task<IEnumerable<Product>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var products = await _repository.GetAsync(cancellationToken);
            return products;
        }
    }
}
