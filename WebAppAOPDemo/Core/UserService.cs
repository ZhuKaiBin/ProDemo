namespace WebAppAOPDemo.Core
{
    public class UserService : IUserService
    {
        public void GetUserInfo(int id)
        {
            Console.WriteLine("我是奥特曼");
        }

        public void GetUserName(int id)
        {
            Console.WriteLine("我的名字叫大古");
        }
    }
}