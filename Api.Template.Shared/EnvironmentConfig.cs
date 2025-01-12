using Api.Template.Shared.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Api.Template.Shared
{
    public class EnvironmentConfig(IConfiguration config) : IEnvironmentConfig
    {
        public string ServiceBusNameSpace => Environment.GetEnvironmentVariable("ServiceBusNameSpace") ?? string.Empty;
        public string ServiceBusTopic => Environment.GetEnvironmentVariable("ServiceBusTopic") ?? string.Empty;
    }
}
