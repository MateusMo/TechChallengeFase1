using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace GetContacts.Config
{
    class Configuration : DefaultOpenApiConfigurationOptions
    {
        public override OpenApiInfo Info { get; set; } = new OpenApiInfo()
        {
            Version = "1.0.0",
            Title = "Contact Zone API",
            Description = "API for managing contacts in the Contact Zone system",
            Contact = new OpenApiContact()
            {
                Name = "Pós FIAP",
                Url = new Uri("https://github.com/MateusMo/TechChallengeFase1"),
            }
        };

        public override OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V3;
    }
}