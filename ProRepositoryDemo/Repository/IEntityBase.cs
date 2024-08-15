namespace ProRepositoryDemo.Domain;

/// <summary>
/// 表的根基,传入什么类型主键是什么类型
/// 还可以加入CreateTime,User....
/// </summary>
/// <typeparam name="TId"></typeparam>
public interface IEntityBase<TId>
{
    /// <summary>
    /// 默认主键F_Id
    /// </summary>
    TId F_Id { get; }
}
