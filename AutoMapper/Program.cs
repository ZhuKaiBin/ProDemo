using AutoMapper;

namespace AutoMapperDemo
{

    /*
     AutoMapper 用法
     */

    internal class Program
    {
        static async Task Main(string[] args)
        {
            // 初始化 AutoMapper
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();  // 添加映射配置
            });

            // 验证映射配置是否有效
            //configuration.AssertConfigurationIsValid();  // 如果映射无效，抛出异常

            // 创建映射器
            IMapper mapper = configuration.CreateMapper();

            // 创建 User 对象
            var user = new User { Name = "John", Age = 30 };

            // 使用 AutoMapper 将 User 映射到 UserDto
            UserDto userDto = mapper.Map<UserDto>(user);

            // 输出 UserDto 内容
            Console.WriteLine($"Name: {userDto.Name}, Age: {userDto.Age}");



        }
    }


    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class UserDto
    {

        public string Name { get; set; }
        public string Age { get; set; }

        public int address { get; set; }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // 配置 User 到 UserDto 的映射
            CreateMap<User, UserDto>();
        }
    }

    #region 构造函数注入
    //public class User
    //{
    //    public string Name { get; set; }
    //    public int Age2 { get; set; }  // 修改为 Age2
    //}

    //public class UserDto
    //{
    //    public string Name { get; set; }
    //    public int Age { get; set; }

    //    // 构造函数手动映射
    //    public UserDto(User user)
    //    {
    //        Name = user.Name;
    //        Age = user.Age2;  // 映射 Age2
    //    }
    //}

    #endregion

}
