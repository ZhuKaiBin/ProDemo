using Microsoft.EntityFrameworkCore;
using RepositoryPattern.WebAPI.Entities.Models;
using RepositoryPattern.WebAPI.Entities.Repository.Interfances;
using System.Linq.Expressions;

namespace RepositoryPattern.WebAPI.Entities.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ModelDbContext _modelDbContext { get; set; }
        public RepositoryBase(ModelDbContext modelContext)
        {
            _modelDbContext = modelContext;
        }


        public IQueryable<T> FindAll() => _modelDbContext.Set<T>().AsNoTracking();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
            _modelDbContext.Set<T>().Where(expression).AsNoTracking();


        public void Create(T entity) => _modelDbContext.Set<T>().Add(entity);

        public void Update(T entity) => _modelDbContext.Set<T>().Update(entity);

        public void Delete(T entity) => _modelDbContext.Set<T>().Remove(entity);
    }
}
