using CacheInvalidation.Api.Dtos;
using CacheInvalidation.Api.Entities;
using CacheInvalidation.Api.Repositories;

namespace CacheInvalidation.Api.UseCases
{
    public class CreateProduct
    {
        private readonly IProductRepository _repository;

        public CreateProduct(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Product> Execute(ProductDto dto) 
        {
            var product = new Product(dto.Name, dto.Description, dto.Price);
            await this._repository.CreateAsync(product);
            return product;
        }
    }
}
