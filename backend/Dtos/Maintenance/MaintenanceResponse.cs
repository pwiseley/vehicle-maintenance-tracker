using VehicleMaintenanceTracker.Entities;

namespace VehicleMaintenanceTracker.Dtos.Maintenance;

public record MaintenanceResponse(
    int Id,
    int VehicleId,
    string Description,
    DateTime ServiceDate,
    decimal Cost,
    int MileageAtService,
    MaintenanceStatus Status
)
{
    public static MaintenanceResponse FromEntity(MaintenanceRecord record) => new(
        record.Id,
        record.VehicleId,
        record.Description,
        record.ServiceDate,
        record.Cost,
        record.MileageAtService,
        record.Status
    );
}