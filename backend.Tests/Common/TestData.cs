using VehicleMaintenanceTracker.Dtos.Maintenance;
using VehicleMaintenanceTracker.Dtos.Vehicles;
using VehicleMaintenanceTracker.Entities;

namespace VehicleMaintenanceTracker.Tests.Common;

public static class TestData
{
    public const string DefaultPlateNumber = "ABC123";
    public const string SecondPlateNumber = "XYZ789";
    public const string DefaultMake = "Toyota";
    public const string DefaultModel = "Corolla";
    public const int DefaultYear = 2022;
    public const int DefaultMileage = 45000;
    public const VehicleType DefaultType = VehicleType.Car;

    public const string DefaultDescription = "Oil change and filter replacement";
    public const decimal DefaultCost = 89.99m;
    public const int DefaultServiceMileage = 45000;

    public const int NonExistentId = 999_999;

    public static CreateVehicleRequest AVehicleRequest(
        string plateNumber = DefaultPlateNumber,
        string make = DefaultMake,
        string model = DefaultModel,
        int year = DefaultYear,
        int mileage = DefaultMileage,
        VehicleType type = DefaultType) =>
        new(plateNumber, make, model, year, mileage, type);

    public static CreateMaintenanceRequest AMaintenanceRequest(
        string description = DefaultDescription,
        DateTime? serviceDate = null,
        decimal cost = DefaultCost,
        int mileageAtService = DefaultServiceMileage) =>
        new(description, serviceDate ?? DateTime.UtcNow, cost, mileageAtService);
}