using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace S3Storage
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string accessKey = "JJTNBSZODHHSPRLYT2WP";
            string secrretKey = "W2DsPGPzJZ6XhdfxCIbnsxj977NfWSGNkcundaGN";

            var config = new AmazonS3Config
            {
                ServiceURL = "http://192.168.200.233:30012",
                ForcePathStyle = true
            };

            var client = new AmazonS3Client(accessKey, secrretKey, config);
            Random rd = new Random();
            int i = rd.Next(1, 100);
            string aa = i.ToString();
            string Whse = "ECSH01";
            string ObjectName =
                $"{Whse}/{DateTimeOffset.Now.Year.ToString()}/{DateTimeOffset.Now.Month.ToString()}/{DateTimeOffset.Now.Day.ToString()}/{aa}";

            var obj = new JObject
            {
                {
                    "Head",
                    new JObject
                    {
                        new JProperty("transMessage", ""),
                        new JProperty("transCode", ""),
                        new JProperty("transId", ""),
                        new JProperty("errorCode", "")
                    }
                },
                {
                    "Body",
                    new JObject { new JProperty("CreateDateTime", "") }
                }
            };

            await UplaodObjectFromFileAsync(
                client,
                "tmstest",
                ObjectName,
                JsonConvert.SerializeObject(obj)
            );
        }

        public static Stream byte2stream(byte[] buffer)
        {
            Stream stream = new MemoryStream(buffer);
            stream.Seek(0, SeekOrigin.Begin);
            //设置stream的position为流的开始
            return stream;
        }

        public static async Task<MemoryStream> ReadObjectDataAsync(
            IAmazonS3 client,
            string bucketName,
            string keyName
        )
        {
            string responseBody = string.Empty;
            try
            {
                var request = new GetObjectRequest { BucketName = bucketName, Key = keyName };
                var ms = new MemoryStream();
                using (var response = await client.GetObjectAsync(request))
                    await response.ResponseStream.CopyToAsync(ms);

                return ms;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }

        private static async Task UplaodObjectFromFileAsync(
            IAmazonS3 client,
            string bucketName,
            string objectName,
            string ContentBody
        )
        {
            try
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = objectName,
                    ContentBody = ContentBody
                };

                PutObjectResponse response = await client.PutObjectAsync(putRequest);
            }
            catch (AmazonS3Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }
    }
}
