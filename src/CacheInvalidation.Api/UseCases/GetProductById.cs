using CacheInvalidation.Api.Entities;
using CacheInvalidation.Api.Repositories;

namespace CacheInvalidation.Api.UseCases
{
    public class GetProductById
    {
        private readonly IProductRepository _repository;

        public GetProductById(IProductRepository repository)
        {
            this._repository = repository;
        }

        public async Task<Product> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var product = await this._repository.GetByIdAsync(id, cancellationToken);
            if (product == null)
            {
                throw new Exception("Product not found with id: " + id.ToString());
            }
            return product;
        }
    }
}
