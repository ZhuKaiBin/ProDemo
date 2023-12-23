using Newtonsoft.Json;

namespace abstractClassDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string strA = "{\"AAA\":\"com3\"}";
            string strB = "{\"BBB2\":\"com3\",\"BBB\":\"com3\"}";
            BaseModelDto baseModelDto;
            if ("A".Equals("A1"))
            {
                baseModelDto =  Newtonsoft.Json.JsonConvert.DeserializeObject<ModelA_dto>(strA);
            }
            else
            {
                baseModelDto = Newtonsoft.Json.JsonConvert.DeserializeObject<ModelB_dto>(strB);
            }

           
        }
    }



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
    public class NameJsonDataObject
    {
        public string Name { get; set; }
        public string JsonData { get; set; }

        public BaseModelDto Deserialize()
        {
            if (Name == "A")
            {
                return JsonConvert.DeserializeObject<ModelA_dto>(JsonData);
            }
            else if (Name == "B")
            {
                return JsonConvert.DeserializeObject<ModelB_dto>(JsonData);
            }
            else
            {
                // 处理其他情况，可以返回默认的 BaseModelDto 或者抛出异常
                throw new InvalidOperationException("Unsupported name");
            }
        }
    }


}
