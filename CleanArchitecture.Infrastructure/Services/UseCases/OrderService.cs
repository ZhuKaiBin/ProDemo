

namespace CleanArchitecture.Infrastructure.Services.UseCases
{
    public class OrderService: IOrderServices
    {
        private readonly IToDoRepository<ToDoItem> _toDoRepository;
        public OrderService(IToDoRepository<ToDoItem> toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }


        public async Task<List<ToDoItem>> GetAllAsync()
        {

            var ret = await _toDoRepository.GetAllAsync();
            return ret;


        }

        public async Task<int> CreateAsync(ToDoItem item)
        {
           var ret=await _toDoRepository.CreateAsync(item);
            return ret;
        }
    }
}
