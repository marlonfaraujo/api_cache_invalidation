using CacheInvalidation.Api.Entities;
using CacheInvalidation.Api.Repositories;

namespace CacheInvalidation.Api.UseCases
{
    public class ListProduct
    {
        private readonly IProductRepository _repository;

        public ListProduct(IProductRepository repository)
        {
            this._repository = repository;  
        }

        public async Task<IEnumerable<Product>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var products = await this._repository.GetAsync(cancellationToken);
            return products;
        }
    }
}
