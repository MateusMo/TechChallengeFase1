using ContactZone.Application.Services;
using ContactZone.Domain.Domains;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Consumer
{
    public class CreateContactConsumer : IConsumer<ContactDomain>
    {
        private readonly IContactService _contactService;
        private readonly ILogger<CreateContactConsumer> _logger;

        public CreateContactConsumer(IContactService contactService, ILogger<CreateContactConsumer> logger)
        {
            _contactService = contactService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ContactDomain> context)
        {
            var contact = context.Message;

            // Lógica para processar uma mensagem de criação
            await _contactService.AddAsync(contact);
            _logger.LogInformation($"Contato criado: {contact.Id}");
        }
    }
}