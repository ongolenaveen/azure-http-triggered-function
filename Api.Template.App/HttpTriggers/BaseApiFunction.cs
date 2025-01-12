using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using Api.Template.Shared.Utilities;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;

namespace Api.Template.App.HttpTriggers
{
    public abstract class BaseApiFunction
    {
        /// <summary>
        /// Creates bad request response from the validation results.
        /// </summary>
        /// <param name="request">Request Received from the consumer.</param>
        /// <param name="validationResults">Validation Results.</param>
        /// <returns>400-bad request response, if there are validation results.
        /// 200- Ok If there are no validation results.</returns>
        public virtual HttpResponseData BadRequestResponse(HttpRequestData request, List<ValidationResult> validationResults)
        {
            if (!validationResults.Any())
                return request.CreateResponse(HttpStatusCode.OK);

            var badRequestResponse = request.CreateResponse(HttpStatusCode.BadRequest);

            var problemDetails = ApiProblemDetails.CreateFromValidationResults(nameof(BaseApiFunction), validationResults);
            var problemDetailsJson = JsonConvert.SerializeObject(problemDetails);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(problemDetailsJson));
            badRequestResponse.Body = stream;

            badRequestResponse.Headers.Add("Content-Type", "application/json");

            return badRequestResponse;
        }

        /// <summary>
        /// Creates success response from the received body.
        /// </summary>
        /// <typeparam name="T">Type of the response payload.</typeparam>
        /// <param name="request">Request Received from the consumer.</param>
        /// <param name="statusCode">Http status code of the response.</param>
        /// <param name="body">Response payload</param>
        /// <returns>Created success response.</returns>
        public virtual HttpResponseData SuccessResponse<T>(HttpRequestData request, HttpStatusCode statusCode, T body) where T : class
        {
            var successResponse = request.CreateResponse(HttpStatusCode.Created);
            var problemDetailsJson = JsonConvert.SerializeObject(body);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(problemDetailsJson));
            successResponse.Body = stream;
            successResponse.Headers.Add("Content-Type", "application/json");
            return successResponse;
        }
    }
}
