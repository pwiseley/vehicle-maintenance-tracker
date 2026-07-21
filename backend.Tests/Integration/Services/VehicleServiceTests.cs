using FluentAssertions;
using VehicleMaintenanceTracker.Exceptions;
using VehicleMaintenanceTracker.Services;
using VehicleMaintenanceTracker.Tests.Common;
using VehicleMaintenanceTracker.Tests.Infrastructure;
using Xunit;

namespace VehicleMaintenanceTracker.Tests.Infrastructure.Services;

public class VehicleServiceTests : IntegrationTestBase
{
    public VehicleServiceTests(DatabaseFixture fixture) : base(fixture) { }

    [Fact]
    public async Task CreateAsync_WithValidRequest_PersistsAndReturnsVehicle()
    {
        await using var context = Fixture.CreateContext();
        var service = new VehicleService(context);
        var request = TestData.AVehicleRequest();

        var result = await service.CreateAsync(request);

        result.Id.Should().BeGreaterThan(0);
        result.PlateNumber.Should().Be(TestData.DefaultPlateNumber);
        result.MaintenanceCount.Should().Be(0);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicatePlateNumber_ThrowsPlateNumberAlreadyExists()
    {
        await using var context = Fixture.CreateContext();
        var service = new VehicleService(context);
        await service.CreateAsync(TestData.AVehicleRequest());

        var act = () => service.CreateAsync(TestData.AVehicleRequest());

        await act.Should().ThrowAsync<PlateNumberAlreadyExistsException>();
    }

    [Fact]
    public async Task GetAllAsync_WithNoVehicles_ReturnsEmptyList()
    {
        await using var context = Fixture.CreateContext();
        var service = new VehicleService(context);

        var result = await service.GetAllAsync();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_WithMultipleVehicles_ReturnsAllOrderedByPlateNumber()
    {
        await using var context = Fixture.CreateContext();
        var service = new VehicleService(context);
        await service.CreateAsync(TestData.AVehicleRequest(plateNumber: TestData.SecondPlateNumber));
        await service.CreateAsync(TestData.AVehicleRequest(plateNumber: TestData.DefaultPlateNumber));

        var result = await service.GetAllAsync();

        result.Should().HaveCount(2);
        result.Should().BeInAscendingOrder(v => v.PlateNumber);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ReturnsVehicle()
    {
        await using var context = Fixture.CreateContext();
        var service = new VehicleService(context);
        var created = await service.CreateAsync(TestData.AVehicleRequest());

        var result = await service.GetByIdAsync(created.Id);

        result.Id.Should().Be(created.Id);
        result.PlateNumber.Should().Be(TestData.DefaultPlateNumber);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistentId_ThrowsVehicleNotFound()
    {
        await using var context = Fixture.CreateContext();
        var service = new VehicleService(context);

        var act = () => service.GetByIdAsync(TestData.NonExistentId);

        await act.Should().ThrowAsync<VehicleNotFoundException>();
    }
}