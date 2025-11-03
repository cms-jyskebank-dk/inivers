using InvestPlatform.Application.Customer;
using InvestPlatform.Domain.Customer;

namespace InvestPlatform.Infrastructure.Customer;

public class InMemoryCustomerRepository : ICustomerRepository
{
    private readonly Dictionary<Guid, InvestPlatform.Domain.Customer.Customer> _customers = new();

    public Task<IEnumerable<InvestPlatform.Domain.Customer.Customer>> GetAllAsync()
    {
        // Address er allerede en del af Customer-recorden, så ingen ændring nødvendig
        return Task.FromResult(_customers.Values.AsEnumerable());
    }

    public Task<bool> RemoveAsync(Guid id) => Task.FromResult(_customers.Remove(id));

    public Task<InvestPlatform.Domain.Customer.Customer?> GetByIdAsync(Guid customerId)
        => Task.FromResult(_customers.TryGetValue(customerId, out var customer) ? customer : null);

    public Task AddAsync(InvestPlatform.Domain.Customer.Customer customer)
    {
        _customers[customer.CustomerID] = customer;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(InvestPlatform.Domain.Customer.Customer customer)
    {
        _customers[customer.CustomerID] = customer;
        return Task.CompletedTask;
    }
}
