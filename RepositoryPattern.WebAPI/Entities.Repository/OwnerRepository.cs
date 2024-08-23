using RepositoryPattern.WebAPI.Entities.Models;
using RepositoryPattern.WebAPI.Entities.Repository.Interfances;

namespace RepositoryPattern.WebAPI.Entities.Repository
{
    public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
    {
        public OwnerRepository(ModelDbContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
