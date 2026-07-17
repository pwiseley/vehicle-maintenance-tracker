using System.ComponentModel.DataAnnotations;
using VehicleMaintenanceTracker.Entities;

namespace VehicleMaintenanceTracker.Dtos.Vehicles;

public record CreateVehicleRequest(
    [Required][StringLength(10, MinimumLength = 2)] string PlateNumber,
    [Required][StringLength(50)] string Make,
    [Required][StringLength(50)] string Model,
    [Range(1900, 2100)] int Year,
    [Range(0, int.MaxValue)] int Mileage,
    [EnumDataType(typeof(VehicleType))] VehicleType Type
);