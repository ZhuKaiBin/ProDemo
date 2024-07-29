using FastEndpoints;
using Microsoft.AspNetCore.Builder;

namespace FastEndpointsDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var bld = WebApplication.CreateBuilder();
            bld.Services.AddFastEndpoints();

            var app = bld.Build();
            app.Run();
        }
    }

    public class MyRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }

    public class MyResponse
    {
        public string FullName { get; set; }
        public bool IsOver18 { get; set; }
    }
}
