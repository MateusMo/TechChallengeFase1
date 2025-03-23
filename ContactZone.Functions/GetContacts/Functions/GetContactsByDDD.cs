using ContactZone.Domain.Domains;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using GetContacts.Dtos;
using GetContacts.Services;

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
