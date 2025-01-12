using Api.Template.Domain.Interfaces;
using Api.Template.Shared.Interfaces;
using Api.Template.Sql.DataProvider.DataProviders;
using Api.Template.Sql.DataProvider.Interfaces;
using Api.Template.Sql.DataProvider.Services;
using DataAbstractions.Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Template.Sql.DataProvider
{
    public class SqlDataProviderStartup : IDataProviderStartup
    {
        public void AddDataProviders(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITextFileReader, TextFileReader>();
            services.AddScoped<ICustomersDataProvider, CustomersDataProvider>();
            var environmentConfig = services.BuildServiceProvider().GetRequiredService<IEnvironmentConfig>();

            var connectingString = environmentConfig.DatabaseConnectionString;
            services.AddScoped<IDataAccessor>(_ => new DataAccessor(new SqlConnection(connectingString)));
        }
    }
}
