namespace CacheInvalidation.Api.Notification
{
    public interface INotificationHandler<TNotification>
    {
        Task HandleAsync(TNotification notification, CancellationToken cancellationToken = default);
    }
}
