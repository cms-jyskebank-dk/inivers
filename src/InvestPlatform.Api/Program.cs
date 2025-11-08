// Program.cs //

using InvestPlatform.Application.RiskProfile;
using InvestPlatform.Infrastructure.RiskProfile;
using InvestPlatform.Application.Customer;
using InvestPlatform.Infrastructure.Customer;
using InvestPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();

// Add Entity Framework and SQL Server
builder.Services.AddDbContext<InvestPlatformDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? "Server=(localdb)\\mssqllocaldb;Database=InvestPlatformDb;Trusted_Connection=true;MultipleActiveResultSets=true"));

builder.Services.AddScoped<ICustomerRepository, EfCustomerRepository>();
builder.Services.AddSingleton<IRiskProfileRepository, InMemoryRiskProfileRepository>();
builder.Services.AddTransient<RegisterCustomerUseCase>();
builder.Services.AddTransient<AssessRiskProfileUseCase>();

var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<InvestPlatformDbContext>();
    context.Database.EnsureCreated();
}




// Customer endpoints
app.MapPost("/customer", async (RegisterCustomerUseCase useCase, CustomerDto dto) =>
{
    var id = await useCase.RegisterAsync(dto.FullName, dto.Nationality, dto.Address);
    return Results.Created($"/customer/{id}", new { CustomerID = id });
});

app.MapGet("/customer/{id:guid}", async (ICustomerRepository repo, Guid id) =>
{
    var customer = await repo.GetByIdAsync(id);
    return customer is not null ? Results.Ok(customer) : Results.NotFound();
});

app.MapGet("/customer", async (ICustomerRepository repo) =>
{
    var customers = await repo.GetAllAsync();
    return Results.Ok(customers);
});

app.MapPut("/customer/{id:guid}", async (ICustomerRepository repo, Guid id, CustomerDto dto) =>
{
    var customer = await repo.GetByIdAsync(id);
    if (customer is null) return Results.NotFound();
    var updated = customer with
    {
        FullName = dto.FullName,
        Nationality = dto.Nationality,
        Address = dto.Address,
        LastUpdatedDate = DateTime.UtcNow
    };
    await repo.UpdateAsync(updated);
    return Results.Ok(updated);
});

app.MapDelete("/customer/{id:guid}", async (ICustomerRepository repo, Guid id) =>
{
    var result = await repo.RemoveAsync(id);
    return result ? Results.NoContent() : Results.NotFound();
});


app.Run();

// DTOs og records
public record CustomerDto(string FullName, string Nationality, string Address);
