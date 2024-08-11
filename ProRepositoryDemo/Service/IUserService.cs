using ProRepositoryDemo.Models;

namespace ProRepositoryDemo.Service;

/// <summary>
/// 这里只是定义逻辑层次要用到的用法
/// </summary>
public interface IUserService
{
    /// <summary>
    /// 获取账号
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    Task<Account> GetAccountMsg(string account);

    /// <summary>
    /// 更新账号最后一次登入时间
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    Task UpdateLoginTime(Account account);
}