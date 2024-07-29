using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sha1
{
    class Program
    {
        private static Object obj = new object();
        static int location = 0;

        static void Main(string[] args)
        {
            //int location1 = 2;
            //int values =666666;
            //int comparand = 1;

            //Interlocked.CompareExchange(ref location1, values, comparand);

            //Console.WriteLine(location1);
            //Console.WriteLine(values);
            //Console.WriteLine(comparand);

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("前" + location + "\n");
                Interlocked.Increment(ref location);
                Console.WriteLine("后" + location + "\n");
                Interlocked.Decrement(ref location);
                Console.WriteLine("Decrement" + location + "\n");
            }
            Console.WriteLine(location);

            //Thread thread1 = new Thread(new ThreadStart(ThreadMethod));
            //Thread thread2 = new Thread(new ThreadStart(ThreadMethod));
            //thread1.Start();
            //thread2.Start();
            //thread1.Join();
            //thread2.Join();

            //Console.WriteLine("UnsafeInstanceCount: {0}"  +   CountClass.SafeInstanceCount.ToString());

            Console.ReadKey();
        }

        static void ThreadMethod()
        {
            CountClass cClass;

            // Create 100,000 instances of CountClass.
            for (int i = 0; i < 10; i++)
            {
                cClass = new CountClass();
            }
        }

        class CountClass
        {
            static int safeInstanceCount = 0; //使用原子操作

            public static int SafeInstanceCount
            {
                get { return safeInstanceCount; }
            }

            public CountClass()
            {
                Console.WriteLine("前" + safeInstanceCount + "\n");
                Interlocked.Increment(ref safeInstanceCount);
                Console.WriteLine("后" + safeInstanceCount + "\n");
            }
        }

        public static string HashPassword(string input)
        {
            var sha1 = SHA1Managed.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] outputBytes = sha1.ComputeHash(inputBytes);
            return Convert.ToBase64String(outputBytes).Replace("-", "").ToLower();
        }

        public static string EncodeBase64(Encoding encode, string source)
        {
            string base64 = "";
            byte[] bytes = encode.GetBytes(source);
            try
            {
                base64 = Convert.ToBase64String(bytes);
                base64 = base64.Replace("+", "-").Replace("/", "_");
            }
            catch { }
            return base64;
        }

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string EncodeBase64(string source)
        {
            return EncodeBase64(Encoding.UTF8, source);
        }

        public static string PasswordEncryption(string pwd)
        {
            //创建SHA1加密算法对象
            SHA1 sha1 = SHA1.Create();
            //将原始密码转换为字节数组
            byte[] originalPwd = Encoding.UTF8.GetBytes(pwd);
            //执行加密
            byte[] encryPwd = sha1.ComputeHash(originalPwd);
            //将加密后的字节数组转换为大写字符串
            string ret = string.Join(
                "",
                encryPwd.Select(b => string.Format("{0:x2}", b)).ToArray()
            );

            return ret;
        }

        public static string EncryptBySHA1(string cleartext)
        {
            using (HashAlgorithm hashAlgorithm = new SHA1CryptoServiceProvider())
            {
                byte[] utf8Bytes = Encoding.UTF8.GetBytes(cleartext);
                byte[] hashBytes = hashAlgorithm.ComputeHash(utf8Bytes);
                StringBuilder builder = new StringBuilder();
                foreach (byte hashByte in hashBytes)
                {
                    builder.AppendFormat("{0:x2}", hashByte);
                }
                return builder.ToString();
            }
        }
    }
}
