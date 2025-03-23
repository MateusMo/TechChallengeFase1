using ContactZone.Domain.Domains;
using ContactZone.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GetContacts.Repositories
{
    public class ContactRepository : IContactRepository
    {
        protected readonly ContactZoneDbContext _context;
        private readonly DbSet<ContactDomain> _dbSet;

        public ContactRepository(ContactZoneDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<ContactDomain>();
        }

        public async Task<ContactDomain> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<ContactDomain>> GetContactFilteringByDDD(int ddd)
        {
            return await _context.Contatos
                .Where(x => x.DDD == ddd.ToString())
                .ToListAsync();
        }

        public async Task<IEnumerable<ContactDomain>> GetContactWithAllInformation()
        {
            return await _context.Contatos
                .OrderBy(contact => contact.DDD)
                .ThenBy(contact => contact.Id)
                .ToListAsync();
        }
    }
}
