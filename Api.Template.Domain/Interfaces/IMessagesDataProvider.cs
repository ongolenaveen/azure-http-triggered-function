using Api.Template.Domain.DataModels;

namespace Api.Template.Domain.Interfaces
{
    public interface IMessagesDataProvider
    {
        Task<UserDetail> PublishText(UserDetail userDetail);

        Task PublishFile(string fileName, Stream data, Dictionary<string, string> tags);
    }
}
