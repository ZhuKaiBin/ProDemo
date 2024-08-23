using RepositoryPattern.WebAPI.Entities.Repository.Interfances;

namespace RepositoryPattern.WebAPI.Entities.RepositoryWrapper
{
    public interface IRepositoryWrapper
    {
        IOwnerRepository Owner { get; }
        IAccountRepository Account { get; }
        void Save();
    }
}
