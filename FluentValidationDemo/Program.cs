using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidationDemo.Models;

namespace FluentValidationDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            //builder.Services.AddScoped<A>(sp =>
            //{
            //    return new A("奥克满", "伊拉克");
            //});

            builder.Services.AddScoped<A>(_ =>
            {
                return new A("奥克满", "伊拉克");
            });

            builder.Services.AddControllers().AddFluentValidation();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddTransient<IValidator<User>, UserValidator>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }

    public class A
    {
        private string a { get; set; }
        private string b { get; set; }
        public A(string a, string b)
        {
            this.a = a;
            this.b = b;
        }


        public void print()
        {
            Console.WriteLine($"{a}。。。。{b}");
        }

    }
}
