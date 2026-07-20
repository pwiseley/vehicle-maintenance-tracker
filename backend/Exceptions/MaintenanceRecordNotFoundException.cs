namespace VehicleMaintenanceTracker.Exceptions;

public class MaintenanceRecordNotFoundException : NotFoundException
{
    public MaintenanceRecordNotFoundException(int id) : base($"No maintenance record exists with id {id}.") { }
}