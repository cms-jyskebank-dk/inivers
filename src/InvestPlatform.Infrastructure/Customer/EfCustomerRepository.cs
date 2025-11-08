using InvestPlatform.Application.Customer;
using InvestPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using CustomerEntity = InvestPlatform.Domain.Customer.Customer;

namespace InvestPlatform.Infrastructure.Customer;

public class EfCustomerRepository : ICustomerRepository
{
    private readonly InvestPlatformDbContext _context;

    public EfCustomerRepository(InvestPlatformDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerEntity?> GetByIdAsync(Guid customerId)
    {
        return await _context.Customers.FindAsync(customerId);
    }

    public async Task AddAsync(CustomerEntity customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CustomerEntity customer)
    {
        // First, check if there's already a tracked entity with the same key
        var trackedEntity = _context.Customers.Local.FirstOrDefault(e => e.CustomerID == customer.CustomerID);
        if (trackedEntity != null)
        {
            // Update the tracked entity
            _context.Entry(trackedEntity).CurrentValues.SetValues(customer);
        }
        else
        {
            // No tracked entity, so update the provided entity
            _context.Customers.Update(customer);
        }
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CustomerEntity>> GetAllAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<bool> RemoveAsync(Guid id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
            return false;

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return true;
    }
}