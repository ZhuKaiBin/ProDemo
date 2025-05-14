namespace ActivatorSln_4
{
    /// <summary>
    /// 这一章就是要从类型来创建对象
    /// </summary>
    internal class Program
    {
        //https://reflect.whuanle.cn/4.to4.html
        static void Main(string[] args)
        {

            {

                //Type typeA0 = typeof(int);
                ////object objA0 = Activator.CreateInstance(typeA0,90);这里是不行的，int是没有构造函数的

                //Type typeA = typeof(DateTime);
                //object objA = Activator.CreateInstance(typeA, 2020, 1, 5);//这里在创建的时候，可以给一个默认的日期，因为DateTime是有构造函数的

                //Console.WriteLine("Hello, World!");
            }

            {
                //实例化泛型

                Type type = typeof(List<int>);
                object obj = Activator.CreateInstance(type);

                // 下面的会报错
                Type _type = typeof(List<>);//
                //在 C# 中，List<> 是一个泛型类型，无法直接通过 typeof(List<>) 获取其类型，因为它缺少指定的类型参数。
                ////为了避免报错，你需要指定一个具体的类型参数。
                object _obj = Activator.CreateInstance(_type);


                Type _type = typeof(List<int>);//               
                object _obj = Activator.CreateInstance(_type);


            }


        }
    }
}
