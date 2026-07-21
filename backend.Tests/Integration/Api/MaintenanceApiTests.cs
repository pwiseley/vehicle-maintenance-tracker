using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using VehicleMaintenanceTracker.Dtos.Maintenance;
using VehicleMaintenanceTracker.Dtos.Vehicles;
using VehicleMaintenanceTracker.Entities;
using VehicleMaintenanceTracker.Tests.Common;
using VehicleMaintenanceTracker.Tests.Infrastructure;
using Xunit;

namespace VehicleMaintenanceTracker.Tests.Integration.Api;

public class MaintenanceControllerTests : IntegrationTestBase
{
    private readonly HttpClient _client;

    public MaintenanceControllerTests(DatabaseFixture fixture) : base(fixture)
    {
        _client = fixture.CreateClient();
    }

    [Fact]
    public async Task UpdateStatus_OnExistingRecord_Returns200WithNewStatus()
    {
        var record = await SeedMaintenanceAsync();
        var request = new UpdateStatusRequest(MaintenanceStatus.Completed);

        var response = await _client.PatchAsJsonAsync(Routes.Maintenance.Status(record!.Id), request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updated = await response.Content.ReadFromJsonAsync<MaintenanceResponse>(JsonConfig.Options);
        updated!.Status.Should().Be(MaintenanceStatus.Completed);
    }

    [Fact]
    public async Task UpdateStatus_OnNonExistentRecord_Returns404()
    {
        var request = new UpdateStatusRequest(MaintenanceStatus.Completed);

        var response = await _client.PatchAsJsonAsync(
            Routes.Maintenance.Status(TestData.NonExistentId), request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task<MaintenanceResponse?> SeedMaintenanceAsync()
    {
        var vehicleResponse = await _client.PostAsJsonAsync(Routes.Vehicles.Base, TestData.AVehicleRequest());
        var vehicle = await vehicleResponse.Content.ReadFromJsonAsync<VehicleResponse>(JsonConfig.Options);

        var maintenanceResponse = await _client.PostAsJsonAsync(
            Routes.Vehicles.Maintenance(vehicle!.Id), TestData.AMaintenanceRequest());
        return await maintenanceResponse.Content.ReadFromJsonAsync<MaintenanceResponse>(JsonConfig.Options);
    }
}