
using Dymitro.DAL.Dapper.Context;

namespace Intelisale.DymitroApi.ServiceExtensions
{
    public static class DBContextExtensions
    {
        public static void ConfigureDapperDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("PostgresConnection")!;
            services.AddSingleton(new DapperContext(connectionString));
        }
    }
}
