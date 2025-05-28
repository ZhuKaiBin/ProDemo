using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Interfaces.Persistence
{
    //储库模式在应用程序的【数据访问层】和【业务逻辑层】之间引入了一个抽象层。
    //这提高了代码的可维护性、可读性和可测试性。
    public interface IToDoRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<int> CreateAsync(T item);

        public  Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(T item);


        public  Task<T?> GetByIdAsync(object id);
        public IQueryable<T> Query(Expression<Func<T, bool>> predicate);
        public  Task<int> DeleteRangeAsync(IEnumerable<T> entities);
        public  Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);


    }
}
