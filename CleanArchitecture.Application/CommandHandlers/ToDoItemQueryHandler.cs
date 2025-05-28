using CleanArchitecture.Application.DTOs;
using MediatR;
using CleanArchitecture.Application.Interfaces.Persistence;
using CleanArchitecture.Domian.Entities;

namespace CleanArchitecture.Application.CommandHandlers
{
    public class ToDoItemQueryHandler(IToDoRepository<ToDoItem> toDoRepository) : IRequestHandler<ToDoItemQuery, List<ToDoItem>>
    {

        public Task<List<ToDoItem>> Handle(ToDoItemQuery request, CancellationToken cancellationToken)
        {
            return toDoRepository.GetAllAsync();
        }
    }
}
