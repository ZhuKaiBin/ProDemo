using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProIndexes
{
    class Program
    {
        private static IConfiguration Configuration { get; set; }
        static void Main(string[] args)
        {


            var builder = new HostBuilder();
            var environment = Environment.GetEnvironmentVariable("zkb");

           





            var config = new ConfigurationBuilder()
                          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        //.AddEnvironmentVariables()
                        .Build();
            Configuration = config.GetSection("ConnectionStrings:xxx");


            builder.ConfigureAppConfiguration((h, services) =>
            {
                Configuration = h.Configuration;

                var zz = h.HostingEnvironment;
                services.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            });

          string  str= Configuration.GetConnectionString("xxx");


            var builder1 = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var configuration = builder1.Build();
            string baseUrl = configuration.GetSection("ConnectionStrings:xxx").Value;


            builder.ConfigureServices((h, services) =>
            {
                Configuration = h.Configuration;
                services.AddOptions();

                string aaaa = h.Configuration.GetConnectionString("xxx");
                services.AddSingleton(Configuration);

            });

            string ss = "";

            var s = Configuration.GetConnectionString("xxx");

            var host = builder.Build();
            using (host)
            {
                host.Run();
            }

            Console.WriteLine("666" + Configuration.GetConnectionString("xxx"));

            //Person breakingbad = new Person();
            ////使用索引器设置值
            //breakingbad[3] = "谢耳朵";
            //breakingbad[4] = "潘妮";

            ////使用索引器获得值
            //for (int i = 0; i < 5; i++)
            //{
            //    System.Console.WriteLine("元素 #{0} = {1}", i, breakingbad[i]);
            //}
            //System.Console.WriteLine("Press any key to exit.");
            //System.Console.ReadKey();

            //indexes indexes = new indexes();
            ////indexes[111] = "zhu";
            //if (string.IsNullOrEmpty(indexes[0]))
            //{
            //    indexes[1] = "zhu";
            //}

            //Console.WriteLine(indexes[1]);
            //System.Console.ReadKey();







            //var randomNumbers = Enumerable.Range(0, 100).OrderBy(x => Guid.NewGuid());
            //int num=  randomNumbers.Count();
            //Dictionary<string, string> dic1 = new Dictionary<string, string>
            //        {
            //           { "access_token","1"},
            //           { "app_key","1" },
            //           { "method","1" },
            //           { "timestamp","1" },
            //           { "v","1" },
            //         };
            //string s1=  string.Join("", dic1.OrderBy(p => p.Key).Select(p => $"{p.Key}{p.Value}"));
            //string ss = string.Join("", dic1);
            //string.Join(",", dic1.OrderBy(p => p.Key).Select(p => p.Value));


            //Dictionary<string, string> dic2 = new Dictionary<string, string>
            //        {
            //           { "access_token","2"},
            //           { "app_key","2" },
            //           { "method","1" },
            //           { "timestamp","2" },
            //           { "v","2" },
            //         };
            //var dic12 = dic1.Union(dic2).Select(p=>$"{p.Value}");
            //var ssss2 = dic12.ToList();

            //var randomNumbers= Enumerable.Range(0,2);
            //string nums = "";
            //foreach (var item in randomNumbers)
            //{
            //    nums = nums + ","+item;
            //}
            //string aa = "";
            //int num3 = 0;
            //Action<int, int> action;
            //action = (int num1,int num2) => {
            //    num3 = num1 + num2;
            //};
            //action(6,5);

            //Student[] stu = new Student[3];
            //stu[0] = new Student()
            //{
            //    Id = 203,
            //    Name = "Tony Stark",
            //    Gender = "Male"
            //};
            //stu[1] = new Student()
            //{
            //    Id = 205,
            //    Name = "Hulk",
            //    Gender = "Male"
            //};
            //stu[2] = new Student()
            //{
            //    Id = 210,
            //    Name = "Black Widow",
            //    Gender = "Female"
            //};

            ////将数组转成List
            //List<Student> stulist=stu.ToList<Student>();
            //foreach (Student student in stulist)
            //{
            //    Console.WriteLine("Id = " + student.Id + " " + " Name = " + student.Name + " " + " Gender = " + student.Gender);
            //}
            //var list= stulist.Select(p => p.Id == 210).FirstOrDefault();
            ////将list转成数组
            //Student[] toarray = stulist.ToArray<Student>();
            //foreach (Student student in toarray)
            //{
            //    Console.WriteLine("Id = " + student.Id + " " + " Name = " + student.Name + " " + " Gender = " + student.Gender);
            //}
            ////将数组转成Dictionary
            //Dictionary<int, Student> StudentDictionary = toarray.ToDictionary(key => key.Id, Studentobj => Studentobj);
            //foreach (KeyValuePair<int, Student> student in StudentDictionary)
            //{
            //    Console.WriteLine("Id = " + student.Key + " " + " Name = " + student.Value.Name + " " + " Gender = " + student.Value.Gender);
            //}

        }
    }


    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
    }

    public class Person
    {
        private string[] hero = new string[5] { "老白", "小粉", "炸鸡哥", "空姐", "Hank" };
        //声明索引
        public string this[int index]
        {
            get
            {
                return hero[index];
            }
            set
            {
                hero[index] = value;
            }
        }
    }
    public class indexes
    {
        private string[] name = new string[10];
        public string this[int num]
        {
            get { return name[num]; }
            set { name[num] = value; }
        }
    }
}
