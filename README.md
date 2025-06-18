This repository contains some strategies for cache invalidation in the api using redis.

### Description:
I have an example endpoint that lists products.
```csharp
//class src/CacheInvalidation.Api/Program.cs
app.MapGet("/api/products-cache", async (ListCachedProduct listCachedProduct, CancellationToken cancellationToken) =>
{
   var response = await listCachedProduct.ExecuteAsync(cancellationToken);
   return Results.Ok(response);
});
```
On the first request, the data is fetched from the Postgres database and cached in Redis. For subsequent requests, the data is retrieved directly from Redis, which offers significantly faster read performance for being recorded in memory.
```csharp
//class src/CacheInvalidation.Api/UseCases/ListCachedProduct.cs
public async Task<IEnumerable<Product>> ExecuteAsync(CancellationToken cancellationToken = default)
{
   var cachedProducts = await this._cacheDatabase.GetAsync<IEnumerable<Product>>(this._cacheConfig.ProductCacheKey, cancellationToken);
   if (cachedProducts != null && cachedProducts.Any())
   {
       return cachedProducts;
   }

   var products = await this._repository.GetAsync(cancellationToken);
   if (products != null && products.Any())
   {
       await this._cacheDatabase.SetAsync(this._cacheConfig.ProductCacheKey, 
           products, cancellationToken, TimeSpan.FromMinutes(this._cacheConfig.ExpirationTimeMinutes));
       return products;
   }
   return Enumerable.Empty<Product>();
}
```
There are a few ways to invalidate the cache, the most common is to set an expiration time when writing to Redis. Other alternatively, is manually refresh the cache.

For example, when a new product is created, it does not yet exist in the cached data. To ensure the cache stays updated, a notification is published. This serves as a trigger to inform the cache that it needs to be refreshed with the new product.
```csharp
//class src/CacheInvalidation.Api/UseCases/CreateProduct.cs
public async Task<Product> ExecuteAsync(ProductDto dto, CancellationToken cancellationToken = default) 
{
   var product = new Product(dto.Name, dto.Description, dto.Price);
   await this._repository.CreateAsync(product, cancellationToken);
   var @event = product.CreateProductCreatedEvent();
   await this._notification.ExecuteAsync(@event, cancellationToken);
   return product;
}
```
In the event handler for product created, there are methods to either update or remove the cached data.
```csharp
//class src/CacheInvalidation.Api/UseCases/Handlers/ProductCreatedEventHandler.cs
public async Task HandleAsync(ProductCreatedEvent notification, CancellationToken cancellationToken = default)
{
   if (this._cacheConfig.ItsToRefresh.GetValueOrDefault() == true)
   {
       await this._refreshProductCache.ExecuteAsync(cancellationToken);
   }
   else
   {
       await this._cacheDatabase.RemoverAsync(this._cacheConfig.ProductCacheKey, cancellationToken);
   }
   var output = new OutboxMessage(notification.Product.Id, 
       nameof(ProductCreatedEvent).ToString(),
       JsonSerializer.Serialize(notification),
       DateTime.UtcNow,
       false);
   this._logger.LogInformation($"Product created event handled: {JsonSerializer.Serialize(output)}");
}
```
If the update strategy is used, refreshes the cache with the new product. If the removal strategy is used, the cache is cleared so that it can be repopulated on the next product listing request.
```csharp
//class src/CacheInvalidation.Api/UseCases/RefreshProductCache.cs
public async Task ExecuteAsync(CancellationToken cancellationToken = default)
{
   var products = await this._repository.GetAsync(cancellationToken);
   if (products != null && products.Any())
   {
       await this._cacheDatabase.SetAsync(this._cacheConfig.ProductCacheKey,
           products, cancellationToken, TimeSpan.FromMinutes(this._cacheConfig.ExpirationTimeMinutes));
   }
}
```
### Technologies and tools used:
* **Databases - Persistence**:
   1. [Redis](https://redis.io/) - Redis is a fast and versatile in-memory database ideal for caching;
   2. [PostgreSQL](https://www.postgresql.org/) - PostgreSQL was used for relational database;
   3. [Dapper](https://github.com/DapperLib/Dapper) - Dapper Micro ORM fast and lightweight mapping between C# objects and SQL queries.
* **Testing**:
   1. [XUnit](https://xunit.net/) - Unit tests to ensure code stability;
   2. [Bogus](https://github.com/bchavez/Bogus) - Generation of fake data in a simple and customizable way.
* **Container Technology**:
   1. [Docker](https://www.docker.com/) Containerizing the application to facilitate testing.
