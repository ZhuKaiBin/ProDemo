using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonPolymorphicDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Person weather = new Man
            {
                City = "Milwaukee",
                Date = new DateTimeOffset(2022, 9, 26, 0, 0, 0, TimeSpan.FromHours(-5)),
                TemperatureCelsius = 15,
                Summary = "Cool"
            };


            var json = JsonSerializer.Serialize<Person>(weather);
            Console.WriteLine(json);

            /*
             JsonPolymorphic: json多态类型   Discriminator：鉴别器，这个是key值。
             JsonDerivedType：json派生类型   这里是所有的派生类

             如果给我一个json，我在json中声明一个type，这个type的value值是JsonDerivedType类里指定的值，
             就会反序列化到对应到JsonDerivedType中的值
             反序列化的时候。只填入基类就行
             */

            string jsonSer = "{\"Key\":\"manvalue\",\"City\":\"Milwaukee\",\"Date\":\"2022-09-26T00:00:00-05:00\",\"TemperatureCelsius\":15,\"Summary\":\"Cool\"}";
            var model = JsonSerializer.Deserialize<Person>(jsonSer);

            string jsonSer2 = "{\"Key\":\"womanvalue\",\"Age\":\"Milwaukee\",\"Date\":\"2022-09-26T00:00:00-05:00\",\"TemperatureCelsius\":15,\"Summary\":\"Cool\"}";
            var model2 = JsonSerializer.Deserialize<Person>(jsonSer2);


        }
    }


    [JsonPolymorphic(TypeDiscriminatorPropertyName = "Key")]
    [JsonDerivedType(typeof(Man), typeDiscriminator: "manvalue")]
    [JsonDerivedType(typeof(Woman), typeDiscriminator: "womanvalue")]
    public class Person
    {
        public DateTimeOffset Date { get; set; }
        public int TemperatureCelsius { get; set; }
        public string? Summary { get; set; }
    }


    public class Man : Person
    {
        public string? City { get; set; }
    }

    public class Woman : Person
    {
        public string Age { set; get; }
    }
}
