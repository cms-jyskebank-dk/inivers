using System;
using InvestPlatform.Application.Customer;
using InvestPlatform.Infrastructure.Customer;
using InvestPlatform.Domain.Customer;
using Xunit;
using System.Threading.Tasks;

namespace InvestPlatform.UnitTests;

public class CustomerRepositoryTests
{
    [Fact]
    public async Task AddAndGetCustomer_ReturnsCustomer()
    {
        var repo = new InMemoryCustomerRepository();
        var customer = new Customer
        {
            CustomerID = Guid.NewGuid(),
            FullName = "Test Kunde",
            Nationality = "DK",
            CreatedDate = DateTime.UtcNow,
            LastUpdatedDate = DateTime.UtcNow
        };
        await repo.AddAsync(customer);
        var result = await repo.GetByIdAsync(customer.CustomerID);
        Assert.NotNull(result);
        Assert.Equal(customer.FullName, result!.FullName);
    }

    [Fact]
    public async Task UpdateCustomer_ChangesValues()
    {
        var repo = new InMemoryCustomerRepository();
        var customer = new Customer
        {
            CustomerID = Guid.NewGuid(),
            FullName = "Test Kunde",
            Nationality = "DK",
            CreatedDate = DateTime.UtcNow,
            LastUpdatedDate = DateTime.UtcNow
        };
        await repo.AddAsync(customer);
        var updated = customer with { FullName = "Opdateret", LastUpdatedDate = DateTime.UtcNow };
        await repo.UpdateAsync(updated);
        var result = await repo.GetByIdAsync(customer.CustomerID);
        Assert.Equal("Opdateret", result!.FullName);
    }

    [Fact]
    public async void RemoveCustomer_RemovesFromRepo()
    {
        var repo = new InMemoryCustomerRepository();
        var customer = new Customer
        {
            CustomerID = Guid.NewGuid(),
            FullName = "Test Kunde",
            Nationality = "DK",
            CreatedDate = DateTime.UtcNow,
            LastUpdatedDate = DateTime.UtcNow
        };
        repo.AddAsync(customer).Wait();
        var removed = await repo.RemoveAsync(customer.CustomerID);
        Assert.True(removed);
        var result = repo.GetByIdAsync(customer.CustomerID).Result;
        Assert.Null(result);
    }
}
