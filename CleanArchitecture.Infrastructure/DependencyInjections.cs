using CleanArchitecture.Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CleanArchitecture.Infrastructure
{
    public static  class DependencyInjections
    {

        public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
        {

            builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
            builder.Services.AddSingleton(TimeProvider.System);





            builder.Services.AddScoped<IOrderServices, OrderService>();
        }

     }
}
