using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domian
{
    //储库模式在应用程序的【数据访问层】和【业务逻辑层】之间引入了一个抽象层。
    //这提高了代码的可维护性、可读性和可测试性。
    public interface IToDoRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<int> CreateAsync(T item);
    }
}
