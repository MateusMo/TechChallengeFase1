using System.Linq.Expressions;
using ContactZone.Domain.Domains;
using ContactZone.Infrastructure.Data;
using ContactZone.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ContactZone.Infrastructure.Test.Repositories
{
    public class GenericRepositoryTest
    {
        private readonly ContactZoneDbContext _context;
        private readonly GenericRepository<ContactDomain> _repository;

        public GenericRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<ContactZoneDbContext>()
                .UseInMemoryDatabase(databaseName: $"ContactZoneDb_{Guid.NewGuid()}")
                .Options;

            _context = new ContactZoneDbContext(options);

            _repository = new GenericRepository<ContactDomain>(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddEntity()
        {
            var contact = new ContactDomain { Id = 1, DDD = "11", Name = "João", Email = "joao@example.com", Phone = "111111111" };

            await _repository.AddAsync(contact);
            var result = await _repository.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("João", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntityById()
        {
            var contact = new ContactDomain { Id = 1, DDD = "11", Name = "João", Email = "joao@example.com", Phone = "111111111" };
            await _repository.AddAsync(contact);

            var result = await _repository.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            var contacts = new List<ContactDomain>
            {
                new ContactDomain { Id = 1, DDD = "11", Name = "João", Email = "joao@example.com", Phone = "111111111" },
                new ContactDomain { Id = 2, DDD = "21", Name = "Maria", Email = "maria@example.com", Phone = "222222222" }
            };

            await _repository.AddRangeAsync(contacts);

            var result = await _repository.GetAllAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task FindAsync_ShouldReturnFilteredEntities()
        {
            var contacts = new List<ContactDomain>
            {
                new ContactDomain { Id = 1, DDD = "11", Name = "João", Email = "joao@example.com", Phone = "111111111" },
                new ContactDomain { Id = 2, DDD = "21", Name = "Maria", Email = "maria@example.com", Phone = "222222222" }
            };

            await _repository.AddRangeAsync(contacts);

            Expression<Func<ContactDomain, bool>> predicate = c => c.Name == "Maria";
            var result = await _repository.FindAsync(predicate);

            Assert.Single(result);
            Assert.Equal("Maria", result.First().Name);
        }

        [Fact]
        public async Task RemoveAsync_ShouldRemoveEntity()
        {
            var contact = new ContactDomain { Id = 1, DDD = "11", Name = "João", Email = "joao@example.com", Phone = "111111111" };
            await _repository.AddAsync(contact);

            await _repository.RemoveAsync(contact);
            var result = await _repository.GetByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task RemoveRangeAsync_ShouldRemoveEntities()
        {
            var contacts = new List<ContactDomain>
            {
                new ContactDomain { Id = 1, DDD = "11", Name = "João", Email = "joao@example.com", Phone = "111111111" },
                new ContactDomain { Id = 2, DDD = "21", Name = "Maria", Email = "maria@example.com", Phone = "222222222" }
            };

            await _repository.AddRangeAsync(contacts);

            await _repository.RemoveRangeAsync(contacts);

            var result = await _repository.GetAllAsync();
            Assert.Empty(result);
        }
        
        [Fact]
        public async Task Update_ShouldUpdateEntity()
        {
            var contact = new ContactDomain { Id = 1, DDD = "11", Name = "João", Email = "joao@example.com", Phone = "111111111" };
            await _repository.AddAsync(contact);

            contact.Name = "João Silva";
            _repository.Update(contact);

            var result = await _repository.GetByIdAsync(1);
            Assert.Equal("João Silva", result.Name);

        }
    }
}
