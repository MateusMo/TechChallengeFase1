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
    public class GetContactByIdFunction
    {
        private readonly IContactService _contactService;
        private readonly ILogger<GetContactByIdFunction> _logger;

        public GetContactByIdFunction(IContactService contactService, ILoggerFactory loggerFactory)
        {
            _contactService = contactService;
            _logger = loggerFactory.CreateLogger<GetContactByIdFunction>();
        }

        [Function("GetContactById")]
        [OpenApiOperation(operationId: "GetContactById", tags: new[] { "Contacts" }, Summary = "Get a contact by ID", Description = "This retrieves a contact by its ID")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Summary = "The contact ID", Description = "The unique identifier for the contact")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ContactDto), Summary = "Successful operation", Description = "Returns the contact details")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Contact not found", Description = "The contact with the specified ID was not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID", Description = "The ID must be a positive value")]
        public async Task<HttpResponseData> GetContactById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "contacts/{id}")] HttpRequestData req,
            int id)
        {
            _logger.LogInformation($"C# HTTP trigger function processed a request to get contact with ID: {id}");

            if (id <= 0)
            {
                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequestResponse.WriteStringAsync("The id must be a positive value");
                return badRequestResponse;
            }

            var contact = await _contactService.GetByIdAsync(id);

            if (contact == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            var contactList = new List<ContactDomain> { contact };
            var contactDto = ContactDto.MapToDto(contactList);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(contactDto.First());
            return response;
        }
    }
}
