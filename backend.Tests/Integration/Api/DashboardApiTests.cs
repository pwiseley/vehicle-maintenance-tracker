using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using VehicleMaintenanceTracker.Dtos.Dashboard;
using VehicleMaintenanceTracker.Tests.Common;
using VehicleMaintenanceTracker.Tests.Infrastructure;
using Xunit;

namespace VehicleMaintenanceTracker.Tests.Integration.Api;

public class DashboardControllerTests : IntegrationTestBase
{
    private readonly HttpClient _client;

    public DashboardControllerTests(DatabaseFixture fixture) : base(fixture)
    {
        _client = fixture.CreateClient();
    }

    [Fact]
    public async Task Get_WithEmptyDatabase_Returns200WithZeroedTotals()
    {
        var response = await _client.GetAsync(Routes.Dashboard.Base);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dashboard = await response.Content.ReadFromJsonAsync<DashboardResponse>(JsonConfig.Options);
        dashboard!.TotalVehicles.Should().Be(0);
    }

    [Fact]
    public async Task Get_ReflectsCreatedVehicles()
    {
        await _client.PostAsJsonAsync(Routes.Vehicles.Base, TestData.AVehicleRequest());

        var response = await _client.GetAsync(Routes.Dashboard.Base);

        var dashboard = await response.Content.ReadFromJsonAsync<DashboardResponse>(JsonConfig.Options);
        dashboard!.TotalVehicles.Should().Be(1);
    }
}