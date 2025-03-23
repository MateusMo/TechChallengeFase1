using ContactZone.Application.Services;
using ContactZone.Domain.Domains;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Consumer
{
    public class DeleteContactConsumer : IConsumer<ContactDomain>
    {
        private readonly IContactService _contactService;
        private readonly ILogger<DeleteContactConsumer> _logger;

        public DeleteContactConsumer(IContactService contactService, ILogger<DeleteContactConsumer> logger)
        {
            _contactService = contactService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ContactDomain> context)
        {
            var contact = context.Message;

            // Lógica para processar uma mensagem de exclusão
            await _contactService.RemoveAsync(contact);
            _logger.LogInformation($"Contato excluído: {contact.Id}");
        }
    }
}