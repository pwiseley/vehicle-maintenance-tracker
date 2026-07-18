using VehicleMaintenanceTracker.Dtos.Vehicle;

namespace VehicleMaintenanceTracker.Services.Interfaces;

public interface IVehicleService
{
    Task<VehicleResponse> CreateAsync(CreateVehicleRequest request);
    Task<List<VehicleResponse>> GetAllAsync();
    Task<VehicleResponse> GetByIdAsync(int id);
}