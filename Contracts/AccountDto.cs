using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class AccountDto
    {
        public Guid Id { get; set; }

        public Guid OwnerId { get; set; }

        public DateTime DateCreated { get; set; }

        public string AccountType { get; set; }
    }
}
