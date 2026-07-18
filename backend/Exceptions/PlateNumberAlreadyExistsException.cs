namespace VehicleMaintenanceTracker.Exceptions;

public class PlateNumberAlreadyExistsException : AlreadyExistsException
{
    public PlateNumberAlreadyExistsException(string plateNumber)
        : base($"A vehicle with plate number '{plateNumber}' already exists.") { }
}