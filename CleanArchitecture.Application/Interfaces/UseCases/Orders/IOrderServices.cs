using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Interfaces.UseCases.Orders
{
    public interface IOrderServices
    {
        Task<List<ToDoItem>> GetAllAsync();

        Task<int> CreateAsync(ToDoItem item);
    }
}
