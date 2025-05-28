using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.DTOs
{
    public class CreateToDoItemRequestDto : IRequest<int>
    {
        public required string Description { get; set; }

        [DefaultValue("12345")]
        public required string Title { get; set; }
    }

    public class CreateToDoItemRequestDtoValidator : AbstractValidator<CreateToDoItemRequestDto>
    {
        public CreateToDoItemRequestDtoValidator()
        {
            RuleFor(v => v.Title)
             .MaximumLength(5).WithMessage("Title太长，最多不能超过5个字符")
             .NotEmpty().WithMessage("Title不能为空");

        }
    }

}
