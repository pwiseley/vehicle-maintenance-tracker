using Microsoft.EntityFrameworkCore;
using VehicleMaintenanceTracker.Data;
using VehicleMaintenanceTracker.Dtos.Vehicle;
using VehicleMaintenanceTracker.Exceptions;
using VehicleMaintenanceTracker.Services.Interfaces;

namespace VehicleMaintenanceTracker.Services;

public class VehicleService : IVehicleService
{
    private readonly AppDbContext _context;

    public VehicleService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<VehicleResponse> CreateAsync(CreateVehicleRequest request)
    {
        var plateExists = await _context.Vehicles
            .AnyAsync(v => v.PlateNumber == request.PlateNumber);

        if (plateExists)
        {
            throw new PlateNumberAlreadyExistsException(request.PlateNumber);
        }

        var vehicle = new Entities.Vehicle
        {
            PlateNumber = request.PlateNumber,
            Make = request.Make,
            Model = request.Model,
            Year = request.Year,
            Mileage = request.Mileage,
            Type = request.Type
        };

        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();

        return VehicleResponse.FromEntity(vehicle);
    }

    public async Task<List<VehicleResponse>> GetAllAsync()
    {
        var vehicles = await _context.Vehicles
            .Include(v => v.MaintenanceRecords)
            .AsNoTracking()
            .OrderBy(v => v.PlateNumber)
            .ToListAsync();

        return vehicles.Select(VehicleResponse.FromEntity).ToList();
    }

    public async Task<VehicleResponse> GetByIdAsync(int id)
    {
        var vehicle = await _context.Vehicles
            .Include(v => v.MaintenanceRecords)
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == id)
            ?? throw new VehicleNotFoundException(id);

        return VehicleResponse.FromEntity(vehicle);
    }
}