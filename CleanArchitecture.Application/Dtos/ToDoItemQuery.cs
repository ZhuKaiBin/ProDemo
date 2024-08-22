using MediatR;

namespace CleanArchitecture.Application.Dtos
{
    public class ToDoItemQuery : IRequest<List<Domian.ToDoItem>>
    {
    }
}
