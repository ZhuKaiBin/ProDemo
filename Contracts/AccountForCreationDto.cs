using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class AccountForCreationDto
    {
        public DateTime DateCreated { get; set; }

        public string AccountType { get; set; }
    }
}
