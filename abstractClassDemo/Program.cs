using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace abstractClassDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var ty = typeof(Class1);
            var infrastructureAssembly = Assembly.GetAssembly(typeof(Class1));




            Man man = new Man
            {
                City = "Milwaukee",
                Date = new DateTimeOffset(2022, 9, 26, 0, 0, 0, TimeSpan.FromHours(-5)),
                TemperatureCelsius = 15,
                Summary = "Cool",
                ManName="男人姓名"
            };



            var json = JsonSerializer.Serialize<Person>(man);
            Console.WriteLine(json);


            string jsonSer = "{\"Key\":\"manvalue11\",\"City\":\"Milwaukee\",\"Date\":\"2022-09-26T00:00:00-05:00\",\"TemperatureCelsius\":15,\"Summary\":\"Cool\"}";
            var model = JsonSerializer.Deserialize<Person>(jsonSer);

            string jsonSer2 = "{\"Key\":\"womanvalue\",\"Age\":\"Milwaukee\",\"Date\":\"2022-09-26T00:00:00-05:00\",\"TemperatureCelsius\":15,\"Summary\":\"Cool\"}";
            var model2 = JsonSerializer.Deserialize<Person>(jsonSer2);



        }
    }


    //这样的写法就是方便多态，这样在序列化的时候，json就会自动的给具体的类一个特定的key值，指明了你这个json是哪个派生类的
    //然后在反序列话的时候，因为json中有具体的key的值，这样可以反序列化成对应的model
    //注：反序列的时候用的是基类，不是派生类
    //序列化的时候也是基类，那这个就碉堡了，我只需要对基类进行序列化和反序列话
    //这个key值也是唯一值，很是重要

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
        public string ManName { set; get; }
    }

    public class Woman : Person
    {
        public string Age { set; get; }
        public string WomanName { set; get; }
    }










    public enum ProtocolEnumDto : ushort
    {
        ModbusRtu = 1,
        ModbusTcp = 2,
        Canopen = 3,
        Dlt645 = 4,
        //非通用
        DandikeDk34 = 1001,
        RigolDho4000 = 1002,
        Smacq3313 = 1003,
        SiglentSdg1000x = 1004
    }


    //public class AbstractLocalDeviceDto
    //{
    //    //[System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverter))]
    //    public ProtocolEnumDto Protocol { set; get; }


    //    [JsonProperty(PropertyName = "newName")]
    //    public string OriginalName { get; set; }

        
    //    public string OldName { get; set; }
    //}






    // 抽象模型基类
    public abstract class BaseModelDto
    {

    }

    // ModelA 对应的 DTO 类
    public class ModelA_dto : BaseModelDto
    {
        public string AAA { set; get; }
    }

    // ModelB 对应的 DTO 类
    public class ModelB_dto : BaseModelDto
    {
        public string BBB { set; get; }
        public string BBB2 { set; get; }
    }

    // 包含名称和 JSON 数据的对象
    //public class NameJsonDataObject
    //{
    //    public string Name { get; set; }
    //    public string JsonData { get; set; }

    //    public BaseModelDto Deserialize()
    //    {
    //        if (Name == "A")
    //        {
    //            return JsonConvert.DeserializeObject<ModelA_dto>(JsonData);
    //        }
    //        else if (Name == "B")
    //        {
    //            return JsonConvert.DeserializeObject<ModelB_dto>(JsonData);
    //        }
    //        else
    //        {
    //            // 处理其他情况，可以返回默认的 BaseModelDto 或者抛出异常
    //            throw new InvalidOperationException("Unsupported name");
    //        }
    //    }
    //}


}
