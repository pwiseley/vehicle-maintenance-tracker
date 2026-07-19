export interface DashboardResponse {
  totalVehicles: number;
  totalMaintenanceCost: number;
  upcomingMaintenanceCount: number;
  maintenanceByStatus: Record<string, number>;
}
