using CacheInvalidation.Api.Events;

namespace CacheInvalidation.Api.Notification
{
    public class NotificationPublisher<TNotification> where TNotification : IDomainEvent
    {
        private readonly IEnumerable<INotificationHandler<TNotification>> _handlers;

        public NotificationPublisher(IEnumerable<INotificationHandler<TNotification>> handlers)
        {
            this._handlers = handlers;
        }

        public async Task ExecuteAsync(TNotification notification, CancellationToken cancellationToken = default)
        {
            foreach (var handler in this._handlers)
            {
                await handler.HandleAsync(notification, cancellationToken);
            }
        }
    }

}
