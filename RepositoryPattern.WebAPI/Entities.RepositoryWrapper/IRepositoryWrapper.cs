using RepositoryPattern.WebAPI.Entities.Repository;

namespace RepositoryPattern.WebAPI.Entities.RepositoryWrapper
{
    public interface IRepositoryWrapper
    {
        IOwnerRepository Owner { get; }
        IAccountRepository Account { get; }
        void Save();
    }
}
