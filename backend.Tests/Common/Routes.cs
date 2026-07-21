namespace VehicleMaintenanceTracker.Tests.Common;

public static class Routes
{
    public static class Vehicles
    {
        public const string Base = "/api/vehicles";

        public static string ById(int id) => $"{Base}/{id}";
        public static string Maintenance(int vehicleId) => $"{Base}/{vehicleId}/maintenance";
    }

    public static class Maintenance
    {
        public const string Base = "/api/maintenance";

        public static string Status(int id) => $"{Base}/{id}/status";
    }

    public static class Dashboard
    {
        public const string Base = "/api/dashboard";
    }
}