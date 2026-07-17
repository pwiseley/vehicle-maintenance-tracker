namespace VehicleMaintenanceTracker.Entities;

public enum VehicleType
{
    Car,
    Truck,
    SnowPlow,
    ServiceVehicle
}

public class Vehicle
{
    public int Id { get; set; }
    public required string PlateNumber { get; set; }
    public required string Make { get; set; }
    public required string Model { get; set; }
    public int Year { get; set; }
    public int Mileage { get; set; }
    public VehicleType Type { get; set; }

    public List<MaintenanceRecord> MaintenanceRecords { get; set; } = [];
}