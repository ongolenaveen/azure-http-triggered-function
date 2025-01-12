using Api.Template.Domain.ReadModels;

namespace Api.Template.Domain.Interfaces
{
    public interface ICustomersDataProvider
    {
        Task<List<Customer>> GetCustomers(RequestParam requestParam);
    }
}
