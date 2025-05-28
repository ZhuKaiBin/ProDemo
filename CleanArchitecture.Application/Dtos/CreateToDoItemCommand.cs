using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.DTOs
{
    public class CreateToDoItemCommand : IRequest<int>
    {
        public required string Description { get; set; }
    }
}
