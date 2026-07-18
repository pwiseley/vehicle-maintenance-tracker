using VehicleMaintenanceTracker.Dtos.Dashboard;

namespace VehicleMaintenanceTracker.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardResponse> GetAsync();
}