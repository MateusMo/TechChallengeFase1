using ContactZone.Domain.Domains;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using GetContacts.Dtos;
using GetContacts.Services;
using System.Text.Json;

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
