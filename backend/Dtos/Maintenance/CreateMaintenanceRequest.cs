using System.ComponentModel.DataAnnotations;

namespace VehicleMaintenanceTracker.Dtos.Maintenance;

public record CreateMaintenanceRequest(
    [Required][StringLength(500, MinimumLength = 3)] string Description,
    [Required] DateTime ServiceDate,
    [Range(0, 1000000)] decimal Cost,
    [Range(0, int.MaxValue)] int MileageAtService
);