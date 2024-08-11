using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ProRepositoryDemo.Domain;

public class Repository<T, TId> : IRepository<T, TId> where T : class, IEntityBase<TId>
{
    /// <summary>
    /// DB上下文
    /// </summary>
    private DbContext Context { get; }
    
    /// <summary>
    /// 实体集合
    /// </summary>
    private DbSet<T> DbSet { get; }

    public Repository(DbContext context)
    {
        Context = context;
        DbSet = Context.Set<T>();
    }
    
    /// <summary>
    /// 事务
    /// </summary>
    /// <returns></returns>
    public IDbContextTransaction BeginTransaction()
    {
        return Context.Database.BeginTransaction();
    }

    IQueryable IRepository<T, TId>.Query()
    {
        return Query();
    }

    // <summary>
    /// 查询
    /// </summary>
    /// <returns></returns>
    public IQueryable<T> Query()
    {
        return DbSet;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="entity"></param>
    public void Add(T entity)
    {
        DbSet.Add(entity);
    }

    /// <summary>
    /// 批量新增
    /// </summary>
    /// <param name="entity"></param>
    public void AddRange(IEnumerable<T> entity)
    {
        DbSet.AddRange(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="entity"></param>
    public void Delete(T entity)
    {
        DbSet.Remove(entity);
    }

    public void Update(T entity)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 同步保存
    /// </summary>
    public void Save()
    {
        Context.SaveChanges();
    }

    /// <summary>
    /// 异步保存
    /// </summary>
    /// <returns></returns>
    public Task SaveAsync()
    {
        return Context.SaveChangesAsync();
    }
}

// <summary>
/// Repository实现类，默认主键类型是long
/// </summary>
/// <typeparam name="T"></typeparam>
public class Repository<T> 
         : Repository<T, long>, IRepository<T> 
         where T : class, IEntityBase<long>
{
    public Repository(DbContext context) : base(context)
    {
    }
}