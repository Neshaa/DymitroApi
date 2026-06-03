using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Intelisale.DymitroApi.ServiceExtensions
{
    public static class ControllersExtensions
    {
        public static void AddAuthorizedControllers(this IServiceCollection services)
        {
            services
                .AddControllers();
                //.AddNewtonsoftJson(options =>
                //{
                //    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //    options.SerializerSettings.Formatting = Formatting.Indented;
                //});
        }

        public static IApplicationBuilder UseControllers(this IApplicationBuilder app)
        {
            return app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
