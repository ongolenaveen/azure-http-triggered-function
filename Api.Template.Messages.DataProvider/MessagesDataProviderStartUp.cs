using Api.Template.Domain.Interfaces;
using Api.Template.Messages.DataProvider.DataProviders;
using Api.Template.Shared.Interfaces;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Template.Messages.DataProvider
{
    public class MessagesDataProviderStartUp : IDataProviderStartup
    {
        public void AddDataProviders(IServiceCollection services)
        {
            services.AddScoped<IMessagesDataProvider, MessagesDataProvider>();
            var environmentConfig = services.BuildServiceProvider().GetRequiredService<IEnvironmentConfig>();

            services.AddSingleton((_) =>
                new ServiceBusClient(environmentConfig.ServiceBusNameSpace, new DefaultAzureCredential()));

        }
    }
}
