export type MaintenanceStatus = "Scheduled" | "InProgress" | "Completed";

export interface MaintenanceRecord {
  id: number;
  vehicleId: number;
  description: string;
  serviceDate: string;
  cost: number;
  mileageAtService: number;
  status: MaintenanceStatus;
}

export interface CreateMaintenanceRequest {
  description: string;
  serviceDate: string;
  cost: number;
  mileageAtService: number;
}

export interface UpdateStatusRequest {
  status: MaintenanceStatus;
}

export const MAINTENANCE_STATUSES: MaintenanceStatus[] = ["Scheduled", "InProgress", "Completed"];

export const STATUS_LABEL: Record<MaintenanceStatus, string> = {
  Scheduled: "Scheduled",
  InProgress: "In progress",
  Completed: "Completed",
};
