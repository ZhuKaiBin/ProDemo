namespace Services.Interfances
{

    /// <summary>
    /// Service层，这个是统一为控制器提供服务
    /// </summary>
    public interface IServiceManager
    {
        IOwnerService OwnerService { get; }

        IAccountService AccountService { get; }
    }
}