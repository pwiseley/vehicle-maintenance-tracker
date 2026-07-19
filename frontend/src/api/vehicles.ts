import { api } from "./client";
import type { Vehicle, CreateVehicleRequest } from "../types/vehicle";

export const vehiclesApi = {
  getAll: () => api.get<Vehicle[]>("/vehicles"),
  getById: (id: number) => api.get<Vehicle>(`/vehicles/${id}`),
  create: (request: CreateVehicleRequest) => api.post<Vehicle>("/vehicles", request),
};
