namespace CacheInvalidation.Api.Application.Dtos
{
    public record ProductResultDto(string Id, string Name, string Description, string Status, decimal Price, DateTime CreatedAt, DateTime? UpdatedAt)
    {
    }
}
