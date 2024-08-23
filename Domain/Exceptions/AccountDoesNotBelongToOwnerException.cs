using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class AccountDoesNotBelongToOwnerException : BadRequestException
    {
        public AccountDoesNotBelongToOwnerException(Guid ownerId, Guid accountId)
         : base($"The account with the identifier {accountId} does not belong to the owner with the identifier {ownerId}")
        {

        }
    }
}
