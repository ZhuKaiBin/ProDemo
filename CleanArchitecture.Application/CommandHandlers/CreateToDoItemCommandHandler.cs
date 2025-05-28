using CleanArchitecture.Application.DTOs;
using MediatR;
using CleanArchitecture.Application.Interfaces.Persistence;
using CleanArchitecture.Domian.Entities;
using CleanArchitecture.Domian.Events;

namespace CleanArchitecture.Application.CommandHandlers
{
    public class CreateToDoItemCommandHandler(IToDoRepository<ToDoItem> toDoRepository) : IRequestHandler<CreateToDoItemCommand, int>
    {
       
     




        public Task<int> Handle(CreateToDoItemCommand request, CancellationToken cancellationToken)
        {
            var item = new ToDoItem
            {
                Description = request.Description
            };

            //当创建一个新的待办事项的时候，可以通知其他的领域事件
            item.AddDomainEvent(new ToDoItemCreatedEvent(item));

            return toDoRepository.CreateAsync(item);
        }
    }
}
