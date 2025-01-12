using Api.Template.Domain.Interfaces;
using Api.Template.Domain.ReadModels;
using Api.Template.Sql.DataProvider.Interfaces;
using DataAbstractions.Dapper;
using Microsoft.Extensions.Logging;

namespace Api.Template.Sql.DataProvider.DataProviders
{
    public class CustomersDataProvider(
        IDataAccessor dataAccessor,
        ITextFileReader textFileReader,
        ILogger<CustomersDataProvider> logger)
        : ICustomersDataProvider
    {
        public async Task<List<Customer>> GetCustomers(RequestParam requestParam)
        {
            // Stage Estimate Inquiries with no status or status as "hold" 
            var sqlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql", "RetrieveCustomers.sql");
            var sqlQuery = await textFileReader.ReadTextFile(sqlFilePath);
            logger.LogInformation("{sqlQuery}", sqlQuery);

            await dataAccessor.ExecuteAsync(sqlQuery);

            var customers = new List<Customer>()
            {
                new() { Name = "Naveen", Type = "primary", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now }
            };
            return customers;
        }
    }
}
