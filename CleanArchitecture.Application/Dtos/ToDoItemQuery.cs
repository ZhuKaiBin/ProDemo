using CleanArchitecture.Domian.Entities;
using MediatR;

namespace CleanArchitecture.Application.DTOs
{
    public class ToDoItemQuery : IRequest<List<ToDoItem>>
    {
    }
}
