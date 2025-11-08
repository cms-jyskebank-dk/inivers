# Migration from SQLite to Entity Framework with SQL Server

This document describes the changes made to replace SQLite with Entity Framework Core and SQL Server.

## Changes Made

### 1. Package References Updated

**Infrastructure Project (`InvestPlatform.Infrastructure.csproj`)**:
- Removed: `Microsoft.Data.Sqlite (Version 8.0.0)`
- Added: 
  - `Microsoft.EntityFrameworkCore (Version 8.0.11)`
  - `Microsoft.EntityFrameworkCore.SqlServer (Version 8.0.11)`
  - `Microsoft.EntityFrameworkCore.Design (Version 8.0.11)`

**Unit Tests Project (`InvestPlatform.UnitTests.csproj`)**:
- Removed: `Microsoft.Data.Sqlite (Version 8.0.0)`
- Added: 
  - `Microsoft.EntityFrameworkCore.InMemory (Version 8.0.11)`
  - `Microsoft.NET.Test.Sdk (Version 17.8.0)`
- Added: `ImplicitUsings` and `Nullable` enabled

### 2. Files Added

- `src/InvestPlatform.Infrastructure/Data/InvestPlatformDbContext.cs` - Entity Framework DbContext
- `src/InvestPlatform.Infrastructure/Customer/EfCustomerRepository.cs` - Entity Framework repository implementation
- `tests/InvestPlatform.UnitTests/EfCustomerRepositoryTests.cs` - Updated unit tests using EF In-Memory provider

### 3. Files Removed

- `src/InvestPlatform.Infrastructure/Customer/SqliteCustomerRepository.cs`
- `tests/InvestPlatform.UnitTests/SqliteCustomerRepositoryTests.cs`

### 4. Configuration Changes

**Program.cs**:
- Added Entity Framework services registration
- Added SQL Server connection string configuration
- Added database initialization on startup
- Updated dependency injection to use `EfCustomerRepository`
- Simplified API endpoints (removed SQLite-specific code)

**Connection Strings**:
- Added connection string configuration in `appsettings.json` and `appsettings.Development.json`
- Default connection: `Server=(localdb)\\mssqllocaldb;Database=InvestPlatformDb;Trusted_Connection=true;MultipleActiveResultSets=true`

### 5. Application Interface Updates

**ICustomerRepository.cs**:
- Added missing methods: `GetAllAsync()` and `RemoveAsync(Guid id)`

## Architecture Benefits

1. **Better ORM Support**: Entity Framework provides a more robust ORM with change tracking, migrations, and LINQ support
2. **Production Database**: SQL Server is more suitable for production environments than SQLite
3. **Scalability**: SQL Server supports concurrent access and better performance for larger datasets
4. **Clean Architecture Maintained**: The changes follow clean architecture principles with proper separation of concerns

## Database Schema

The Entity Framework implementation creates the following table structure:

```sql
CREATE TABLE [Customers] (
    [CustomerID] uniqueidentifier NOT NULL,
    [FullName] nvarchar(200) NOT NULL,
    [Nationality] nvarchar(50) NOT NULL,
    [Address] nvarchar(500) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [LastUpdatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([CustomerID])
);
```

## Running the Application

1. **Prerequisites**: SQL Server LocalDB should be installed (comes with Visual Studio)
2. **Database Creation**: The database is automatically created on first run using `EnsureCreated()`
3. **Connection**: The application uses the connection string from `appsettings.json`

## Testing

All existing tests have been migrated to use Entity Framework Core In-Memory provider:
- Tests use isolated in-memory databases for each test
- Entity tracking issues have been resolved in the repository implementation
- All tests pass successfully

## Future Enhancements

For production use, consider:
1. **Migrations**: Use EF Migrations instead of `EnsureCreated()` for database schema management
2. **Connection Pooling**: Configure connection pooling for better performance
3. **Logging**: Add structured logging for EF queries
4. **Health Checks**: Add database health checks
5. **Configuration**: Use Azure SQL Database or SQL Server instance for production