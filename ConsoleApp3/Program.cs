using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {

            Program program = new Program();

            //MydRet("1111", program.ddRet);

            MydRet("1111", () => { return Task.FromResult("带有返回值的"); });

            Console.ReadKey();
        }




        public static async Task MydRet(string a, Func<Task<string>> next)
        {
            var ret = a;

            var rett = await next();

            Console.WriteLine($"{ret}........{rett}");
        }


        public static async Task Myd(string a, Func<Task> next)
        {
            var ret = a;

            await next();
        }


        public async Task dd()
        {
            Console.WriteLine("返回值Taskkkk");
        }

        public async Task<string> ddRet()
        {
            return "带有返回值的";
        }

    }







}
