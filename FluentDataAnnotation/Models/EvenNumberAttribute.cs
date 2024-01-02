using System.ComponentModel.DataAnnotations;

namespace FluentDataAnnotation.Models
{


    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class EvenNumberAttribute: ValidationAttribute
    {
        public override bool IsValid(object? input)
        {
            if (input == null)
                return false;

            if (!int.TryParse(input.ToString(), out int val))
                return false;

            return val % 2 == 0;
        }
    }




    
}
