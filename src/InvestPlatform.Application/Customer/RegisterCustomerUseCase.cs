using InvestPlatform.Domain.Customer;

namespace InvestPlatform.Application.Customer;

public class RegisterCustomerUseCase
{
    private readonly ICustomerRepository _repository;
    public RegisterCustomerUseCase(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> RegisterAsync(string fullName, string nationality, string address)
    {
        var customer = new InvestPlatform.Domain.Customer.Customer
        {
            CustomerID = Guid.NewGuid(),
            FullName = fullName,
            Nationality = nationality,
            Address = address,
            CreatedDate = DateTime.UtcNow,
            LastUpdatedDate = DateTime.UtcNow
        };
        await _repository.AddAsync(customer);
        return customer.CustomerID;
    }
}
