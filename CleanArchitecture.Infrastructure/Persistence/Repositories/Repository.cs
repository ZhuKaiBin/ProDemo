using CleanArchitecture.Application.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T:class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        //你可以把 DbContext 想象成一个“图书馆管理员”
        //而每一个 DbSet<T> 是一类书的书架，比如小说、杂志、论文。

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();//_dbSet 不需要通过构造函数注入进来
            //在 EF Core 中，DbContext 本身就提供了泛型方法 Set<T>()，可以动态获取对应实体类型的 DbSet<T>，
            //你只要注入 DbContext，然后通过 Set<T>() 拿到对应的 _dbSet 就够了。
            //DbSet<T> 是由 DbContext 管理的；
            //注入 DbSet<T> 没有意义，因为你可以随时从 DbContext 获取它；
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }


        public T GetById(object id)
        {
            return _dbSet.Find(id);
        }


        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
