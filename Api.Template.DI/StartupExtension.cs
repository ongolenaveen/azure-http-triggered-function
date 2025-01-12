using Api.Template.Messages.DataProvider;
using Api.Template.Services;
using Api.Template.Services.Interfaces;
using Api.Template.Shared;
using Api.Template.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Template.DI
{
    public static class StartupExtension
    {
        public static void AddFunctionAppBindings(this IServiceCollection services)
        {
            SharedServicesStartup.AddSharedServices(services);
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IDataProviderStartup, MessagesDataProviderStartUp>();

            var serviceProvider = services.BuildServiceProvider();
            var providers = serviceProvider.GetServices<IDataProviderStartup>();
            foreach (var provider in providers)
            {
                provider.AddDataProviders(services);
            }
        }
    }
}
