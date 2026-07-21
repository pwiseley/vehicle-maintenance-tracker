using FluentAssertions;
using VehicleMaintenanceTracker.Dtos.Maintenance;
using VehicleMaintenanceTracker.Entities;
using VehicleMaintenanceTracker.Services;
using VehicleMaintenanceTracker.Tests.Common;
using VehicleMaintenanceTracker.Tests.Infrastructure;
using Xunit;

namespace VehicleMaintenanceTracker.Tests.Integration.Services;

public class DashboardServiceTests : IntegrationTestBase
{
    private const int ExpectedEmptyCount = 0;
    private const decimal ExpectedEmptyCost = 0m;
    private const int WithinUpcomingWindowDays = 10;
    private const int BeyondUpcomingWindowDays = 60;

    public DashboardServiceTests(DatabaseFixture fixture) : base(fixture) { }

    private async Task<int> SeedVehicleAsync()
    {
        await using var context = Fixture.CreateContext();
        var vehicleService = new VehicleService(context);
        var vehicle = await vehicleService.CreateAsync(TestData.AVehicleRequest());
        return vehicle.Id;
    }

    private async Task SeedMaintenanceAsync(int vehicleId, DateTime serviceDate, MaintenanceStatus status)
    {
        await using var context = Fixture.CreateContext();
        var service = new MaintenanceService(context);
        var record = await service.CreateForVehicleAsync(
            vehicleId, TestData.AMaintenanceRequest(serviceDate: serviceDate));
        await service.UpdateStatusAsync(record.Id, new UpdateStatusRequest(status));
    }

    [Fact]
    public async Task GetAsync_WithEmptyDatabase_ReturnsZeroedTotals()
    {
        await using var context = Fixture.CreateContext();
        var service = new DashboardService(context);

        var result = await service.GetAsync();

        result.TotalVehicles.Should().Be(ExpectedEmptyCount);
        result.TotalMaintenanceCost.Should().Be(ExpectedEmptyCost);
        result.UpcomingMaintenanceCount.Should().Be(ExpectedEmptyCount);
    }

    [Fact]
    public async Task GetAsync_AlwaysIncludesEveryStatusKey()
    {
        await using var context = Fixture.CreateContext();
        var service = new DashboardService(context);

        var result = await service.GetAsync();

        foreach (var status in Enum.GetValues<MaintenanceStatus>())
        {
            result.MaintenanceByStatus.Should().ContainKey(status.ToString());
        }
    }

    [Fact]
    public async Task GetAsync_CountsAllVehicles()
    {
        await using var context = Fixture.CreateContext();
        var vehicleService = new VehicleService(context);
        await vehicleService.CreateAsync(TestData.AVehicleRequest(plateNumber: TestData.DefaultPlateNumber));
        await vehicleService.CreateAsync(TestData.AVehicleRequest(plateNumber: TestData.SecondPlateNumber));
        var service = new DashboardService(context);

        var result = await service.GetAsync();

        result.TotalVehicles.Should().Be(2);
    }

    [Fact]
    public async Task GetAsync_SumsMaintenanceCostAcrossAllRecords()
    {
        var vehicleId = await SeedVehicleAsync();
        await SeedMaintenanceAsync(vehicleId, DateTime.UtcNow, MaintenanceStatus.Completed);
        await SeedMaintenanceAsync(vehicleId, DateTime.UtcNow, MaintenanceStatus.Completed);
        await using var context = Fixture.CreateContext();
        var service = new DashboardService(context);

        var result = await service.GetAsync();

        result.TotalMaintenanceCost.Should().Be(TestData.DefaultCost * 2);
    }

    [Fact]
    public async Task GetAsync_CountsScheduledWithinWindowAsUpcoming()
    {
        var vehicleId = await SeedVehicleAsync();
        await SeedMaintenanceAsync(
            vehicleId, DateTime.UtcNow.AddDays(WithinUpcomingWindowDays), MaintenanceStatus.Scheduled);
        await using var context = Fixture.CreateContext();
        var service = new DashboardService(context);

        var result = await service.GetAsync();

        result.UpcomingMaintenanceCount.Should().Be(1);
    }

    [Fact]
    public async Task GetAsync_ExcludesScheduledBeyondWindowFromUpcoming()
    {
        var vehicleId = await SeedVehicleAsync();
        await SeedMaintenanceAsync(
            vehicleId, DateTime.UtcNow.AddDays(BeyondUpcomingWindowDays), MaintenanceStatus.Scheduled);
        await using var context = Fixture.CreateContext();
        var service = new DashboardService(context);

        var result = await service.GetAsync();

        result.UpcomingMaintenanceCount.Should().Be(ExpectedEmptyCount);
    }

    [Fact]
    public async Task GetAsync_ExcludesNonScheduledFromUpcoming()
    {
        var vehicleId = await SeedVehicleAsync();
        await SeedMaintenanceAsync(
            vehicleId, DateTime.UtcNow.AddDays(WithinUpcomingWindowDays), MaintenanceStatus.Completed);
        await using var context = Fixture.CreateContext();
        var service = new DashboardService(context);

        var result = await service.GetAsync();

        result.UpcomingMaintenanceCount.Should().Be(ExpectedEmptyCount);
    }
}