using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    //创建自定义用户，框架自带的user表中的字段，不够用，现在要自己新增几个字段
    public class UserCustom : IdentityUser
    {
        public string File1Name { set; get; }
    }
}