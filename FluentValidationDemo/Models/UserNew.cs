namespace FluentValidationDemo.Models
{
    
    public class UserNew
    {       
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Password { set; get; } = string.Empty;
        public string ComfirmPwd { set; get; } = string.Empty;

    }
}
