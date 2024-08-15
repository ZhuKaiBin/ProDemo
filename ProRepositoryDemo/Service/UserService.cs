using ProRepositoryDemo.Domain;
using ProRepositoryDemo.Models;

namespace ProRepositoryDemo.Service;

public class UserService: IUserService
{
    private readonly IRepository<Account> accountRepository;
    public UserService(IRepository<Account> accountRepository)
    {
        this.accountRepository = accountRepository;//注入仓储类
    }
    
    public async Task<Account> GetAccountMsg(string account)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateLoginTime(Account account)
    {
        throw new NotImplementedException();
    }
}