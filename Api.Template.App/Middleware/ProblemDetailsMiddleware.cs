using System.Net;
using Api.Template.Shared.Utilities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace Api.Template.App.Middleware
{
    internal class ProblemDetailsMiddleware(ILogger<ProblemDetailsMiddleware> logger) : IFunctionsWorkerMiddleware
    {
        private readonly ILogger<ProblemDetailsMiddleware> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error processing invocation");

                var httpReqData = await context.GetHttpRequestDataAsync();

                if (httpReqData != null)
                {
                    // Create an instance of HttpResponseData with 500 status code.
                    var newHttpResponse = httpReqData.CreateResponse(HttpStatusCode.InternalServerError);
                    // You need to explicitly pass the status code in WriteAsJsonAsync method.
                    // https://github.com/Azure/azure-functions-dotnet-worker/issues/776

                    var problemDetails = ApiProblemDetails.CreateFromException(context.FunctionDefinition.Name, exception);

                    await newHttpResponse.WriteAsJsonAsync(problemDetails, newHttpResponse.StatusCode.ToString());

                    var invocationResult = context.GetInvocationResult();

                    var httpOutputBindingFromMultipleOutputBindings = GetHttpOutputBindingFromMultipleOutputBinding(context);
                    if (httpOutputBindingFromMultipleOutputBindings is not null)
                    {
                        httpOutputBindingFromMultipleOutputBindings.Value = newHttpResponse;
                    }
                    else
                    {
                        invocationResult.Value = newHttpResponse;
                    }
                }
            }
        }

        private OutputBindingData<HttpResponseData>? GetHttpOutputBindingFromMultipleOutputBinding(FunctionContext context)
        {
            // The output binding entry name will be "$return" only when the function return type is HttpResponseData
            var httpOutputBinding = context.GetOutputBindings<HttpResponseData>()
                .FirstOrDefault(b => b.BindingType == "http" && b.Name != "$return");

            return httpOutputBinding;
        }
    }
}
