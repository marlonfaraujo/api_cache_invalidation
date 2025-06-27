using CacheInvalidation.Api.Application.Dtos;
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

        public async Task<IEnumerable<ProductResultDto>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var products = await _repository.GetAsync(cancellationToken);
            return products.Select(product => new ProductResultDto(
                product.Id.ToString(), 
                product.Name, 
                product.Description, 
                product.Status, 
                product.Price.Value, 
                product.CreatedAt, 
                product.UpdatedAt
                ));
        }
    }
}
