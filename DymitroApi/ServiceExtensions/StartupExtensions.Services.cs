using Dymitro.Contracts;
using Dymitro.Services;
using System.Reflection;

namespace Intelisale.DymitroApi.ServiceExtensions
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<ISalaryService, SalaryService>();
            services.AddTransient<IFootballService, FootballService>();
            services.AddTransient<ISportCompetitionService, SportCompetitionService>();
            services.AddTransient<ISportActivityService, SportActivityService>();
        }
    }
}
