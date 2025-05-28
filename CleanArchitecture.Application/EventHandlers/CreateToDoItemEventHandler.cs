using CleanArchitecture.Domian.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.EventHandlers
{
    public class CreateToDoItemEventHandler: INotificationHandler<ToDoItemCreatedEvent>
    {
        private readonly ILogger<CreateToDoItemEventHandler> _logger;

        public CreateToDoItemEventHandler(ILogger<CreateToDoItemEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(ToDoItemCreatedEvent notification, CancellationToken cancellationToken)
        {
            //第三步：触发领域事件：ToDoItemCreatedEvent
            var para = notification._item;

            _logger.LogInformation("触发了领域事件_CleanArchitectureSolutionTemplate Domain Event: {DomainEvent}", notification.GetType().Name);


            return Task.CompletedTask;
        }
    }
}
