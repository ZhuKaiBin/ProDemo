using RepositoryPattern.WebAPI.Entities.Models;
using RepositoryPattern.WebAPI.Entities.Repository;
using RepositoryPattern.WebAPI.Entities.Repository.Interfances;

namespace RepositoryPattern.WebAPI.Entities.RepositoryWrapper
{
    public class RepositoryWrapper : IRepositoryWrapper
    {

        private ModelDbContext _repoContext;
        private IOwnerRepository _owner;
        private IAccountRepository _account;


        public IOwnerRepository Owner
        {
            get
            {
                if (_owner == null)
                {
                    _owner = new OwnerRepository(_repoContext);
                }
                return _owner;
            }
        }
        public IAccountRepository Account
        {
            get
            {
                if (_account == null)
                {
                    _account = new AccountRepository(_repoContext);
                }
                return _account;
            }
        }


        public RepositoryWrapper(ModelDbContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }
        public void Save()
        {
            _repoContext.SaveChanges();
        }

    }
}
