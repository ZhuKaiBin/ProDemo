using Domain.Entities;

namespace Domain.Repositories.Interfaces
{
    /// <summary>
    /// 定义Account 操作数据库的规则
    /// </summary>
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);

        Task<Account> GetByIdAsync(Guid accountId, CancellationToken cancellationToken = default);

        void Insert(Account account);

        void Remove(Account account);
    }
}