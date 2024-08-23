using Microsoft.EntityFrameworkCore;

namespace RepositoryPattern.WebAPI.Entities.Models
{
    public static class ServiceExtensions
    {

        public static void ConfigureMySqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["mysqlconnection:connectionString"];
            services.AddDbContext<ModelDbContext>(o => o.UseMySQL(connectionString));

            //services.AddDbContext<RepositoryContext>(o => o.UseMySQL(connectionString,
            //  MySqlServerVersion.LatestSupportedServerVersion));
        }
    }
}
