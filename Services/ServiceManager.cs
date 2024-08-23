using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Abstractions;
using Domain.Repositories.Interfaces;
using Contracts;

namespace Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IOwnerService> _lazyOwnerService;
        private readonly Lazy<IAccountService> _lazyAccountService;

        public ServiceManager(IRepositoryManager repositoryManager)
        {
            _lazyOwnerService = new Lazy<IOwnerService>(() => new OwnerService(repositoryManager));
            _lazyAccountService = new Lazy<IAccountService>(() => new AccountService(repositoryManager));
        }

        public IOwnerService OwnerService => _lazyOwnerService.Value;

        public IAccountService AccountService => _lazyAccountService.Value;
    }
}
