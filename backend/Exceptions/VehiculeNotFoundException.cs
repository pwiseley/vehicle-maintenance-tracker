namespace VehicleMaintenanceTracker.Exceptions;

public class VehicleNotFoundException : NotFoundException
{
    public VehicleNotFoundException(int id) : base($"No vehicle exists with id {id}.") { }
}