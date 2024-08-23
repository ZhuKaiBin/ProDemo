using Domain.Repositories.Interfaces;
using Services.Interfances;

namespace Services
{

    /// <summary>
    /// 实现IServiceManager，供调用IServiceManager的地方使用里面对应的Service
    /// </summary>
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IOwnerService> _lazyOwnerService;
        private readonly Lazy<IAccountService> _lazyAccountService;

        public ServiceManager(IRepositoryManager repositoryManager)
        {
            //Service依赖Domain层的仓库，进行与数据库的交互
            _lazyOwnerService = new Lazy<IOwnerService>(() => new OwnerService(repositoryManager));


            _lazyAccountService = new Lazy<IAccountService>(() => new AccountService(repositoryManager));
        }

        public IOwnerService OwnerService => _lazyOwnerService.Value;

        public IAccountService AccountService => _lazyAccountService.Value;
    }
}