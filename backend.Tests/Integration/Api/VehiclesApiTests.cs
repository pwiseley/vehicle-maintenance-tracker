using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using VehicleMaintenanceTracker.Dtos.Maintenance;
using VehicleMaintenanceTracker.Dtos.Vehicles;
using VehicleMaintenanceTracker.Tests.Common;
using VehicleMaintenanceTracker.Tests.Infrastructure;
using Xunit;

namespace VehicleMaintenanceTracker.Tests.Integration.Api;

public class VehiclesControllerTests : IntegrationTestBase
{
    private const string VehiclesEndpoint = "/api/vehicles";
    private const string InvalidPlateNumber = "";

    private readonly HttpClient _client;

    public VehiclesControllerTests(DatabaseFixture fixture) : base(fixture)
    {
        _client = fixture.CreateClient();
    }

    [Fact]
    public async Task Create_WithValidRequest_Returns201WithLocation()
    {
        var request = TestData.AVehicleRequest();

        var response = await _client.PostAsJsonAsync(VehiclesEndpoint, request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_WithDuplicatePlateNumber_Returns409()
    {
        var request = TestData.AVehicleRequest();
        await _client.PostAsJsonAsync(VehiclesEndpoint, request);

        var response = await _client.PostAsJsonAsync(VehiclesEndpoint, request);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Create_WithInvalidRequest_Returns400()
    {
        var request = TestData.AVehicleRequest(plateNumber: InvalidPlateNumber);

        var response = await _client.PostAsJsonAsync(VehiclesEndpoint, request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetAll_WithNoVehicles_ReturnsEmptyArray()
    {
        var response = await _client.GetAsync(VehiclesEndpoint);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var vehicles = await response.Content.ReadFromJsonAsync<List<VehicleResponse>>(JsonConfig.Options);
        vehicles.Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_WithExistingVehicle_Returns200()
    {
        var created = await CreateVehicleAsync(TestData.AVehicleRequest());

        var response = await _client.GetAsync(Routes.Vehicles.ById(created!.Id));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_WithNonExistentVehicle_Returns404()
    {
        var response = await _client.GetAsync(Routes.Vehicles.ById(TestData.NonExistentId));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddMaintenance_ToExistingVehicle_Returns201()
    {
        var created = await CreateVehicleAsync(TestData.AVehicleRequest());

        var response = await _client.PostAsJsonAsync(
            Routes.Vehicles.Maintenance(created!.Id), TestData.AMaintenanceRequest());

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task AddMaintenance_ToNonExistentVehicle_Returns404()
    {
        var response = await _client.PostAsJsonAsync(
            Routes.Vehicles.Maintenance(TestData.NonExistentId), TestData.AMaintenanceRequest());

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetMaintenance_ForNonExistentVehicle_Returns404()
    {
        var response = await _client.GetAsync(Routes.Vehicles.Maintenance(TestData.NonExistentId));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task<VehicleResponse?> CreateVehicleAsync(CreateVehicleRequest request)
    {
        var response = await _client.PostAsJsonAsync(VehiclesEndpoint, request);
        return await response.Content.ReadFromJsonAsync<VehicleResponse>(JsonConfig.Options);
    }
}