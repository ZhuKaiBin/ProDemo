namespace Domain.Repositories.Interfaces
{
    /// <summary>
    /// 统一管理所有的数据库访问的规则
    /// (注：所有对数据访问的逻辑都应该在domain层设计好，其他层只有【遵守】)
    /// </summary>
    public interface IRepositoryManager
    {
        IOwnerRepository OwnerRepository { get; }

        IAccountRepository AccountRepository { get; }

        IUnitOfWork UnitOfWork { get; }
    }
}