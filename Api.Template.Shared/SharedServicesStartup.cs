using Api.Template.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Template.Shared
{
    public static class SharedServicesStartup
    {
        public static void AddSharedServices(IServiceCollection services)
        {
            services.AddScoped<IEnvironmentConfig, EnvironmentConfig>();
        }
    }
}
