using ContactZone.Domain.Domains;

namespace GetContacts.Services
{
    public interface IContactService
    {
        Task<IEnumerable<ContactDomain>> GetContactWithAllInformation();
        Task<IEnumerable<ContactDomain>> GetContactFilteringByDDD(int ddd);
        Task<ContactDomain> GetByIdAsync(int id);
    }
}
