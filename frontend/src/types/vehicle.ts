export type VehicleType = "Car" | "Truck" | "SnowPlow" | "ServiceVehicle";

export interface Vehicle {
  id: number;
  plateNumber: string;
  make: string;
  model: string;
  year: number;
  mileage: number;
  type: VehicleType;
  maintenanceCount: number;
}

export interface CreateVehicleRequest {
  plateNumber: string;
  make: string;
  model: string;
  year: number;
  mileage: number;
  type: VehicleType;
}

export const VEHICLE_TYPES: VehicleType[] = ["Car", "Truck", "SnowPlow", "ServiceVehicle"];

export const VEHICLE_TYPE_LABEL: Record<VehicleType, string> = {
  Car: "Car",
  Truck: "Truck",
  SnowPlow: "Snow plow",
  ServiceVehicle: "Service vehicle",
};
