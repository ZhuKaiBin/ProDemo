using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Masuit.Tools;
using Masuit.Tools.Hardware;
using Masuit.Tools.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ProList
{
    class Program
    {
        /// <summary>
        /// SHA256 转换为 Hex字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Sha256Hex(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(bytes);
                return ByteArrayToHexString(hash);
            }
        }

        public static string ByteArrayToHexString(byte[] data, bool toLowerCase = true)
        {
            var hex = BitConverter.ToString(data).Replace("-", string.Empty);
            return toLowerCase ? hex.ToLower() : hex.ToUpper();
        }

        public class gf
        {
            public gf()
            {
                Console.WriteLine("gf");
            }
        }

        public class fath : gf
        {
            public fath(int age)
            {
                Console.WriteLine("age");
            }

            public int age;
        }

        public class son : fath
        {
            public son()
                : base(20) { }

            public string name;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string dicJson = "{\"颜色\":\"黑色\",\"尺码\":\"x\"}";
            var proper = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(
                dicJson
            );
            var join = string.Join(";", proper.Values);

            var obj = JObject.Parse(
                "{\"access_token\":\"b424f3ce-6fde-4567-9876-e0bceda9d946\",\"token_type\":\"bearer\",\"refresh_token\":\"87fb2465-bb2f-46ec-8546-06d63c7b0087\",\"refresh_token_expires_in\":11544205,\"expires_in\":7199,\"scope\":\"default\",\"public_account_id\":\"4020541464018\",\"business_id\":\"1501948018\"}"
            );
            var ak = obj.SelectToken("ak", false);
            var sk = obj.SelectToken("access_token", false);
            var a = ak?.ToString();
            var s = sk.ToString();

            Console.ReadKey();
        }
    }
}
