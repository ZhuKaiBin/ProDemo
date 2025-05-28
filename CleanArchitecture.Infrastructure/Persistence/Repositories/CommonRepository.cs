using CleanArchitecture.Domian;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Application.Interfaces.Persistence;
using System.Linq.Expressions;
namespace CleanArchitecture.Infrastructure.Persistence.Repositories
{

    public class CommonRepository<T> : IToDoRepository<T> where T : class
    {
        private readonly ToDoDbContext _context;//ToDoDbContext 就像是一栋数据库的大房子。
        private readonly DbSet<T> _dbSet;//DbSet<T> 是这栋房子里的某个房间，专门用来存放某一类数据（比如 ToDoItem）。
                                         //DbSet<T> _dbSet 这是你在屋子（DbContext）里挑了一个房间出来，用来处理那一类数据。
                                         //我拿到了这栋房子，然后我根据 T（类型）进到了相应的房间里，去拿东西、放东西。

        public CommonRepository(ToDoDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        private async Task<int> SaveChangesWithTransactionAsync(Func<Task<int>> saveChangesFunc)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    int result = await saveChangesFunc();
                    await transaction.CommitAsync();
                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        // 创建
        public async Task<int> CreateAsync(T entity)
        {
            _dbSet.Add(entity);
            return await SaveChangesWithTransactionAsync(() => _context.SaveChangesAsync());
        }

        // 更新
        public async Task<int> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return await SaveChangesWithTransactionAsync(() => _context.SaveChangesAsync());
        }

        // 查询所有
        public Task<List<T>> GetAllAsync()
        {
            return _dbSet.ToListAsync();
        }

        // 删除
        public async Task<int> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return await SaveChangesWithTransactionAsync(() => _context.SaveChangesAsync());
        }


        // 根据Id查询，假设实体有主键名为 Id
        public async Task<T?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        // 根据条件查询，返回 IQueryable 方便调用者继续拼接查询条件
        public IQueryable<T> Query(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        // 批量删除
        public async Task<int> DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            return await SaveChangesWithTransactionAsync(() => _context.SaveChangesAsync());
        }

        // 判断是否存在满足条件的实体
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

    }







}



