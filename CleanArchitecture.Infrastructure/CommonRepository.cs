using CleanArchitecture.Domian;
using Microsoft.EntityFrameworkCore;
namespace CleanArchitecture.Infrastructure
{

    public class CommonRepository<T> : IToDoRepository<T> where T : class
    {
        private readonly ToDoDbContext _context;
        private readonly DbSet<T> _dbSet;

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

        public async Task<int> CreateAsync(T entity)
        {
            _dbSet.Add(entity);
            return await SaveChangesWithTransactionAsync(() => _context.SaveChangesAsync());
        }

        public async Task<int> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return await SaveChangesWithTransactionAsync(() => _context.SaveChangesAsync());
        }

        public Task<List<T>> GetAllAsync()
        {
            return _dbSet.ToListAsync();
        }
    }


    #region 原始 SqlToDoRepository

    //public class SqlToDoRepository : IToDoRepository
    //{
    //    private readonly ToDoDbContext _context;
    //    public SqlToDoRepository(ToDoDbContext context)
    //    {
    //        _context = context;
    //    }
    //    public Task<int> CreateAsync(ToDoItem item)
    //    {
    //        _context.ToDoItems.Add(item);
    //        return _context.SaveChangesAsync();
    //    }
    //    public Task<List<ToDoItem>> GetAllAsync()
    //    {
    //        return _context.ToDoItems.ToListAsync();
    //    }
    //}

    #endregion




}



