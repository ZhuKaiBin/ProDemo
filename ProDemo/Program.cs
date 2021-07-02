using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace ProDemo
{
    class Program
    {
        abstract class BaseClass
        {
            public virtual void MethodA() { }
            public virtual void MethodB() { }
        }
        class Class1 : BaseClass
        {
            public void MethodA(string arg) { }

            public override void MethodB()
            {

            }
        }
        class Class2 : Class1
        {
            new public void MethodB()
            { }
        }
        static void Main(string[] args)
        {

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                    .AddConsole()
                    .AddEventLog();
            });
            ILogger logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation("Example log message");


            var CHTimeZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
            Console.WriteLine(CHTimeZone);
            //TimeZoneInfo.Local：得到一个System.TimeZoneInfo表示本地时区的
            //ConvertTime：将时间从一个时区转换到另一个时区.
            //1：要转换的日期和时间。2：日期时间的时区。3：要将日期时间转换为的时区。
            DateTime Chtime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, CHTimeZone);
            Console.WriteLine(Chtime);
            //返回一个
            DateTimeOffset ds = new DateTimeOffset(Chtime, TimeSpan.FromHours(8));
            Console.WriteLine(ds);

            var obj = new JObject
                    {
                        {"head", new JObject
                                {
                                        { "TransId", "" },
                                        { "TransCode", "X1008" }
                                }
                        },
                        {"body", new JObject
                            {
                                { "whse", ""},
                                { "po", new JObject{
                                    new JProperty("po_number",""),
                                    new JProperty("receive_date_time",""),
                                    new JProperty("detail","")
                                } },
                            }
                        }
                    };

            var json = JsonConvert.SerializeObject(obj);

            string end = "";

            //Class2 o = new Class2();
            //o.MethodA();

            #region 
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            //dic.Add("aaa", "123");
            //dic.Add("bbb", "456");
            //dic.Add("ccc", "789");
            //dic.Add("ddd", "321");
            ////获取与指定的键相关联的值,
            ////虽然这里给ourStr已经是赋值了的,但是执行到dic的时候,就是会去找键:'aaa'的值，找到了outStr就是值,找不到就是null值

            //string outStr = "999";            //上面的dic中并没有ttt,所以下面的outStr会是NULL值,
            ////是避免了判断key知否存在而引发"给定关键字不在字典中."的错误。
            //dic.TryGetValue("ttt", out outStr);
            //dic.TryGetValue("ttt", out string str);
            //Console.WriteLine(outStr + "<br />");
            //dic.TryGetValue("bbb", out outStr);
            //Console.WriteLine(outStr + "<br />");
            #endregion

            //Base obj = new Sub();
            //obj.Func();
            //Console.ReadKey();

            //Car car = new Car();
            //car.price = "";
            //car.type = "";


            //所谓索引器就是一类特殊的属性，
            //通过它们你就可以像引用数组一样引用自己的类
            //通过索引器可以存取类的实例的数组成员，操作方法和数组相似，一般形式如下：对象名[索引]
            ////表示先创建一个对象IndexClass，再通过索引来引用该对象中的数组元素
            IndexClass index = new IndexClass();
            index[0] = "王五";
            index[1] = "赵四";
            Console.WriteLine(index[0].ToString());
            Console.WriteLine(index[1].ToString());

            //inters inters = new inters();
            //inters[0] = "bob";
            //Console.WriteLine(inters[0]);

            // string zasa= getm("阿里巴巴", out string a);
            //string CEO = a;

            Console.ReadKey();
        }



        public static string getm(string z, out string CreateUser)
        {
            if (z == "阿里巴巴")
            {
                CreateUser = "马云";
            }
            else
            {
                CreateUser = "刘强东";
            }
            return z;
        }


        public class IndexClass
        {
            //所谓索引器就是一类特殊的属性，
            //通过它们你就可以像引用数组一样引用自己的类
            private string[] name = new string[10];
            public string this[int index]
            {
                get { return name[index]; }
                set
                {

                    if (value == "王五")
                    {
                        name[index] = "Bob";
                    }
                    else
                    {
                        name[index] = value;
                    }
                }
            }
        }

        class Base
        {
            public Base()
            {
                Func();
            }

            public virtual void Func()
            {
                Console.WriteLine("Base.FUNC");
            }
        }
        class Sub : Base
        {
            public Sub()
            {
                Func();
            }

            public override void Func()
            {
                Console.WriteLine("Sub.Fun123");
            }
        }

        public class Car
        {
            public string type;
            string No;
            private int heavy;
            double Speed;
            protected string ower;
            public string price;
            private string color;
        }


        interface inter
        {
            void GetMethodA();
            string GetProp { set; get; }
            event EventHandler even;
            string this[int index] { get; set; }

        }
        public class inters : inter
        {

            private string[] str = new string[100];
            public string this[int index]
            {
                get
                {
                    return str[index];
                }
                set
                {
                    str[index] = value;
                }
            }

            public string GetProp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public event EventHandler even;

            public void GetMethodA()
            {
                throw new NotImplementedException();
            }





            public class Root
            {
                public head head { set; get; }
                public body body { set; get; }
            }


            public class head
            {
                public string transmessage { set; get; }
                public string transcode { set; get; }
                public string transid { set; get; }
                public string errorcode { set; get; }
            }

            public class body
            {
                public string co { set; get; }
                public string whse { set; get; }
                public string paytime { set; get; }
                public string ordersn { set; get; }
                public string province { set; get; }

                public string city { set; get; }
                public string area { set; get; }
                public string address { set; get; }

                public string consignee { set; get; }
                public string shippingname { set; get; }
                public string shipmobile { set; get; }

                public string erpno { set; get; }

                public string invoicetitle { set; get; }
                public List<detail> detail { set; get; }
            }



            public class detail
            {
                public string lineno { set; get; }
                public string goodsname { set; get; }
                public string goodsnumber { set; get; }
                public string partno { set; get; }
                public string errorcode { set; get; }

            }
        }
    }
}
