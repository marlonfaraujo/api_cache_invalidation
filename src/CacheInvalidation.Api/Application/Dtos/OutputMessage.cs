namespace CacheInvalidation.Api.Application.Dtos
{
    public record OutputMessage(Guid Id, string Type, string Payload, DateTime CreatedAt, bool Processed = false)
    { 
    }
}
