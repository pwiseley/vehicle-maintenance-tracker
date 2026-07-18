namespace VehicleMaintenanceTracker.Dtos.Dashboard;

public record DashboardResponse(
    int TotalVehicles,
    decimal TotalMaintenanceCost,
    int UpcomingMaintenanceCount,
    Dictionary<string, int> MaintenanceByStatus
);