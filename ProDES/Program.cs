using System;
using System.Security.Cryptography;
using System.Text;

namespace ProDES
{
    class Program
    {
        static void Main(string[] args)
        {


            {

                byte[] data = new byte[] { 0x01, 0x02, 0x03, 0x04 };
                Memory<byte> memory = new Memory<byte>(data);

                Span<byte> span = memory.Span;
                byte[] array = memory.ToArray();

            }

            {

                byte[] data = new byte[] { 0x01, 0x02, 0x03, 0x04 };
                Span<byte> span = data.AsSpan(1, 2);


                span[0] = 0x05;
                span[1] = 0x06;


            }






            var ency = DESHelper.Encrypt("{\"name\":\"Asus\",\"age\":\"28\",\"messageId\":\"C0A8A6BA002977369BD39230711D07E3\"}");

            var dec=  DESHelper.Decrypt("NT092uXVyFCzZ2iWjaF+/Pf37qvRp3k+Slcan87lrm2mTvUzM8n/bsUwc5d9tDymeJMbHn94lLaKN81fa1CfjLdNDMqbuRGVoakTEeKh/n0ppoLSaXH3yFHL9ggLcIg7RscIB3QMMsV4Lj310dZ3w/3IHcdzM+IldZthzzGcYa+Dtpc7uxLxrujclZ68MLwvoiIhuyBFHjIi1GrHr/aO9pTDBAlKXgV0dCYv/fLLsGSi5BAhbAtEcnEtZ5AMVNB2GsMvhTU8Moo4NqOk3vUw2bCmfzz3/8VdVEZt2qN/wJYG//Sd7BZxmHkDwaZZke3A");
        }
    }

    public static class DESHelper
    {
        private const string KEY = "ADFB7CBA0DAB1546";
        //private const string IV = "thisisasampleiv";

        // 加密方法
        public static string Encrypt(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                // 设置密钥和向量
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.PKCS7;
                des.Key = Encoding.UTF8.GetBytes(KEY.Substring(0, 8));//Encoding.UTF8.GetString( Encoding.UTF8.GetBytes(KEY.Substring(0, 8)))==>ADFB7CBA
                // 创建加密器对象
                ICryptoTransform encryptor = des.CreateEncryptor();
                // 对输入数据进行加密
                byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
                // 将加密后的字节数据转换为Base64编码字符串
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        // 解密方法
        public static string Decrypt(string input)
        {
            byte[] inputBytes = Convert.FromBase64String(input);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                // 设置密钥和向量

                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.PKCS7;
                des.Key = Encoding.UTF8.GetBytes(KEY.Substring(0, 8));


                // 创建解密器对象
                ICryptoTransform decryptor = des.CreateDecryptor();

                // 对输入数据进行解密
                byte[] decryptedBytes = decryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

                // 将解密后的字节数据转换为UTF8编码字符串
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}
