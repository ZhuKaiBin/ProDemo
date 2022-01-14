using System;

namespace ProEqual
{
    class Program
    {

        public decimal price { get; set; }
        public int age { get; set; }
        public override string ToString()
        {
            return "$" + price.ToString()+age.ToString();
        }
        static void Main(string[] args)
        {
            Program program = new Program();
            program.price = 2;
            program.age = 18;

          Console.WriteLine(  program.ToString());

            string s = "a".ToString();
            string ss = "a";
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


            Person person = new Person {Name="bob",Age=12 };
            Person person2 = new Person { Name = "bob", Age = 12 };

            bool b = person.Equals(person2);

            bool b2= person.Age.Equals(person2.Age);

            string a=  person.ToString();




            User user = new User { Name = "bob", Age = 12 };
            User user2 = new User { Name = "bob", Age = 12 };


            bool u = user.Equals(user);
            bool u2 = user.Age.Equals(user.Age);
           

            Console.ReadKey();



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
