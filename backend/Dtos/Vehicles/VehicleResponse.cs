using VehicleMaintenanceTracker.Entities;

namespace VehicleMaintenanceTracker.Dtos.Vehicles;

public record VehicleResponse(
    int Id,
    string PlateNumber,
    string Make,
    string Model,
    int Year,
    int Mileage,
    VehicleType Type,
    int MaintenanceCount
)
{
    public static VehicleResponse FromEntity(Vehicle vehicle) => new(
        vehicle.Id,
        vehicle.PlateNumber,
        vehicle.Make,
        vehicle.Model,
        vehicle.Year,
        vehicle.Mileage,
        vehicle.Type,
        vehicle.MaintenanceRecords.Count
    );
}