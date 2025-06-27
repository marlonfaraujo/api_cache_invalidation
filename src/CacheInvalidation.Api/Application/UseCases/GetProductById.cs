using CacheInvalidation.Api.Domain.Entities;
using CacheInvalidation.Api.Infra.Repositories;

namespace CacheInvalidation.Api.Application.UseCases
{
    public class GetProductById
    {
        private readonly IProductRepository _repository;

        public GetProductById(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Product> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var product = await _repository.GetByIdAsync(id, cancellationToken);
            if (product == null)
            {
                throw new Exception("Product not found with id: " + id.ToString());
            }
            return product;
        }
    }
}
