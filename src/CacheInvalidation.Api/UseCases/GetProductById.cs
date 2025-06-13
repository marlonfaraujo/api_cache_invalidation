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
            return await this._repository.GetByIdAsync(id, cancellationToken);
        }
    }
}
