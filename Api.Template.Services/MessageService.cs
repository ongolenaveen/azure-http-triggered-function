using System.ComponentModel.DataAnnotations;
using Api.Template.Domain.Interfaces;
using Api.Template.Services.Interfaces;
using HttpMultipartParser;
using Microsoft.Extensions.Logging;

namespace Api.Template.Services
{
    public class MessageService(
        IMessagesDataProvider messagesDataProvider,
        ILogger<MessageService> logger)
        : IMessageService
    {
        public async Task Publish(string filePrefix, MultipartFormDataParser? multipartFormDataParser, IReadOnlyDictionary<string, string> tags)
        {
            logger.LogInformation("publishing message.");

            if (multipartFormDataParser?.Files == null)
                return;

            var uploadedFiles = multipartFormDataParser.Files;
            
            foreach (var uploadedFile in uploadedFiles)
            {
                var fileName = $"{filePrefix}/{uploadedFile.FileName}";
                var tagsDictionary = new Dictionary<string, string>(tags)
                {
                    { "Type", uploadedFile.Name },
                    { "FileName", uploadedFile.FileName }
                };

                await messagesDataProvider.PublishFile(fileName, uploadedFile.Data, tagsDictionary);
            }
        }

        public List<ValidationResult> Validate(MultipartFormDataParser? multipartFormDataParser)
        {
            var validationResults = new List<ValidationResult>();
            if (multipartFormDataParser?.Files == null || !multipartFormDataParser.Files.Any())
            {
                validationResults.Add(new ValidationResult("You must provide at least one file for upload."));
                return validationResults;
            }

            if (multipartFormDataParser.Files.Any(receivedFile => string.IsNullOrWhiteSpace(receivedFile.FileName)))
            {
                validationResults.Add(new ValidationResult("File name is missing for one of the uploaded file."));
                return validationResults;
            }

            if (multipartFormDataParser.Files.Any(receivedFile => receivedFile.FileName.Length > 256))
            {
                validationResults.Add(new ValidationResult("File name should not exceed more than 256 characters."));
                return validationResults;
            }

            if (multipartFormDataParser.Files.Any(receivedFile => receivedFile.Data.Length <= 0))
            {
                validationResults.Add(new ValidationResult("Data is missing for one of the uploaded file."));
                return validationResults;
            }

            var uploadedFiles = multipartFormDataParser.Files;

            validationResults.AddRange(from uploadedFile in uploadedFiles
                where uploadedFile.Data.Length > 10485760
                select new ValidationResult($"UploadedFile {uploadedFile.FileName} as exceeded maximum size allowed."));

            return validationResults;
        }
    }
}
