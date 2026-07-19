import { api } from "./client";
import type {
  MaintenanceRecord,
  CreateMaintenanceRequest,
  UpdateStatusRequest,
} from "../types/maintenance";

export const maintenanceApi = {
  getForVehicle: (vehicleId: number) =>
    api.get<MaintenanceRecord[]>(`/vehicles/${vehicleId}/maintenance`),
  createForVehicle: (vehicleId: number, request: CreateMaintenanceRequest) =>
    api.post<MaintenanceRecord>(`/vehicles/${vehicleId}/maintenance`, request),
  updateStatus: (id: number, request: UpdateStatusRequest) =>
    api.patch<MaintenanceRecord>(`/maintenance/${id}/status`, request),
};
