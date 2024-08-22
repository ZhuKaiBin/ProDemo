using CleanArchitecture.Application.Dtos;
using CleanArchitecture.Domian;
using MediatR;

namespace CleanArchitecture.Application.CommandHandlers
{
    public class ToDoItemQueryHandler(IToDoRepository<ToDoItem> toDoRepository) : IRequestHandler<ToDoItemQuery, List<Domian.ToDoItem>>
    {

        public Task<List<Domian.ToDoItem>> Handle(ToDoItemQuery request, CancellationToken cancellationToken)
        {
            return toDoRepository.GetAllAsync();
        }
    }
}
