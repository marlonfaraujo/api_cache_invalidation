using CacheInvalidation.Api.Database;
using CacheInvalidation.Api.Dtos;
using CacheInvalidation.Api.Events;
using CacheInvalidation.Api.Middlewares;
using CacheInvalidation.Api.Notification;
using CacheInvalidation.Api.Repositories;
using CacheInvalidation.Api.UseCases;
using CacheInvalidation.Api.UseCases.Handlers;
using StackExchange.Redis;

public partial class Program 
{
    public static WebApplication BuildApp(string[]? args = null)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddScoped<IDatabaseConnection, PostgresDatabaseConnection>();
        builder.Services.AddScoped<IProductRepository, ProductDatabaseRepository>();
        builder.Services.AddScoped<ICacheDatabase, RedisCacheDatabase>();
        builder.Services.AddScoped<ActiveProduct>();
        builder.Services.AddScoped<CreateProduct>();
        builder.Services.AddScoped<DisableProduct>();
        builder.Services.AddScoped<GetProductById>();
        builder.Services.AddScoped<ListProduct>();
        builder.Services.AddScoped<ListCachedProduct>();
        builder.Services.AddScoped<UpdateProduct>();
        builder.Services.AddTransient<INotificationHandler<ProductActivedEvent>, ProductActivedEventHandler>();
        builder.Services.AddTransient<INotificationHandler<ProductCreatedEvent>, ProductCreatedEventHandler>();
        builder.Services.AddTransient<INotificationHandler<ProductDisabledEvent>, ProductDisabledEventHandler>();
        builder.Services.AddTransient<INotificationHandler<ProductUpdatedEvent>, ProductUpdatedEventHandler>();
        builder.Services.AddTransient<NotificationPublisher<ProductActivedEvent>>();
        builder.Services.AddTransient<NotificationPublisher<ProductCreatedEvent>>();
        builder.Services.AddTransient<NotificationPublisher<ProductDisabledEvent>>();
        builder.Services.AddTransient<NotificationPublisher<ProductUpdatedEvent>>();

        builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")));

        var app = builder.Build();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        
        app.MapGet("/api/products", async (ListProduct listProduct, CancellationToken cancellationToken) =>
        {
            var response = await listProduct.ExecuteAsync(cancellationToken);
            return Results.Ok(response);
        });

        app.MapGet("/api/products-cache", async (ListCachedProduct listCachedProduct, CancellationToken cancellationToken) =>
        {
            var response = await listCachedProduct.ExecuteAsync(cancellationToken);
            return Results.Ok(response);
        });

        app.MapGet("/api/products/{id}", async (Guid id, GetProductById getProductById, CancellationToken cancellationToken) =>
        {
            var response = await getProductById.ExecuteAsync(id, cancellationToken);
            return Results.Ok(response);
        });

        app.MapPost("/api/products", async (ProductDto record, CreateProduct createProduct, CancellationToken cancellationToken) =>
        {
            var response = await createProduct.ExecuteAsync(record, cancellationToken);
            return Results.Created(string.Empty, response);
        });

        app.MapPut("/api/products/{id}", async (Guid id, ProductDto record, UpdateProduct updateProduct, CancellationToken cancellationToken) =>
        {
            var response = await updateProduct.ExecuteAsync(id, record, cancellationToken);
            return Results.Ok(response);
        });

        app.MapPost("/api/products/{id}/disable", async (Guid id, DisableProduct disableProduct, CancellationToken cancellationToken) =>
        {
            await disableProduct.ExecuteAsync(id, cancellationToken);
            return Results.Ok();
        });

        app.MapPost("/api/products/{id}/active", async (Guid id, ActiveProduct activeProduct, CancellationToken cancellationToken) =>
        {
            await activeProduct.ExecuteAsync(id, cancellationToken);
            return Results.Ok();
        });

        return app;
    }

    public static void Main(string[] args)
    {
        BuildApp(args).Run();
    }
}
