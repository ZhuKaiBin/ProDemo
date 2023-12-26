using FluentValidation;
using System.Text.RegularExpressions;

namespace FluentValidationDemo.Models
{
    public class User
    {      
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Password { set; get; }=string.Empty;
        public string ComfirmPwd { set; get; } = string.Empty;
    }

    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("不可以为空置");
            RuleFor(x => x.FirstName).MinimumLength(3).WithMessage("最小长度是3");
            RuleFor(x => x.FirstName).MaximumLength(20);

            RuleFor(x => x.LastName).NotEmpty();


            RuleFor(x => x.Password).Must(x => HasValidPassword(x));

            RuleFor(x => x.ComfirmPwd).Equal(x => x.Password).WithMessage("两次密码不一样");
        }



        private bool HasValidPassword(string pw)
        {
            var lowercase = new Regex("[a-z]+");
            var uppercase = new Regex("[A-Z]+");
            var digit = new Regex("(\\d)+");
            var symbol = new Regex("(\\W)+");

            return (lowercase.IsMatch(pw) && uppercase.IsMatch(pw) && digit.IsMatch(pw) && symbol.IsMatch(pw));
        }

    }



}
