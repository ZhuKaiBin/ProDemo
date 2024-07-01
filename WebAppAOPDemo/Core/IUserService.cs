using Autofac.Extras.DynamicProxy;
using WebAppAOPDemo.AufofacIntercepter;

namespace WebAppAOPDemo.Core
{
    [Intercept(typeof(AufofacIntercepterDemo))]
    public interface IUserService
    {
        public void GetUserInfo(int id);

        [ingoreInterceptor]
        public void GetUserName(int id);
    }
}