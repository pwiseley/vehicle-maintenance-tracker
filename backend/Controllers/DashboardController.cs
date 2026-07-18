using Microsoft.AspNetCore.Mvc;
using VehicleMaintenanceTracker.Dtos.Dashboard;
using VehicleMaintenanceTracker.Services.Interfaces;

namespace VehicleMaintenanceTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<ActionResult<DashboardResponse>> Get()
    {
        return Ok(await _dashboardService.GetAsync());
    }
}