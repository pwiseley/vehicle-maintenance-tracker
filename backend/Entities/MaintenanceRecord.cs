namespace VehicleMaintenanceTracker.Entities;

public enum MaintenanceStatus
{
    Scheduled,
    InProgress,
    Completed
}

public class MaintenanceRecord
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }

    public required string Description { get; set; }
    public DateTime ServiceDate { get; set; }
    public decimal Cost { get; set; }
    public int MileageAtService { get; set; }
    public MaintenanceStatus Status { get; set; } = MaintenanceStatus.Scheduled;
}