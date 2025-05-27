using MediatR;
using Scalar2Sln_Application.Common.Interfaces;
using Scalar2Sln_Application.Handler.CreateTodoList;
using Scalar2Sln_Domain.Entities.TodoList;
using Scalar2Sln_Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scalar2Sln_Application.Handler.CreateTodoList
{
    public record CreateTodoListCommand : IRequest<int>
    {
        public string? Title { get; init; }
        public List<CreateTodoItemDto>? Items { get; init; }
    }


    public record CreateTodoItemDto
    {
        public string? Title { get; init; }
        public string? Note { get; init; }
        public int Priority { get; init; }
        public DateTime? Reminder { get; init; }
        public bool Done { get; init; }
    }
}

public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IEmailSender _emailSender;

    public CreateTodoListCommandHandler(IApplicationDbContext context, IEmailSender emailSender)
    {
        _context = context;
        _emailSender = emailSender;
    }

    public async Task<int> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoList();

        entity.Title = request.Title;
        entity.Items = request!.Items!.Count > 0
            ? request.Items.Select(item => new TodoItem()
            {
                Note = item.Note,
                Title = item.Title,
                Priority = (PriorityLevel)item.Priority,
                Reminder = item.Reminder,
                Done = item.Done

            }).ToList() : new List<TodoItem>();

        _context.TodoLists.Add(entity);


        _emailSender.SendEmail("开始发送邮件");

        //entity.AddDomainEvent(new TodoListCreatedEvent(entity));
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
