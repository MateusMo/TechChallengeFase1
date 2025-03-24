using ContactZone.Domain.Domains;
using GetContacts.Dtos;
using GetContacts.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace GetContacts.Functions
{
    public class ContactsByDDDFunction
    {
        private readonly IContactService _contactService;
        private readonly ILogger<ContactsByDDDFunction> _logger;

        public ContactsByDDDFunction(IContactService contactService, ILoggerFactory loggerFactory)
        {
            _contactService = contactService;
            _logger = loggerFactory.CreateLogger<ContactsByDDDFunction>();
        }

        [Function("GetContactsByDDD")]
        [OpenApiOperation(operationId: "GetContactsByDDD", tags: new[] { "Contacts" }, Summary = "Get contacts by DDD", Description = "This retrieves contacts filtered by DDD (area code)")]
        [OpenApiParameter(name: "ddd", In = ParameterLocation.Path, Required = true, Type = typeof(int), Summary = "The DDD code", Description = "The area code to filter contacts. Use 0 to get all contacts.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<ContactDto>), Summary = "Successful operation", Description = "Returns the list of contacts")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid DDD", Description = "DDD cannot be negative")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "contacts/ddd/{ddd}")] HttpRequestData req,
            int ddd)
        {
            _logger.LogInformation($"Processing request to get contacts with DDD: {ddd}");

            var response = req.CreateResponse();

            if (ddd < 0)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync("DDD cannot be negative.");
                return response;
            }

            IEnumerable<ContactDomain> contacts;

            if (ddd == 0)
            {
                contacts = await _contactService.GetContactWithAllInformation();
                _logger.LogInformation("Fetching all contacts with information");
            }
            else
            {
                contacts = await _contactService.GetContactFilteringByDDD(ddd);
                _logger.LogInformation($"Fetching contacts filtered by DDD: {ddd}");
            }

            var contactDtos = ContactDto.MapToDto(contacts);

            response.StatusCode = HttpStatusCode.OK;
            await response.WriteAsJsonAsync(contactDtos);

            return response;
        }
    }
}
