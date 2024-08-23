using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Account
    {
        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; }

        public string AccountType { get; set; }

        public Guid OwnerId { get; set; }
    }
}
