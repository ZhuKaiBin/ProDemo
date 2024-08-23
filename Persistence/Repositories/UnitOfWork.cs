using Domain.Repositories.Interfaces;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// 实现Domain层的 功能单元接口，自己在基础层把控【SaveChanges】
    /// </summary>
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly EFDbContext _dbContext;

        public UnitOfWork(EFDbContext dbContext) => _dbContext = dbContext;

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}