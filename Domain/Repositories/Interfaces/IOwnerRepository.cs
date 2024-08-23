using Domain.Entities;

namespace Domain.Repositories.Interfaces
{
    /// <summary>
    /// 定义Owner 操作数据库的规则
    /// </summary>
    public interface IOwnerRepository
    {
        Task<IEnumerable<Owner>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Owner> GetByIdAsync(Guid ownerId, CancellationToken cancellationToken = default);

        void Insert(Owner owner);

        void Remove(Owner owner);
    }
}