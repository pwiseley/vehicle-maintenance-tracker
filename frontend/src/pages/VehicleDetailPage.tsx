import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { vehiclesApi } from "../api/vehicles";
import { maintenanceApi } from "../api/maintenance";
import { useAsync } from "../hooks/useAsync";
import { MaintenanceList } from "../components/maintenance/MaintenanceList";
import { MaintenanceForm } from "../components/maintenance/MaintenanceForm";
import { Loading } from "../components/common/Loading";
import { ErrorMessage } from "../components/common/ErrorMessage";
import { EmptyState } from "../components/common/EmptyState";
import { VEHICLE_TYPE_LABEL } from "../types/vehicle";
import type { CreateMaintenanceRequest, MaintenanceRecord, MaintenanceStatus } from "../types/maintenance";

const NEXT_STATUS: Record<MaintenanceStatus, MaintenanceStatus | null> = {
  Scheduled: "InProgress",
  InProgress: "Completed",
  Completed: null,
};

function money(n: number): string {
  return n.toLocaleString("en-CA", { style: "currency", currency: "CAD" });
}

export function VehicleDetailPage() {
  const { id } = useParams<{ id: string }>();
  const vehicleId = Number(id);
  const navigate = useNavigate();
  const [showForm, setShowForm] = useState(false);

  const vehicle = useAsync(() => vehiclesApi.getById(vehicleId), [vehicleId]);
  const records = useAsync(() => maintenanceApi.getForVehicle(vehicleId), [vehicleId]);

  useEffect(() => {
    document.title = vehicle.data
      ? `${vehicle.data.plateNumber} · Vehicle Maintenance Tracker`
      : "Vehicle · Vehicle Maintenance Tracker";
  }, [vehicle.data]);

  const handleCreate = async (request: CreateMaintenanceRequest) => {
    await maintenanceApi.createForVehicle(vehicleId, request);
    records.reload();
    vehicle.reload();
  };

  const handleAdvance = async (record: MaintenanceRecord) => {
    const next = NEXT_STATUS[record.status];
    if (!next) return;
    await maintenanceApi.updateStatus(record.id, { status: next });
    records.reload();
  };

  if (vehicle.loading) return <Loading />;
  if (vehicle.error) return <ErrorMessage message={vehicle.error} />;
  if (!vehicle.data) return null;

  const v = vehicle.data;
  const totalSpent = (records.data ?? []).reduce((sum, r) => sum + r.cost, 0);

  return (
    <>
      <button className="back" onClick={() => navigate("/vehicles")}>
        <i className="bi bi-arrow-left" />
        Back to vehicles
      </button>

      <div className="panel-card" style={{ marginBottom: "20px" }}>
        <span className="vtype">{VEHICLE_TYPE_LABEL[v.type]}</span>
        <h1 style={{ fontSize: "24px", fontWeight: 600, margin: "4px 0 0" }}>{v.plateNumber}</h1>
        <p style={{ color: "var(--muted)", margin: "2px 0 0", fontSize: "14px" }}>
          {v.make} {v.model}
        </p>

        <div className="detail-grid">
          <div className="dcell">
            <p className="k">Year</p>
            <p className="v">{v.year}</p>
          </div>
          <div className="dcell">
            <p className="k">Mileage</p>
            <p className="v">{v.mileage.toLocaleString("en-CA")}</p>
          </div>
          <div className="dcell">
            <p className="k">Records</p>
            <p className="v">{v.maintenanceCount}</p>
          </div>
          <div className="dcell">
            <p className="k">Total spent</p>
            <p className="v">{money(totalSpent)}</p>
          </div>
        </div>
      </div>

      <div className="toolbar">
        <h2>Maintenance history</h2>
        <button className="btn-accent" onClick={() => setShowForm(true)}>
          <i className="bi bi-plus-lg" />
          Log maintenance
        </button>
      </div>

      {records.loading && <Loading />}
      {records.error && <ErrorMessage message={records.error} />}
      {records.data && records.data.length === 0 && (
        <EmptyState icon="bi-clipboard-check" message="No maintenance logged yet." />
      )}
      {records.data && records.data.length > 0 && (
        <MaintenanceList records={records.data} onAdvance={handleAdvance} />
      )}

      <MaintenanceForm show={showForm} onClose={() => setShowForm(false)} onSubmit={handleCreate} />
    </>
  );
}
