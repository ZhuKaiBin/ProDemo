using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace ProDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            DerivedClassA aa = new DerivedClassA();
            aa.Execute();
        }
    }

    public abstract class BaseAbstract
    {
        public void Execute()
        {
            Before();
            Excute();
            After();
        }

        //每个子类必须要重写的自己的逻辑
        public abstract void Excute();

        public virtual void Before()
        {
            Console.WriteLine("BaseAbstract中的Before");
        }

        public virtual void After()
        {
            Console.WriteLine("BaseAbstract中的After");
        }
    }

    public class DerivedClassA : BaseAbstract
    {
        public override void Before()
        {
            Console.WriteLine("DerivedClassA中的Before");
        }

        public override void Excute()
        {
            Console.WriteLine("DerivedClassA中的Excute");
        }
    }
}
