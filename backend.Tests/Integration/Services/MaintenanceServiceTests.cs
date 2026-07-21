using FluentAssertions;
using VehicleMaintenanceTracker.Dtos.Maintenance;
using VehicleMaintenanceTracker.Entities;
using VehicleMaintenanceTracker.Exceptions;
using VehicleMaintenanceTracker.Services;
using VehicleMaintenanceTracker.Tests.Common;
using VehicleMaintenanceTracker.Tests.Infrastructure;
using Xunit;

namespace VehicleMaintenanceTracker.Tests.Infrastructure.Services;

public class MaintenanceServiceTests : IntegrationTestBase
{
    public MaintenanceServiceTests(DatabaseFixture fixture) : base(fixture) { }

    private async Task<int> SeedVehicleAsync()
    {
        await using var context = Fixture.CreateContext();
        var vehicleService = new VehicleService(context);
        var vehicle = await vehicleService.CreateAsync(TestData.AVehicleRequest());
        return vehicle.Id;
    }

    [Fact]
    public async Task CreateForVehicleAsync_WithExistingVehicle_PersistsRecordAsScheduled()
    {
        var vehicleId = await SeedVehicleAsync();
        await using var context = Fixture.CreateContext();
        var service = new MaintenanceService(context);

        var result = await service.CreateForVehicleAsync(vehicleId, TestData.AMaintenanceRequest());

        result.Id.Should().BeGreaterThan(0);
        result.VehicleId.Should().Be(vehicleId);
        result.Status.Should().Be(MaintenanceStatus.Scheduled);
    }

    [Fact]
    public async Task CreateForVehicleAsync_WithNonExistentVehicle_ThrowsVehicleNotFound()
    {
        await using var context = Fixture.CreateContext();
        var service = new MaintenanceService(context);

        var act = () => service.CreateForVehicleAsync(TestData.NonExistentId, TestData.AMaintenanceRequest());

        await act.Should().ThrowAsync<VehicleNotFoundException>();
    }

    [Fact]
    public async Task CreateForVehicleAsync_StoresServiceDateAsUtc()
    {
        var vehicleId = await SeedVehicleAsync();
        await using var context = Fixture.CreateContext();
        var service = new MaintenanceService(context);
        var localDate = new DateTime(2026, 6, 1, 10, 0, 0, DateTimeKind.Unspecified);

        var result = await service.CreateForVehicleAsync(
            vehicleId, TestData.AMaintenanceRequest(serviceDate: localDate));

        result.ServiceDate.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public async Task GetByVehicleAsync_WithNonExistentVehicle_ThrowsVehicleNotFound()
    {
        await using var context = Fixture.CreateContext();
        var service = new MaintenanceService(context);

        var act = () => service.GetByVehicleAsync(TestData.NonExistentId);

        await act.Should().ThrowAsync<VehicleNotFoundException>();
    }

    [Fact]
    public async Task GetByVehicleAsync_ReturnsRecordsOrderedByServiceDateDescending()
    {
        var vehicleId = await SeedVehicleAsync();
        await using var context = Fixture.CreateContext();
        var service = new MaintenanceService(context);

        var older = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var newer = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc);
        await service.CreateForVehicleAsync(vehicleId, TestData.AMaintenanceRequest(serviceDate: older));
        await service.CreateForVehicleAsync(vehicleId, TestData.AMaintenanceRequest(serviceDate: newer));

        var result = await service.GetByVehicleAsync(vehicleId);

        result.Should().HaveCount(2);
        result.Should().BeInDescendingOrder(r => r.ServiceDate);
    }

    [Fact]
    public async Task UpdateStatusAsync_WithExistingRecord_UpdatesStatus()
    {
        var vehicleId = await SeedVehicleAsync();
        await using var context = Fixture.CreateContext();
        var service = new MaintenanceService(context);
        var record = await service.CreateForVehicleAsync(vehicleId, TestData.AMaintenanceRequest());

        var result = await service.UpdateStatusAsync(
            record.Id, new UpdateStatusRequest(MaintenanceStatus.Completed));

        result.Status.Should().Be(MaintenanceStatus.Completed);
    }

    [Fact]
    public async Task UpdateStatusAsync_WithNonExistentRecord_ThrowsMaintenanceRecordNotFound()
    {
        await using var context = Fixture.CreateContext();
        var service = new MaintenanceService(context);

        var act = () => service.UpdateStatusAsync(
            TestData.NonExistentId, new UpdateStatusRequest(MaintenanceStatus.Completed));

        await act.Should().ThrowAsync<MaintenanceRecordNotFoundException>();
    }
}