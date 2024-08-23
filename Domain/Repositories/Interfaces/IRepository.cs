namespace Domain.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        void Insert(T entity);

        void Remove(T entity);
    }
}