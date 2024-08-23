using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories.Interfaces
{
    public interface IOwnerRepository
    {
        Task<IEnumerable<Owner>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Owner> GetByIdAsync(Guid ownerId, CancellationToken cancellationToken = default);

        void Insert(Owner owner);

        void Remove(Owner owner);
    }
}
