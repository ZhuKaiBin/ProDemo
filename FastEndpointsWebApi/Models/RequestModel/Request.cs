namespace EndpointsFastWebApi.Models.RequestModel
{
    public class Request
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDay { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
    }

    public class Person
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
