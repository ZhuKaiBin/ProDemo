using Microsoft.AspNetCore.JsonPatch;

namespace JsonPatchSln
{

    //它是为了解决什么问题？
    //核心问题：    
    //在传统的 REST API 中，更新资源要么是整体更新（PUT），要么是手动设计局部更新的API。    
    //如果用 PUT，哪怕只改一个字段，也得传一整个对象，浪费流量。    
    //JsonPatch 允许轻量级局部更新，规范地描述变更，而且兼容 HTTP PATCH 方法。


    internal class Program
    {
        static void Main(string[] args)
        {
            var person = new Person { Name = "Tom", Age = 25 };

            // 创建一个Patch文档
            var patchDoc = new JsonPatchDocument<Person>();
            patchDoc.Replace(p => p.Name, "Jerry"); // 把Name换成Jerry
            //patchDoc.Increment(p => p.Age, 1);       // 年龄加1 （.NET 7后支持Increment）

            // 应用Patch到对象
            patchDoc.ApplyTo(person);


            Console.WriteLine($"Name: {person.Name}, Age: {person.Age}");
        }
    }


    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

}
