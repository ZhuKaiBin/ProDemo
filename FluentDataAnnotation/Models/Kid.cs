using System.ComponentModel.DataAnnotations;

namespace FluentDataAnnotation.Models
{
    public class Kid
    {
        [Range(0, 18)] // 年龄不能超过18岁，不能为负数
        public int Age { get; set; }

        [StringLength(maximumLength:50,MinimumLength =10)] // 名称的长度不能超过 50，不能小于 3
        public string Name { get; set; }

        [DataType(DataType.Date)] // 生日将作为日期展示 (不带时间)
        public DateTime Birthday { get; set; }
    }
}
