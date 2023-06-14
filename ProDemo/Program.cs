using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using MongoDB.Bson;
using Newtonsoft.Json.Schema;
using System.Linq;

namespace ProDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            {
                string a = "str_1"; //声明变量a，将变量a的指针指向内存中新产生的"str_1"的地址
                a = "str_2";  //CLR先会在字符串池中遍历，查看"str_2"是否已存在，如果没有，则新建"str_2"，并修改变量a的指针，指向"str_2"内存地址，"str_1"保持不变。（字符串恒定）
                string c = "str_1"; //CLR先会在字符串池中遍历"str_2"是否已存在，如果存在，则直接将变量c的指针指向"str_2"的地址。（字符串驻留）
                string d = "str_1";                
                Console.WriteLine(ReferenceEquals(a, c));
            }

            
        }
    }
}
