using ContactZone.Domain.Domains;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;

namespace GetContacts.Dtos
{
    [OpenApiExample(typeof(ContactDtoExample))]
    public class ContactDto
    {
        [OpenApiProperty(Description = "The unique identifier for the contact")]
        public int Id { get; set; }

        [OpenApiProperty(Description = "The contact's name")]
        public string Name { get; set; }

        [OpenApiProperty(Description = "The contact's email address")]
        public string Email { get; set; }

        [OpenApiProperty(Description = "The contact's phone number")]
        public string Phone { get; set; }

        [OpenApiProperty(Description = "The contact's DDD (area code)")]
        public string DDD { get; set; }

        public static IEnumerable<ContactDto> MapToDto(IEnumerable<ContactDomain> contacts)
        {
            return contacts.Select(c => new ContactDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
                DDD = c.DDD
            });
        }
    }

    public class ContactDtoExample : OpenApiExample<ContactDto>
    {
        public override IOpenApiExample<ContactDto> Build(NamingStrategy namingStrategy = null)
        {
            Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "sample1",
                    new ContactDto
                    {
                        Id = 1,
                        Name = "John Doe",
                        Email = "john.doe@example.com",
                        Phone = "99999-9999",
                        DDD = "11"
                    },
                    namingStrategy
                )
            );

            return this;
        }
    }
}
