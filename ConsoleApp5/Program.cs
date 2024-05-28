namespace ConsoleApp5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //string input = "朱";
            //SourceType? enumValue = EnumHelper.GetEnumValueFromName<SourceType>(input);

            //if (enumValue.HasValue)
            //{
            //    Console.WriteLine($"输入的字符串 \"{input}\" 对应的枚举值是: {enumValue}");
            //}
            //else
            //{
            //    Console.WriteLine($"输入的字符串 \"{input}\" 无法映射到枚举值。");
            //}

            string input = "朱";
            SourceType? enumValue = EnumHelper.GetEnumValueFromName<SourceType>(input);

            if (enumValue.HasValue)
            {
                Console.WriteLine($"输入的字符串 \"{input}\" 对应的枚举值是: {enumValue}");
            }
            else
            {
                Console.WriteLine($"输入的字符串 \"{input}\" 无法映射到枚举值。");
            }

            SourceType enumMember = SourceType.A;
            string name = EnumHelper.GetNameFromEnum(enumMember);
            Console.WriteLine($"枚举成员 \"{enumMember}\" 对应的字符串是: {name}");


        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class SourceNameAttribute : Attribute
    {
        public string Name { get; }

        public SourceNameAttribute(string name)
        {
            Name = name;
        }
    }

    public class EnumHelper
    {
        public static TEnum? GetEnumValueFromName<TEnum>(string name) where TEnum : struct, Enum
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum must be an enumerated type");
            }

            foreach (TEnum value in Enum.GetValues(typeof(TEnum)))
            {
                var field = typeof(TEnum).GetField(value.ToString());
                var attribute = (SourceNameAttribute)Attribute.GetCustomAttribute(field, typeof(SourceNameAttribute));
                if (attribute != null && attribute.Name == name)
                {
                    return value;
                }
            }
            return null;
        }

        public static string GetNameFromEnum<TEnum>(TEnum value) where TEnum : struct, Enum
        {
            var field = typeof(TEnum).GetField(value.ToString());
            var attribute = (SourceNameAttribute)Attribute.GetCustomAttribute(field, typeof(SourceNameAttribute));
            if (attribute != null)
            {
                return attribute.Name;
            }
            else
            {
                return value.ToString();
            }
        }
    }

    public enum SourceType
    {
        [SourceName("朱")]
        A,
        [SourceName("赵")]
        B
    }
}
