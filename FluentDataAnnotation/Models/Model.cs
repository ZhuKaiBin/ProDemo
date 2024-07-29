using System.ComponentModel.DataAnnotations;

namespace FluentDataAnnotation.Models
{
    public class Model
    {
        //[EvenNumberAttribute(ErrorMessage = "数字必须是偶数")]
        [CustomValidation(typeof(MyCustomerValidator), "IsNotEvenNumber")]
        [Required]
        public int MyNumber { get; set; }
    }

    //自定义验证
    public static class MyCustomerValidator
    {
        public static ValidationResult IsNotEvenNumber(object input)
        {
            var result = new ValidationResult("数字必须是整数！！！");
            if (input == null || !int.TryParse(input.ToString(), out int val))
                return result;
            return val % 2 == 0 ? ValidationResult.Success : result;
        }
    }
}
