using Microsoft.AspNetCore.Mvc;
using VehicleMaintenanceTracker.Dtos.Maintenance;
using VehicleMaintenanceTracker.Dtos.Vehicle;
using VehicleMaintenanceTracker.Services.Interfaces;

namespace VehicleMaintenanceTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;
    private readonly IMaintenanceService _maintenanceService;

    public VehiclesController(IVehicleService vehicleService, IMaintenanceService maintenanceService)
    {
        _vehicleService = vehicleService;
        _maintenanceService = maintenanceService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<VehicleResponse>> Create(CreateVehicleRequest request)
    {
        var vehicle = await _vehicleService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
    }

    [HttpGet]
    public async Task<ActionResult<List<VehicleResponse>>> GetAll()
    {
        return Ok(await _vehicleService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VehicleResponse>> GetById(int id)
    {
        return Ok(await _vehicleService.GetByIdAsync(id));
    }

    [HttpPost("{id:int}/maintenance")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MaintenanceResponse>> AddMaintenance(int id, CreateMaintenanceRequest request)
    {
        var record = await _maintenanceService.CreateForVehicleAsync(id, request);
        return Created($"/api/maintenance/{record.Id}", record);
    }

    [HttpGet("{id:int}/maintenance")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<MaintenanceResponse>>> GetMaintenance(int id)
    {
        return Ok(await _maintenanceService.GetByVehicleAsync(id));
    }
}