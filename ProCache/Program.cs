using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace ProCache
{

    public static class ExtensionEx
    {

        public static string Get(this Exception exception)
        {
            return string.Concat(exception.Message,";",exception.StackTrace);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {


            {
                string json = "{\"name\":\"张三\",\"birthday\":\"2023-03-30T23:17:25.8158806+08:00\",\"content\":{\"code\":\"zhangsan\"}}";

                var DesDic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);



            }



            {

                int x = 0;
                try
                {
                    int y = 100 / x;
                }
                //catch (ArithmeticException e)
                //{
                //    Console.WriteLine($"ArithmeticException Handler: {e}");
                //}
                catch (Exception e)
                {
                    string eee = e.Get();

                    Console.WriteLine($"Generic Exception Handler: {e}");
                }

            }




            {


                var num1 = 1;
                var num2 = "1";

                var b = num2.Equals(num1);


                Dictionary<object, int> dic = new Dictionary<object, int>();
                int hashCode_int1 = 123456;
                int hashCode_int2 = 123456;

                string hashCode_str1 = "123456";
                string hashCode_str2 = "123456";

                Coord coord1 = new Coord(55, 5);
                Coord coord2 = new Coord(5, 5);

                dic.Add(coord1, 1);
                dic.Add(coord2, 2);


            }

            {
                JObject obj = new JObject();
                obj.Add("name", "张三");
                obj.Add("birthday", DateTime.Now);
                obj.Add("content", JToken.FromObject(new
                {
                    code = "zhangsan"
                }));
                var strObj = JsonConvert.SerializeObject(obj);
            }

            {
                var iter = MyIterator().GetEnumerator();
                while (iter.MoveNext())
                {
                    Console.WriteLine(iter.Current);
                }
            }

            {
                string str = "12345678";
                Console.WriteLine("String = " + str);
                char[] destArr = new char[20];
                destArr[0] = 'A';
                destArr[1] = 'B';
                destArr[2] = 'C';
                destArr[3] = 'D';
                Console.WriteLine(destArr);
                str.CopyTo(3, destArr, 2, 4);
                //str.CopyTo("从st的第几个", destArr, "destArr的索引", "str的长度");
                Console.Write(destArr);

            }


            {

                Dictionary<string, object> para = new Dictionary<string, object>()
            {
                { "expressCode","121"},//物流单号
                { "expressCompanyCode","4"},//物流公司编码
                { "outOrderId","" },//订单编号
                { "outOrderType","1" },//订单类型:1正向订单发货,2售后单发货
                { "subOutOrderId","121" },//子订单号 ||售后换货或者补货编号===》//备用字段1记录订单子项ID
                { "quantity","121" },//
                { "operatorType","0" },//操作人类型 0:系统 1:买家 2:商家
                { "operateType","14" },//发货:14 修改物流：31
            };

                Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "shipInfoList",new List<object>{ para } },
                { "idempotentKey",Guid.NewGuid()}
            };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(parameters);
            }
            {
                var data = DesDecrypt("NT092uXVyFCzZ2iWjaF+/Pf37qvRp3k+Slcan87lrm2mTvUzM8n/bsUwc5d9tDymeJMbHn94lLaKN81fa1CfjLdNDMqbuRGVoakTEeKh/n0ppoLSaXH3yFHL9ggLcIg7RscIB3QMMsV4Lj310dZ3w/3IHcdzM+IldZthzzGcYa+Dtpc7uxLxrujclZ68MLwvoiIhuyBFHjIi1GrHr/aO9pTDBAlKXgV0dCYv/fLLsGSi5BAhbAtEcnEtZ5AMVNB2GsMvhTU8Moo4NqOk3vUw2bCmfzz3/8VdVEZt2qN/wJYG//Sd7BZxmHkDwaZZke3A", "ADFB7CBA0DAB1546");
            }
            Console.ReadKey();
        }






        public static IEnumerable<int> MyIterator()
        {
            yield return 1;
            yield return 2;
            yield return 3;
            yield return 4;
        }

        public class Coord
        {
            int x { set; get; }
            int y { set; get; }


            public Coord(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public override bool Equals(object obj)
            {
                Coord coor = obj as Coord;
                if (coor.x == this.x && coor.y == this.y)
                    return true;
                else
                    return false;
            }

            public override int GetHashCode()
            {
                var ret = (this.x * 5) * (this.y * 10);
                return ret;
            }

        }

        /// <summary>
        /// ECB解密
        /// </summary>
        /// <param name="data">解密字符串</param>
        /// <param name="key">秘钥</param>
        /// <returns></returns>
        public static string DesDecrypt(string data, string key)
        {
            using var des = new DESCryptoServiceProvider();
            des.Key = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.PKCS7;
            MemoryStream memory;
            CryptoStream crypto;
            byte[] byt;
            byt = Convert.FromBase64String(data);
            memory = new MemoryStream();
            crypto = new CryptoStream(memory, des.CreateDecryptor(), CryptoStreamMode.Write);
            crypto.Write(byt, 0, byt.Length);
            crypto.FlushFinalBlock();
            crypto.Close();
            return Encoding.UTF8.GetString(memory.ToArray());
        }


    }


}
