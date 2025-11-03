// Program.cs //

using InvestPlatform.Application.RiskProfile;
using InvestPlatform.Infrastructure.RiskProfile;
using InvestPlatform.Application.Customer;
using InvestPlatform.Infrastructure.Customer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddOpenApiDocument();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<ICustomerRepository, InMemoryCustomerRepository>();
builder.Services.AddSingleton<IRiskProfileRepository, InMemoryRiskProfileRepository>();
builder.Services.AddTransient<RegisterCustomerUseCase>();
builder.Services.AddTransient<AssessRiskProfileUseCase>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Add OpenAPI 3.0 document serving middleware
    // Available at: http://localhost:<port>/swagger/v1/swagger.json
    app.UseOpenApi();

    // Add web UIs to interact with the document
    // Available at: http://localhost:<port>/swagger
    app.UseSwaggerUi(); // UseSwaggerUI is called only in Development.

    // Add ReDoc UI to interact with the document
    // Available at: http://localhost:<port>/redoc
    app.UseReDoc(options =>
    {
        options.Path = "/redoc";
    });
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
    // InMemory: Return all customers
    if (repo is InMemoryCustomerRepository memRepo)
        return Results.Ok(await memRepo.GetAllAsync());
    return Results.StatusCode(501);
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
    // InMemory: Remove customer
    if (repo is InMemoryCustomerRepository memRepo)
    {
        var result = await memRepo.RemoveAsync(id);
        return result ? Results.NoContent() : Results.NotFound();
    }
    return Results.StatusCode(501);
});


app.Run();

// DTOs og records
public record CustomerDto(string FullName, string Nationality, string Address);
