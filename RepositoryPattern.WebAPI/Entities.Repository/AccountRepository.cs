using RepositoryPattern.WebAPI.Entities.Models;
using RepositoryPattern.WebAPI.Entities.Repository.Interfances;

namespace RepositoryPattern.WebAPI.Entities.Repository
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(ModelDbContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
