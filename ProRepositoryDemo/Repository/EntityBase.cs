namespace ProRepositoryDemo.Domain;

/// <summary>
/// 用户可以自定义主键类型,int,long,double,flout....
/// 取决于传入的T_id
/// </summary>
/// <typeparam name="TId"></typeparam>
public abstract class EntityBase<T_id> :  IEntityBase<T_id>
{
    /// <summary>
    /// 默认主键字段是F_Id
    /// 虚方法
    /// </summary>
    public virtual T_id F_Id { get;  set; }
}

/// <summary>
/// Entity父类,这个是默认的主键Id是long类型
/// </summary>
public abstract class EntityBase : EntityBase<long>//默认字段类型是long
{
    
}


/// <summary>
/// Entity父类,这个是默认的主键Id是long类型
/// </summary>
public abstract class EntityBaseInt : EntityBase<int>//默认字段类型是long
{
    
}