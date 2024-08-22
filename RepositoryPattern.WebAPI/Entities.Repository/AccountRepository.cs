using RepositoryPattern.WebAPI.Entities.Models;

namespace RepositoryPattern.WebAPI.Entities.Repository
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
