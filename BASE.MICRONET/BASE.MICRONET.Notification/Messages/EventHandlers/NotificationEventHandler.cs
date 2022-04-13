using BASE.MICRONET.Cross.Event.Dir.Bus;
using BASE.MICRONET.Notification.Messages.Events;
using BASE.MICRONET.Notification.Services;
using System.Threading.Tasks;

namespace BASE.MICRONET.Notification.Messages.EventHandlers
{
    public class NotificationEventHandler : IEventHandler<NotificationCreatedEvent>
    {
        private readonly INotificationService _historyService;

        public NotificationEventHandler(INotificationService historyService)
        {
            _historyService = historyService;
        }

        public Task Handle(NotificationCreatedEvent @event)
        {
            _historyService.Add(new Models.SendMail()
            {
                AccountId = @event.AccountId,
                Address = "Corporation Address",
                Message = string.Concat("Withdrawal to ", @event.Amount),
                SendDate = @event.Timestamp.ToString(),
                Type = @event.Type

            });
            return Task.CompletedTask;
        }
    }
}
