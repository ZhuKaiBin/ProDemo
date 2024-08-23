using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence;
using Domain.Entities;

namespace Persistence.Repositories
{
    internal sealed class AccountRepository : IAccountRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public AccountRepository(RepositoryDbContext dbContext) => _dbContext = dbContext;

        public async Task<IEnumerable<Account>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default) =>
            await _dbContext.Accounts.Where(x => x.OwnerId == ownerId).ToListAsync(cancellationToken);

        public async Task<Account> GetByIdAsync(Guid accountId, CancellationToken cancellationToken = default) =>
            await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == accountId, cancellationToken);

        public void Insert(Account account) => _dbContext.Accounts.Add(account);

        public void Remove(Account account) => _dbContext.Accounts.Remove(account);
    }
}
