using System;
using System.Collections.Generic;

namespace ProEqual
{
    class Program
    {

        public decimal price { get; set; }
        public int age { get; set; }
        public override string ToString()
        {
            return "$" + price.ToString() + age.ToString();
        }

        public class Dic
        {

            public int get(IDictionary<int,int> nnn)
            {
                return nnn.Count;
            }
        }

        static void Main(string[] args)
        {

            {

                Dictionary<int, int> keyValuePairs = new Dictionary<int, int>() {
                    { 1,1},{ 2,2},{ 3,3}
                };

                Dic dic = new Dic();
                 dic.get(keyValuePairs);
            }

            {
                baseType baseType = new baseType();
                Console.WriteLine(baseType.Num);

            }


            //Program program = new Program();
            //program.price = 2;
            //program.age = 18;

            //Console.WriteLine(program.ToString());

            //string s = "a".ToString();
            //string ss = "a";
            //if (s.Equals(ss))
            //{
            //    Console.WriteLine("相等");
            //}
            //else
            //{
            //    Console.WriteLine("不相等");
            //}

            //if (s==ss)
            //{
            //    Console.WriteLine("相等");
            //}
            //else
            //{
            //    Console.WriteLine("不相等");
            //}


            //Person person = new Person { Name = "bob", Age = 12 };
            //Person person2 = new Person { Name = "bob", Age = 12 };

            //bool b = person.Equals(person2);

            //bool b2 = person.Age.Equals(person2.Age);

            //string a = person.ToString();




            //User user = new User { Name = "bob", Age = 12 };
            //User user2 = new User { Name = "bob", Age = 12 };


            //bool u = user.Equals(user);
            //bool u2 = user.Age.Equals(user.Age);


            Console.ReadKey();



        }

        public class baseType
        {
            public int Num { private set; get; }

            public baseType()
            { }

            public baseType(int num)
            {
                this.Num = num;
            }
        }

        public class Person
        {
            public string Name { set; get; }
            public int Age { set; get; }
            public override string ToString()
            {
                return "Name: " + Name + ", Age:" + Age.ToString();
            }
        }


        public struct User
        {
            public string Name { set; get; }
            public int Age { set; get; }
        }


        
    }
}
