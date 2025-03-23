using ContactZone.Domain.Domains;
using GetContacts.Repositories;

namespace GetContacts.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<ContactDomain> GetByIdAsync(int id)
        {
            return await _contactRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ContactDomain>> GetContactFilteringByDDD(int ddd)
        {
            return await _contactRepository.GetContactFilteringByDDD(ddd);
        }

        public async Task<IEnumerable<ContactDomain>> GetContactWithAllInformation()
        {
            return await _contactRepository.GetContactWithAllInformation();
        }
    }
}
