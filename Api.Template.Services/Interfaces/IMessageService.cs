using System.ComponentModel.DataAnnotations;
using HttpMultipartParser;

namespace Api.Template.Services.Interfaces
{
    public interface IMessageService
    {
        List<ValidationResult> Validate(MultipartFormDataParser? multipartFormDataParser);

        Task Publish(string filePrefix, MultipartFormDataParser? multipartFormDataParser, IReadOnlyDictionary<string, string> tags);
    }
}
