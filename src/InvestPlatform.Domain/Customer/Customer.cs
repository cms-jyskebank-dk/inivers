namespace InvestPlatform.Domain.Customer;

public sealed record Customer
{
    public Guid CustomerID { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Nationality { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public DateTime CreatedDate { get; init; }
    public DateTime LastUpdatedDate { get; set; }
}