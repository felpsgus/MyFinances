using MyFinances.Application;
using MyFinances.Infrastructure;

namespace MyFinances.Api;

public static class DependencyInjection
{
    public static void ConfigureDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureApplication();
        services.ConfigureInfrastructure(configuration);
    }
}