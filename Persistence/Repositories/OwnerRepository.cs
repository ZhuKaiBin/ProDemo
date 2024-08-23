using Domain.Entities;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// 实现Domain层的IAccountRepository，自己按照Domain层设计的规则，自己实现与数据库的交流
    /// </summary>
    internal sealed class OwnerRepository : IOwnerRepository
    {
        private readonly EFDbContext _dbContext;

        public OwnerRepository(EFDbContext dbContext) => _dbContext = dbContext;

        public async Task<IEnumerable<Owner>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await _dbContext.Owners.Include(x => x.Accounts).ToListAsync(cancellationToken);

        public async Task<Owner> GetByIdAsync(Guid ownerId, CancellationToken cancellationToken = default) =>
            await _dbContext.Owners.Include(x => x.Accounts).FirstOrDefaultAsync(x => x.Id == ownerId, cancellationToken);

        public void Insert(Owner owner) => _dbContext.Owners.Add(owner);

        public void Remove(Owner owner) => _dbContext.Owners.Remove(owner);
    }
}