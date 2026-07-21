using Xunit;

namespace VehicleMaintenanceTracker.Tests.Infrastructure;

[Collection("Database")]
public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected readonly DatabaseFixture Fixture;

    protected IntegrationTestBase(DatabaseFixture fixture)
    {
        Fixture = fixture;
    }

    // Runs before every test method: guarantees a clean database.
    public async Task InitializeAsync() => await Fixture.ResetDatabaseAsync();

    public Task DisposeAsync() => Task.CompletedTask;
}