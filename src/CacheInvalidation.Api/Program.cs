using CacheInvalidation.Api.Database;
using CacheInvalidation.Api.Dtos;
using CacheInvalidation.Api.Repositories;
using CacheInvalidation.Api.UseCases;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

builder.Services.AddScoped<IProductRepository, ProductDatabaseRepository>();
builder.Services.AddScoped<ICacheDatabase, RedisCacheDatabase>();
builder.Services.AddScoped<CreateProduct>();

builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")));


app.MapPost("/api/products", async (ProductDto record, CreateProduct createProduct) =>
{
    var response = await createProduct.Execute(record);
    return Results.Created(string.Empty, response);
});

app.Run();

