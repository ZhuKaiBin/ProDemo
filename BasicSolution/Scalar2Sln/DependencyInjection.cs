using Scalar2Sln.Services.Users;
using Scalar2Sln_Application.Common.Interfaces;
using Scalar2Sln_Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;


namespace Scalar2Sln
{
    public static class DependencyInjection
    {
        public static void AddWebServices(this IHostApplicationBuilder builder)
        {

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IUser, CurrentUser>();

            //builder.Services.AddHealthChecks()
            // .AddDbContextCheck<ApplicationDbContext>();
        }
     }
}
