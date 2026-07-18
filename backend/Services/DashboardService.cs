using Microsoft.EntityFrameworkCore;
using VehicleMaintenanceTracker.Data;
using VehicleMaintenanceTracker.Dtos.Dashboard;
using VehicleMaintenanceTracker.Entities;
using VehicleMaintenanceTracker.Services.Interfaces;

namespace VehicleMaintenanceTracker.Services;

public class DashboardService : IDashboardService
{
    private const int UpcomingWindowInDays = 30;

    private readonly AppDbContext _context;

    public DashboardService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardResponse> GetAsync()
    {
        var now = DateTime.UtcNow;
        var horizon = now.AddDays(UpcomingWindowInDays);

        var totalVehicles = await _context.Vehicles.CountAsync();

        var totalCost = await _context.MaintenanceRecords
            .SumAsync(m => (decimal?)m.Cost) ?? 0m;

        var upcomingCount = await _context.MaintenanceRecords
            .CountAsync(m => m.Status == MaintenanceStatus.Scheduled
                          && m.ServiceDate >= now
                          && m.ServiceDate <= horizon);

        var byStatus = await _context.MaintenanceRecords
            .GroupBy(m => m.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Status.ToString(), x => x.Count);

        foreach (var status in Enum.GetValues<MaintenanceStatus>())
        {
            byStatus.TryAdd(status.ToString(), 0);
        }

        return new DashboardResponse(totalVehicles, totalCost, upcomingCount, byStatus);
    }
}