using ContactZone.Domain.Domains;

namespace GetContacts.Dtos
{
    public class ContactDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DDD { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public static List<ContactDto> MapToDto(IEnumerable<ContactDomain> contacts)
        {
            return contacts.Select(contact => new ContactDto
            {
                Id = contact.Id,
                Name = contact.Name,
                DDD = contact.DDD,
                Phone = contact.Phone,
                Email = contact.Email
            }).ToList();
        }
        public static ContactDto MapToDto(ContactDomain contact)
        {
            return new ContactDto()
            {
                Id = contact.Id,
                Name = contact.Name,
                DDD = contact.DDD,
                Phone = contact.Phone,
                Email = contact.Email
            };
        }
    }
}
