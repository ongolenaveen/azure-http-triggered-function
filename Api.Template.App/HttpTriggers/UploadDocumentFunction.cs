using System.Net;
using Api.Template.App.Extensions;
using Api.Template.Domain.DataModels;
using Api.Template.Services.Interfaces;
using Api.Template.Shared.Utilities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Api.Template.App.HttpTriggers
{
    public class UploadDocumentFunction(IMessageService messageService, ILogger<UploadDocumentFunction> logger) : BaseApiFunction
    {
        [Function("UploadUsersMedia")]
        [OpenApiOperation(operationId: "UploadMedia", tags: ["Documents"], Description = "Add Media Files to a Service Bus Topic.")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "ApiKey", In = OpenApiSecurityLocationType.Header)]
        [OpenApiParameter(name: "itemId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "Item identifier.")]
        [OpenApiRequestBody(contentType: "multipart/form-data", typeof(MultiPartFormDocumentData), Description = "Media as form data.")]
        [OpenApiResponseWithoutBody(HttpStatusCode.Created, Description = "Created")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(ApiProblemDetails), Description = "Bad Request.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(ApiProblemDetails), Description = "Internal Server Error.")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post",
                Route = "items/{itemId:maxlength(25)}/documents")]
            HttpRequestData req, string userId, string itemId)
        {
            logger.LogInformation("Received media from the consumer app.");
            var parsedData = await MultiPartFormDataExtensions.TryParseAsync(req);
            if (parsedData.validationResults != null && parsedData.validationResults.Any())
                return BadRequestResponse(req, parsedData.validationResults);

            var parsedFormBody = parsedData.parsedFormData;

            var filePrefix = $"items/{itemId}/documents";
            var validationResults = messageService.Validate(parsedFormBody);
            if (validationResults.Any())
                return BadRequestResponse(req, validationResults);

            var tags = new Dictionary<string, string>
            {
                { "ItemId", itemId }
            };

            await messageService.Publish(filePrefix, parsedFormBody, tags);

            var response = req.CreateResponse(HttpStatusCode.Created);
            response.Headers.Add("Content-Type", "application/json");
            return response;
        }
    }
}
