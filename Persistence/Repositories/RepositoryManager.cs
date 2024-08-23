using Domain.Repositories.Interfaces;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// 统一管理所有的基础层访问的规则，并懒加载实现相对于的仓储
    /// </summary>
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly Lazy<IOwnerRepository> _lazyOwnerRepository;
        private readonly Lazy<IAccountRepository> _lazyAccountRepository;
        private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;

        public RepositoryManager(EFDbContext dbContext)
        {
            _lazyOwnerRepository = new Lazy<IOwnerRepository>(() => new OwnerRepository(dbContext));
            _lazyAccountRepository = new Lazy<IAccountRepository>(() => new AccountRepository(dbContext));
            _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(dbContext));
        }

        public IOwnerRepository OwnerRepository => _lazyOwnerRepository.Value;

        public IAccountRepository AccountRepository => _lazyAccountRepository.Value;

        public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
    }
}