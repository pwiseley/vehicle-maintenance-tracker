using VehicleMaintenanceTracker.Dtos.Maintenance;

namespace VehicleMaintenanceTracker.Services.Interfaces;

public interface IMaintenanceService
{
    Task<MaintenanceResponse> CreateForVehicleAsync(int vehicleId, CreateMaintenanceRequest request);
    Task<List<MaintenanceResponse>> GetByVehicleAsync(int vehicleId);
    Task<MaintenanceResponse> UpdateStatusAsync(int id, UpdateStatusRequest request);
}