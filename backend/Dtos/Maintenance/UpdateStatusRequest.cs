using System.ComponentModel.DataAnnotations;
using VehicleMaintenanceTracker.Entities;

namespace VehicleMaintenanceTracker.Dtos.Maintenance;

public record UpdateStatusRequest(
    [EnumDataType(typeof(MaintenanceStatus))] MaintenanceStatus Status
);