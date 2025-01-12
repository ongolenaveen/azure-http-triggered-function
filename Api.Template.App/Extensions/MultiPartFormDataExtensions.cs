using System.ComponentModel.DataAnnotations;
using HttpMultipartParser;
using Microsoft.Azure.Functions.Worker.Http;

namespace Api.Template.App.Extensions
{
    public static class MultiPartFormDataExtensions
    {
        public static async Task<(List<ValidationResult>? validationResults, MultipartFormDataParser? parsedFormData)>
            TryParseAsync(HttpRequestData requestData)
        {
            var validationResults = new List<ValidationResult>();
            MultipartFormDataParser? multipartFormData = null;
            try
            {
                multipartFormData = await MultipartFormDataParser.ParseAsync(requestData.Body);
            }
            catch (Exception ex)
            {
                validationResults.Add(new ValidationResult(ex.Message));
            }

            return (validationResults, multipartFormData);
        }
    }
}
