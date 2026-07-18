using Microsoft.AspNetCore.Mvc;
using VehicleMaintenanceTracker.Dtos.Maintenance;
using VehicleMaintenanceTracker.Services.Interfaces;

namespace VehicleMaintenanceTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaintenanceController : ControllerBase
{
    private readonly IMaintenanceService _maintenanceService;

    public MaintenanceController(IMaintenanceService maintenanceService)
    {
        _maintenanceService = maintenanceService;
    }

    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MaintenanceResponse>> UpdateStatus(int id, UpdateStatusRequest request)
    {
        return Ok(await _maintenanceService.UpdateStatusAsync(id, request));
    }
}