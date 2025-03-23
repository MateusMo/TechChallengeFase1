using ContactZone.Application.Services;
using ContactZone.Domain.Domains;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Consumer
{
    public class UpdateContactConsumer : IConsumer<ContactDomain>
    {
        private readonly IContactService _contactService;
        private readonly ILogger<UpdateContactConsumer> _logger;

        public UpdateContactConsumer(IContactService contactService, ILogger<UpdateContactConsumer> logger)
        {
            _contactService = contactService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ContactDomain> context)
        {
            var contact = context.Message;

            // Lógica para processar uma mensagem de atualização
            _contactService.Update(contact);
            _logger.LogInformation($"Contato atualizado: {contact.Id}");
        }
    }
}