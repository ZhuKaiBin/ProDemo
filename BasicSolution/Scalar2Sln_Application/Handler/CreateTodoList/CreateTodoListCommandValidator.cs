using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scalar2Sln_Application.Handler.CreateTodoList
{
    public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateTodoListCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Title)
                .NotEmpty()
                .MaximumLength(200)
                .MustAsync(BeUniqueTitle)
                    .WithMessage("'{PropertyName}' must be unique.")
                    .WithErrorCode("Unique");
        }

        public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
        {
            return !await _context.TodoLists
                .AnyAsync(l => l.Title == title, cancellationToken);
        }
    }
}
