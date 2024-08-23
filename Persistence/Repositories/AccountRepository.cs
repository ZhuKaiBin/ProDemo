using Domain.Entities;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// 实现Domain层的IAccountRepository，自己按照Domain层设计的规则，自己实现与数据库的交流
    /// </summary>
    internal sealed class AccountRepository : IAccountRepository
    {
        private readonly EFDbContext _dbContext;

        public AccountRepository(EFDbContext dbContext) => _dbContext = dbContext;

        public async Task<IEnumerable<Account>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default) =>
            await _dbContext.Accounts.Where(x => x.OwnerId == ownerId).ToListAsync(cancellationToken);

        public async Task<Account> GetByIdAsync(Guid accountId, CancellationToken cancellationToken = default) =>
            await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == accountId, cancellationToken);

        public void Insert(Account account) => _dbContext.Accounts.Add(account);

        public void Remove(Account account) => _dbContext.Accounts.Remove(account);
    }
}