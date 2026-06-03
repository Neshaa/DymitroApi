using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Intelisale.DymitroApi.ServiceExtensions
{
    public static class SwaggerExtensions
    {
        public static void AddAuthorizedSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(sw =>
            {
                sw.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Dymitro Api",
                    Version = "1",
                    Description = $"<b><big>Dymitro Api<b><big>"
                });

                sw.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Bearer token authentication",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                sw.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    new List<string>()
                }
            });
            });
        }

        public static IApplicationBuilder ConfigureSwaggerUse(this IApplicationBuilder app)
        {
            app.UseSwagger(c => c.RouteTemplate = "/swagger/{documentName}/swagger.json");

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v{1}/swagger.json", $"DYMITRO API V{1}");
            });

            return app;
        }
    }
}
