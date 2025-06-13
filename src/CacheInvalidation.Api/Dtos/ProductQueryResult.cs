namespace CacheInvalidation.Api.Dtos
{
    public record ProductQueryResult(Guid Id, string Name, string Description, string Status, decimal Price, DateTime CreatedAt, DateTime? UpdatedAt)
    {
    }
}
