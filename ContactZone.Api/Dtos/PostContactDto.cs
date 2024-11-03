using System.ComponentModel.DataAnnotations;
using ContactZone.Domain.Domains;

namespace ContactZone.Api.Dtos
{
    public class PostContactDto
    {
        public string Name { get; set; }

        public string DDD { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public static ContactDomain ToDomain(PostContactDto contactDto)
        {
            return new ContactDomain
            {
                Name = contactDto.Name,
                DDD = contactDto.DDD,
                Phone = contactDto.Phone,
                Email = contactDto.Email,
            };
        }
    }
}
