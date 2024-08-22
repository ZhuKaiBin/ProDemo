using CleanArchitecture.Application.Dtos;
using CleanArchitecture.Domian;
using MediatR;

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


            return toDoRepository.CreateAsync(item);
        }
    }
}
