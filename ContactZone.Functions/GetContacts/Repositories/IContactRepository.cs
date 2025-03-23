using ContactZone.Domain.Domains;

namespace GetContacts.Repositories
{
    public interface IContactRepository
    {
        Task<IEnumerable<ContactDomain>> GetContactWithAllInformation();
        Task<IEnumerable<ContactDomain>> GetContactFilteringByDDD(int ddd);
        Task<ContactDomain> GetByIdAsync(int id);
    }
}
