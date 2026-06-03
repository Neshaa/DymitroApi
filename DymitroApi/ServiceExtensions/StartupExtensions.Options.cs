using Dymitro.Common;

namespace Intelisale.DymitroApi.ServiceExtensions
{
    public static class OptionsExtensions
    {
        public static void AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            //services.Configure<IntegrationOptions>(configuration.GetSection("Integration"));
            //services.Configure<CoolbearOptions>(configuration.GetSection("Coolbear"));
            //services.Configure<ItemOptions>(configuration.GetSection(nameof(ItemOptions)));
            //services.Configure<SalesPriceTypeEshopOrder>(configuration.GetSection("SalesPriceTypeEshopOrder"));
        }
    }
}
