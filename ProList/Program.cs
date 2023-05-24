using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace ProList
{
    public enum method
    {
        发货后修改物流单号 
    }

    public static class ErrorLevelExtensions
    {
        public static string GetString(this method me)
        {
            switch (me)
            {
                case method.发货后修改物流单号:
                    return "splitLogisitics";              
                default:
                    return "";
            }
        }
    }

    class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            {
                var sd44 = method.发货后修改物流单号.GetString();

                Task.Run(() => Console.WriteLine("123"));

                Task.Run(() => Console.WriteLine("123"));
                    

                Console.WriteLine("123");
                Console.WriteLine()

            }

            {

                int val = 12345678;
                val.ToString("N", new CultureInfo("fr-FR"));

                Console.WriteLine(val);

                DateTime d = new DateTime(2003, 08, 09);
                d.ToString("D", new CultureInfo("fr-FR"));

                Console.WriteLine(d);
            }

            {
                // Embed an array of characters in a string
                string strSource = "changed";
                char[] destination = { 'T', 'h', 'e', ' ', 'i', 'n', 'i', 't', 'i', 'a', 'l', ' ',
                'a', 'r', 'r', 'a', 'y' };

                // Print the char array
                Console.WriteLine(destination);

                // Embed the source string in the destination string
                strSource.CopyTo(0, destination, 4, strSource.Length);

                // Print the resulting array
                Console.WriteLine(destination);

                strSource = "A different string";

                // Embed only a section of the source string in the destination
                strSource.CopyTo(2, destination, 3, 9);

                // Print the resulting array
                Console.WriteLine(destination);

            }


            {
                string sessionUid = "101058s";
                var b = int.TryParse(sessionUid, out int result);
            }





            List<dynamic> itemBatchRefList = new List<dynamic>();

            for (int i = 0; i < 5; i++)
            {
                var itemBatch = new { batchNo = i, goodsSn = i, orderId = i };
                itemBatchRefList.Add(itemBatch);
            }






            string dicJson = "{\"颜色\":\"黑色\",\"尺码\":\"x\"}";

            var proper = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(dicJson);
            var join = string.Join(";", proper.Values);



            var obj = JObject.Parse("{\"access_token\":\"b424f3ce-6fde-4567-9876-e0bceda9d946\",\"token_type\":\"bearer\",\"refresh_token\":\"87fb2465-bb2f-46ec-8546-06d63c7b0087\",\"refresh_token_expires_in\":11544205,\"expires_in\":7199,\"scope\":\"default\",\"public_account_id\":\"4020541464018\",\"business_id\":\"1501948018\"}");
            var ak = obj.SelectToken("ak", false);
            var sk = obj.SelectToken("access_token", false);

            var a = ak?.ToString();
            var s = sk.ToString();


            var mobile = "123456789";


            var sd = mobile.Substring(mobile.Length - 8);

            List<Person> list = new List<Person>();
            list.Add(new Person { name = "BOB" });

            var x = list.FirstOrDefault(x => x.name.Equals("bob"));

            var xx = list.FirstOrDefault(x => x.name.Equals("bob", StringComparison.OrdinalIgnoreCase));

            var xxx = list.FirstOrDefault(x => x.name.Equals("BOB"));


            Console.ReadKey();
        }
    }


    public class Person
    {
        public string name;
    }


}
