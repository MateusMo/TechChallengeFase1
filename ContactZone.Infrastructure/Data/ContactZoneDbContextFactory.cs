using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ContactZone.Infrastructure.Data
{
    public class ContactZoneDbContextFactory : IDesignTimeDbContextFactory<ContactZoneDbContext>
    {
        public ContactZoneDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ContactZoneDbContext>();
            
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "ContactZone.Api"))
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("ContactZone");
            optionsBuilder.UseSqlServer(connectionString);

            return new ContactZoneDbContext(optionsBuilder.Options);
        }
    }
}