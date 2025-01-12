using System.Net;
using System.Net.Mime;
using Api.Template.Domain.ReadModels;
using Api.Template.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Template.Api
{
    [ApiController]
    public class SampleController(ICustomersService customersService, 
        ILogger<SampleController> logger) : ControllerBase
    {
        
        /// <summary>
        /// Retrieves customers matching search criteria.
        /// </summary>
        /// <param name="filter">Search criteria.
        /// <remarks>
        /// examples: \
        /// filter='customerId=1'\
        /// name='name='Naveen'' \
        /// type='Priority' AND name='naveen' \
        /// </remarks>
        /// </param>
        /// <param name="sort">Sort criteria.
        /// <remarks>
        /// examples: \
        /// sort='customerId asc'\
        /// sort='name desc' \
        /// sort='Type asc,name desc' \
        /// </remarks>
        /// </param>
        /// <param name="offset">Number of records to skip before retrieving next batch.</param>
        /// <param name="limit">Number of records to be fetched from the data store.
        /// <remarks>
        /// examples: \
        /// limit = 10\
        /// limit = 20\
        /// </remarks>
        /// </param>        
        /// <returns>Get Customers matching search criteria.</returns>
        [Authorize]
        [HttpGet("customers")]
        [ProducesResponseType(typeof(List<Customer>), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType(typeof(ProblemDetails))]
        [Produces(MediaTypeNames.Application.Json)]

        public async Task<ActionResult<List<Customer>>> GetCustomers(
            [FromQuery] string filter, [FromQuery] string sort, [FromQuery] int? offset, [FromQuery] int? limit)
        {
            logger.LogInformation("Received request to get customers.");
            var queryParams = new RequestParam { Filter = filter, Sort = sort, Offset = offset, Limit = limit};
            var response = await customersService.GetCustomers(queryParams);
            return Ok(response);
        }
    }
}