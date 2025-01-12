using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json.Serialization;

namespace Api.Template.Shared.Utilities
{
    public class ApiProblemDetails
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("status")]
        public int? Status { get; set; }

        [JsonPropertyName("detail")]
        public string? Detail { get; set; }

        [JsonPropertyName("instance")]
        public string? Instance { get; set; }

        [JsonPropertyName("timeStamp")]
        public DateTime Timestamp { get; set; }

        public static ApiProblemDetails CreateFromValidationResults(string functionName, IEnumerable<ValidationResult> validationResults)
        {
            var problemDetails = new ApiProblemDetails
            {
                Detail = string.Join(",", validationResults.Select(x => x.ErrorMessage)),
                Instance = functionName,
                Status = (int)HttpStatusCode.BadRequest,
                Title = "Validation Error",
                Type = "Bad Request",
                Timestamp = DateTime.Now
            };
            return problemDetails;
        }

        public static ApiProblemDetails CreateFromException(string functionName, Exception exception)
        {
            var problemDetails = new ApiProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = exception.Message,
                Type = "Internal server error",
                Instance = functionName,
                Detail = exception.Source,
                Timestamp = DateTime.Now
            };
            return problemDetails;
        }
    }
}
