using RepositoryPattern.WebAPI.Entities.Models;

namespace RepositoryPattern.WebAPI.Entities.Repository
{
    public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
    {
        public OwnerRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
