using Microsoft.EntityFrameworkCore;
using VehicleMaintenanceTracker.Data;
using VehicleMaintenanceTracker.Dtos.Maintenance;
using VehicleMaintenanceTracker.Entities;
using VehicleMaintenanceTracker.Exceptions;
using VehicleMaintenanceTracker.Services.Interfaces;

namespace VehicleMaintenanceTracker.Services;

public class MaintenanceService : IMaintenanceService
{
    private readonly AppDbContext _context;

    public MaintenanceService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<MaintenanceResponse> CreateForVehicleAsync(int vehicleId, CreateMaintenanceRequest request)
    {
        var vehicleExists = await _context.Vehicles.AnyAsync(v => v.Id == vehicleId);

        if (!vehicleExists)
        {
            throw new VehicleNotFoundException(vehicleId);
        }

        var record = new MaintenanceRecord
        {
            VehicleId = vehicleId,
            Description = request.Description,
            ServiceDate = DateTime.SpecifyKind(request.ServiceDate, DateTimeKind.Utc),
            Cost = request.Cost,
            MileageAtService = request.MileageAtService,
            Status = MaintenanceStatus.Scheduled
        };

        _context.MaintenanceRecords.Add(record);
        await _context.SaveChangesAsync();

        return MaintenanceResponse.FromEntity(record);
    }

    public async Task<List<MaintenanceResponse>> GetByVehicleAsync(int vehicleId)
    {
        var vehicleExists = await _context.Vehicles.AnyAsync(v => v.Id == vehicleId);

        if (!vehicleExists)
        {
            throw new VehicleNotFoundException(vehicleId);
        }

        var records = await _context.MaintenanceRecords
            .Where(m => m.VehicleId == vehicleId)
            .AsNoTracking()
            .OrderByDescending(m => m.ServiceDate)
            .ToListAsync();

        return records.Select(MaintenanceResponse.FromEntity).ToList();
    }

    public async Task<MaintenanceResponse> UpdateStatusAsync(int id, UpdateStatusRequest request)
    {
        var record = await _context.MaintenanceRecords.FindAsync(id)
            ?? throw new MaintenanceRecordNotFoundException(id);

        record.Status = request.Status;
        await _context.SaveChangesAsync();

        return MaintenanceResponse.FromEntity(record);
    }
}