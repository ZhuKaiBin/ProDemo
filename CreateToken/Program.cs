using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace CreateToken
{

    class Program
    {
        static void Main(string[] args)
        {

            string labels = "";
            string ret=  labels.Split(',')[0];


            Person man = new Man();
            man.per();

            Man man2 = new Man();
            man2.per();


            ThirdMethod thirdMethod = new ThirdMethod();
            thirdMethod.Me(man);
            thirdMethod.Me(man2);
        }
    }

    public class Person
    {
        public void per()
        {
            Console.WriteLine("per");
        }
    }

    public class Man : Person
    {
        public void per()
        {
            base.per();
            Console.WriteLine("per");
        }

        public void man()
        {
            Console.WriteLine("man");
        }
    }

    public class ThirdMethod
    {
        public void Me(Person person)
        { 
        
        }
    }
}
