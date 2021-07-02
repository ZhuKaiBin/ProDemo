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
            string Begin = "";

            //var ary = new JArray();

            //for (int i = 0; i < 2; i++)
            //{
            //    var objj = new JObject
            //              {
            //                new JProperty("abnReasonCode","1"),
            //                new JProperty("abnToRec","1"),
            //                new JProperty("bLPQuantity","1"),
            //                new JProperty("cartonCode","1"),
            //                new JProperty("deliveryNumber","1"),
            //                new JProperty("description1","1"),
            //                new JProperty("description2","1"),
            //                new JProperty("height","1"),
            //                new JProperty("length","1"),
            //                new JProperty("lineNumber","1"),
            //                new JProperty("locator","1"),
            //                new JProperty("packQuantity","1"),
            //                new JProperty("packQuantityPallet","1"),
            //                new JProperty("packingType","1"),
            //                new JProperty("pieceQuantity","1"),
            //                new JProperty("quantity","1"),
            //                new JProperty("receiveDateTime","1"),
            //                new JProperty("receiveQuantity","1"),
            //                new JProperty("sKU","1"),
            //                new JProperty("salesDateTime","1"),
            //                new JProperty("salesQuantity","1"),
            //                new JProperty("serialNumberBegin","1"),
            //                new JProperty("serialNumberEnd","1"),
            //                new JProperty("sub","1"),
            //                new JProperty("weight","1" ),
            //                new JProperty("width","1" ),

            //              };
            //    ary.Add(objj);
            //}


            //JObject jo = new JObject(
            //                            new JProperty ( "aSUSContactPerson",""),
            //                            new JProperty ( "ToCity",""),
            //                            new JProperty( "aSUSContactPersonTel",""),
            //                            new JProperty( "contractNO", ""),
            //                            new JProperty("details", ary),
            //                            new JProperty("expectReceiveDateTime", ""),
            //                            new JProperty("org", ""),
            //                            new JProperty("originPONumber", ""),
            //                            new JProperty("pONumber", ""),
            //                            new JProperty("status", ""),
            //                            new JProperty("transportContactPerson", ""),
            //                            new JProperty("transportContactPersonTel", "")                                
            //                    );

            //JArray ja = new JArray();
            //ja.Add(jo);

            //var obj = new JObject
            //              {
            //                  {"Name","PO"},
            //                  {"Company", new JObject
            //                        {
            //                            {"CO", "" },
            //                            {"DIV", "" },
            //                            {"WHSE", ""}
            //                        }
            //                   },
            //                    {"pOs",ja}
            //               };



            //var json = JsonConvert.SerializeObject(obj);

            string bob = "";








            #region  Contains
            //string s = "ECCQ01,ECCQ03";
            //string a = "ECCQ03";
            //bool b = a.Contains(s);
            //bool bb = s.Contains(a);
            #endregion
            #region 值类型和引用类型
            //Console.WriteLine("***************student是值类型Struct***************");
            //student student1 = new student {ID=1,name="bob" };
            //student student2 = student1;
            //student2.ID = 2;
            //student2.name = "bob2";

            //Console.WriteLine($"student1的ID是{student1.ID},Name是{student1.name}");
            //Console.WriteLine($"student2的ID是{student2.ID},Name是{student2.name}");


            //Console.WriteLine("***************Teacher是引用类型Class***************");
            //Teacher Teacher1 = new Teacher { ID = 1, name = "Teacher" };
            //Teacher Teacher2 = Teacher1;
            //Teacher2.ID = 2;
            //Teacher2.name = "Teacher2";

            //Console.WriteLine($"student1的ID是{Teacher1.ID},Name是{Teacher1.name}");
            //Console.WriteLine($"student2的ID是{Teacher2.ID},Name是{Teacher2.name}");
            #endregion
            #region JObject
            //var Body = new JObject(new JProperty("心脏", "一个"), 
            //                       new JProperty("心房", "2只"));
            //var Body2 = new JObject(new JProperty("心脏", "一个"),
            //                       new JProperty("心房", "2只"));
            //JArray ary = new JArray();
            //ary.Add(Body);
            //ary.Add(Body2);

            //var obj = new JObject {                           
            //                  new JProperty("e", new JObject{ new JProperty("1","11"),new JProperty("2","22"),new JProperty("3","33"),new JProperty("4","44")}) ,
            //                  new JProperty("r", "rr"),
            //                  new JProperty("人体", ary)
            //            };

            //var json = JsonConvert.SerializeObject(obj);


            //var obj = new JObject {
            //                  new JProperty("Head",new JObject(new JProperty("transId",""),new JProperty("transCode","GetPO"),
            //                                       new JProperty("transMessage",""),new JProperty("errorCode",""))),
            //                  new JProperty("Body", new JObject(new JProperty("whse","ECSH01"),new JProperty("so_type","ALL"),new JProperty("last_day","2")))
            //            };


            //Console.WriteLine("1的字节数是："+ returnNum("1") + "");
            //Console.WriteLine("A的字节数是：" + returnNum("A") + "");
            //Console.WriteLine("a的字节数是：" + returnNum("a") + "");
            //Console.WriteLine("朱的字节数是：" + returnNum("朱") + "");
            #endregion
            #region DateTimeOffset
            //var timeout = TimeSpan.FromSeconds(180);
            //var times = DateTimeOffset.UtcNow;
            //var ass0 = (DateTimeOffset.UtcNow + timeout);
            //var ass = (DateTimeOffset.UtcNow + timeout).ToUnixTimeSeconds();
            //var expiry = (DateTimeOffset.UtcNow + timeout).ToUnixTimeSeconds().ToString();

            //var q0 = DateTimeOffset.Now;
            //var q = DateTimeOffset.Now.Offset;
            //var q1 = DateTimeOffset.Now.Ticks;
            //var q2 = DateTimeOffset.UtcNow;
            //var q3 = DateTimeOffset.MaxValue;
            //var q4 = DateTimeOffset.MinValue;




            #endregion
            #region CreateSASToken
            string appid = "A9SP5uLCVWXu";
            string appkey = "txE8EiZF3bWH9jh2poZYE3ypR7vzWfEN";
            string ss = CreateSASToken(appid, appkey, TimeSpan.FromMinutes(120));
            Console.WriteLine(ss);
            //Console.WriteLine(JsonConvert.SerializeObject(obj));
            #endregion
            #region Task
            //Console.Out.WriteLine(1);
            //Test().GetAwaiter().GetResult();
            //Console.Out.WriteLine(7);
            #endregion
            #region 继承
            //People people = new People();
            //people.ID = 2;          

            //People people1 = new Teachers();
            //people1.ID = 2;           
            //Teachers teacher = new Teachers();
            //teacher.ID = 2;
            //teacher.Name = "zhuzhuzhu";
            #endregion


            //student student = new student();
            //student.Name = "15";


            // Console.WriteLine(student.Name);


            Console.ReadKey();
        }

        private static async Task Test()
        {
            Console.Out.WriteLine(2);
            await GetV();
            Console.Out.WriteLine(6);
        }
        private static async Task GetV()
        {
            Console.Out.WriteLine(3);
            await Task.Run(() =>
             {
                 Thread.Sleep(10000);
                 Console.WriteLine(4);
             });
            Console.Out.WriteLine(5);
        }


        public struct student
        {
            public int ID { set; get; }
            private string name;
            public string Name
            {
                set
                {
                    if (value != "18")
                    {
                        name = "不能小于18岁";
                    }
                    else
                    {
                        name = value;
                    }

                }
                get { return name; }
            }
        }
        public class Teacher
        {
            public int ID { set; get; }
            public string name { set; get; }
        }
        public static int returnNum(string str)
        {
            int charNum = 0; //统计字节位数
            char[] _charArray = str.ToCharArray();
            for (int i = 0; i < _charArray.Length; i++)
            {
                char _eachChar = _charArray[i];
                if (_eachChar >= 0x4e00 && _eachChar <= 0x9fa5) //判断中文字符
                    charNum += 2;
                else
                    charNum += 1;
            }

            return charNum;
        }
        public static string CreateSASToken(string appid, string appkey, TimeSpan timeout)
        {
            var values = new Dictionary<string, string>
            {
                { "once", CreateRandCode(8) },
                { "appid", appid },
                { "expiry", (DateTimeOffset.UtcNow + timeout).ToUnixTimeSeconds().ToString() }
            };

            var signContent = string.Join("", values.OrderBy(pair => pair.Key).Select(pair => pair.Key + pair.Value));

            string sign;
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(appkey)))
            {
                sign = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(signContent)));
            }

            var para = string.Join("&", values.OrderBy(pair => pair.Key).Select(pair => pair.Key + "=" + HttpUtility.UrlEncode(pair.Value)));
            return para + "&token=" + HttpUtility.UrlEncode(sign);
        }
        private static int Random(int maxValue)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            decimal _base = (decimal)long.MaxValue;
            byte[] rndSeries = new byte[8];
            rng.GetBytes(rndSeries);
            return (int)(Math.Abs(BitConverter.ToInt64(rndSeries, 0)) / _base * maxValue);
        }
        public static string CreateRandCode(int codeLen)
        {
            string keySet = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int keySetLength = keySet.Length;
            StringBuilder str = new StringBuilder(keySetLength);
            for (int i = 0; i < codeLen; ++i)
            {
                str.Append(keySet[Random(keySetLength)]);
            }
            return str.ToString();
        }

        public class People
        {
            public int ID { set; get; }
        }

        public class Teachers : People
        {
            public string Name { set; get; }
        }

    }
}
