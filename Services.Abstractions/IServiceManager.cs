using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
namespace Services.Abstractions
{
    public interface IServiceManager
    {
        IOwnerService OwnerService { get; }

        IAccountService AccountService { get; }
    }
}
