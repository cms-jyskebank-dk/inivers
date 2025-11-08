using InvestPlatform.Domain.Customer;

namespace InvestPlatform.Application.Customer;

public interface ICustomerRepository
{
    Task<InvestPlatform.Domain.Customer.Customer?> GetByIdAsync(Guid customerId);
    Task AddAsync(InvestPlatform.Domain.Customer.Customer customer);
    Task UpdateAsync(InvestPlatform.Domain.Customer.Customer customer);
    Task<IEnumerable<InvestPlatform.Domain.Customer.Customer>> GetAllAsync();
    Task<bool> RemoveAsync(Guid id);
}