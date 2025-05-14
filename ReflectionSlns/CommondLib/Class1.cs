using System.Reflection;

namespace CommondLib
{
    public class Class1
    {

        public string RetInfo()
        {
            //这个是返回当前自己所在的程序集信息
            //当前方法定义在哪个程序集，就返回哪个（通常是类库）
            var executingAssembly =  Assembly.GetExecutingAssembly();


            //这个是返回谁来调用了RetInfo的程序集信息
            //项目A调用了这个Class1.RetInfo,那么返回的就是项目A的程序集名称
            //项目B调用了这个Class1.RetInfo,那么返回的就是项目B的程序集名称
            //谁调用了当前方法，就返回那个调用者的程序集（通常是主程序）
            var callingAssembly = Assembly.GetCallingAssembly().FullName;

            return callingAssembly;
        }
    }
}
