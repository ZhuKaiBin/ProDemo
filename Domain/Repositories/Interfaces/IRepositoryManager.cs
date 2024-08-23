using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.Interfaces
{
    public interface IRepositoryManager
    {
        IOwnerRepository OwnerRepository { get; }
        IAccountRepository AccountRepository { get; }

        IUnitOfWork UnitOfWork { get; }

    }
}
