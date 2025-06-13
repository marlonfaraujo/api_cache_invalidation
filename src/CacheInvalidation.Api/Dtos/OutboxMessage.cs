namespace CacheInvalidation.Api.Dtos
{
    public record OutboxMessage(Guid Id, string Type, string Payload, DateTime CreatedAt, bool Processed = false)
    { 
    }
}
