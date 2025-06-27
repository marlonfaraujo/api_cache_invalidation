using CacheInvalidation.Api.Application.Dtos;
using CacheInvalidation.Api.Domain.Repositories;

namespace CacheInvalidation.Api.Application.UseCases
{
    public class GetProductById
    {
        private readonly IProductRepository _repository;

        public GetProductById(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductResultDto> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var product = await _repository.GetByIdAsync(id, cancellationToken);
            if (product == null)
            {
                throw new Exception("Product not found with id: " + id.ToString());
            }
            return new ProductResultDto(
                    product.Id.ToString(),
                    product.Name,
                    product.Description,
                    product.Status,
                    product.Price.Value,
                    product.CreatedAt,
                    product.UpdatedAt
                );
        }
    }
}
