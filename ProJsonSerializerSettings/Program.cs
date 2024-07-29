using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ProJsonSerializerSettings
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime date = new DateTime(2015, 12, 11);
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(date);
            Console.WriteLine(json);

            string jsonMicrosoft = JsonConvert.SerializeObject(
                date,
                new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                    DateFormatString = "d MMMM, yyyy HH:mm:ss",
                    Formatting = Formatting.Indented
                }
            );
            Console.WriteLine(jsonMicrosoft);

            //时间反序列化
            string jsonTime =
                @"[
                '2015-12-11T00:00:00',
                '2015-12-14 13:23:22'
            ]";

            IList<DateTime> list = JsonConvert.DeserializeObject<IList<DateTime>>(
                jsonTime,
                new JsonSerializerSettings { DateFormatString = "yyyy/MM/dd HH:mm:ss" }
            );

            foreach (DateTime datetime in list)
            {
                Console.WriteLine(datetime);
            }
        }
    }
}
