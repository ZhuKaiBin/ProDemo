namespace Domain.Repositories.Interfaces
{
    /// <summary>
    /// 定义 数据库保存的规则
    /// </summary>
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}