using InvestPlatform.Infrastructure.Customer;
using InvestPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;
using CustomerEntity = InvestPlatform.Domain.Customer.Customer;

namespace InvestPlatform.UnitTests;

public class EfCustomerRepositoryTests
{
    private InvestPlatformDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<InvestPlatformDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        return new InvestPlatformDbContext(options);
    }

    [Fact]
    public async Task AddAndGetCustomer_PersistsInDatabase()
    {
        using var context = CreateInMemoryContext();
        var repo = new EfCustomerRepository(context);
        
        var customer = new CustomerEntity
        {
            CustomerID = Guid.NewGuid(),
            FullName = "Integration Test",
            Nationality = "DK",
            Address = "Testvej 1",
            CreatedDate = DateTime.UtcNow,
            LastUpdatedDate = DateTime.UtcNow
        };
        
        await repo.AddAsync(customer);
        var loaded = await repo.GetByIdAsync(customer.CustomerID);
        
        Assert.NotNull(loaded);
        Assert.Equal(customer.FullName, loaded!.FullName);
        Assert.Equal(customer.Address, loaded.Address);
    }

    [Fact]
    public async Task UpdateCustomer_UpdatesDatabase()
    {
        using var context = CreateInMemoryContext();
        var repo = new EfCustomerRepository(context);
        
        var customer = new CustomerEntity
        {
            CustomerID = Guid.NewGuid(),
            FullName = "Integration Test",
            Nationality = "DK",
            Address = "Testvej 1",
            CreatedDate = DateTime.UtcNow,
            LastUpdatedDate = DateTime.UtcNow
        };
        
        await repo.AddAsync(customer);
        var updated = customer with { FullName = "Opdateret", Address = "Nyvej 2", LastUpdatedDate = DateTime.UtcNow };
        await repo.UpdateAsync(updated);
        var loaded = await repo.GetByIdAsync(customer.CustomerID);
        
        Assert.Equal("Opdateret", loaded!.FullName);
        Assert.Equal("Nyvej 2", loaded.Address);
    }

    [Fact]
    public async Task RemoveCustomer_DeletesFromDatabase()
    {
        using var context = CreateInMemoryContext();
        var repo = new EfCustomerRepository(context);
        
        var customer = new CustomerEntity
        {
            CustomerID = Guid.NewGuid(),
            FullName = "Integration Test",
            Nationality = "DK",
            Address = "Testvej 1",
            CreatedDate = DateTime.UtcNow,
            LastUpdatedDate = DateTime.UtcNow
        };
        
        await repo.AddAsync(customer);
        var removed = await repo.RemoveAsync(customer.CustomerID);
        
        Assert.True(removed);
        var loaded = await repo.GetByIdAsync(customer.CustomerID);
        Assert.Null(loaded);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllCustomers()
    {
        using var context = CreateInMemoryContext();
        var repo = new EfCustomerRepository(context);
        
        var customer1 = new CustomerEntity
        {
            CustomerID = Guid.NewGuid(),
            FullName = "Test1",
            Nationality = "DK",
            Address = "Avej 1",
            CreatedDate = DateTime.UtcNow,
            LastUpdatedDate = DateTime.UtcNow
        };
        var customer2 = new CustomerEntity
        {
            CustomerID = Guid.NewGuid(),
            FullName = "Test2",
            Nationality = "DK",
            Address = "Bvej 2",
            CreatedDate = DateTime.UtcNow,
            LastUpdatedDate = DateTime.UtcNow
        };
        
        await repo.AddAsync(customer1);
        await repo.AddAsync(customer2);
        var all = await repo.GetAllAsync();
        
        Assert.Contains(all, c => c.FullName == "Test1");
        Assert.Contains(all, c => c.FullName == "Test2");
    }
}