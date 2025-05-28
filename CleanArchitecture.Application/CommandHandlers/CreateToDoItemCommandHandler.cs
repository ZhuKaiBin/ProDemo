using CleanArchitecture.Application.Common.Enums;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Application.Interfaces.Persistence;
using CleanArchitecture.Application.Interfaces.UseCases.Orders;
using CleanArchitecture.Domian.Entities;
using CleanArchitecture.Domian.Events;

namespace CleanArchitecture.Application.CommandHandlers
{
    public class CreateToDoItemCommandHandler : IRequestHandler<CreateToDoItemRequestDto, int>
    {
        private readonly IOrderServices _orderServices;

        public CreateToDoItemCommandHandler(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }





        public async Task<int> Handle(CreateToDoItemRequestDto request, CancellationToken cancellationToken)
        {
            var item = new ToDoItem
            {
                Description = request.Description,              
            };

          
            //当创建一个新的待办事项的时候，可以通知其他的领域事件
            item.AddDomainEvent(new ToDoItemCreatedEvent(item));


           var ret = await  _orderServices.CreateAsync(item);

            return ret;
        }
    }
}
