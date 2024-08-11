using ProRepositoryDemo.Domain;

namespace ProRepositoryDemo.Models;

public class Account:EntityBase
{
    /// <summary>
    /// 账号
    /// </summary>
    public string F_Account { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string F_Password { get; set; }

    /// <summary>
    /// 最后登入时间
    /// </summary>
    public DateTime? F_LastLoginTime { get; set; }
}