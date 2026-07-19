import { useEffect, useState } from "react";
import { vehiclesApi } from "../api/vehicles";
import { useAsync } from "../hooks/useAsync";
import { VehicleList } from "../components/vehicles/VehicleList";
import { VehicleForm } from "../components/vehicles/VehicleForm";
import { Loading } from "../components/common/Loading";
import { ErrorMessage } from "../components/common/ErrorMessage";
import { EmptyState } from "../components/common/EmptyState";
import type { CreateVehicleRequest } from "../types/vehicle";

export function VehiclesPage() {
  const { data, loading, error, reload } = useAsync(() => vehiclesApi.getAll());
  const [showForm, setShowForm] = useState(false);

  useEffect(() => {
    document.title = "Vehicles · Vehicle Maintenance Tracker";
  }, []);

  const handleCreate = async (request: CreateVehicleRequest) => {
    await vehiclesApi.create(request);
    reload();
  };

  const count = data?.length ?? 0;

  return (
    <>
      <div className="page-head">
        <h1>Vehicles</h1>
        <p>Every vehicle in the fleet.</p>
      </div>

      <div className="toolbar">
        <h2>
          {count} vehicle{count === 1 ? "" : "s"}
        </h2>
        <button className="btn-accent" onClick={() => setShowForm(true)}>
          <i className="bi bi-plus-lg" />
          Add vehicle
        </button>
      </div>

      {loading && <Loading />}
      {error && <ErrorMessage message={error} />}
      {data && data.length === 0 && (
        <EmptyState icon="bi-truck" message="No vehicles yet. Add your first one." />
      )}
      {data && data.length > 0 && <VehicleList vehicles={data} />}

      <VehicleForm show={showForm} onClose={() => setShowForm(false)} onSubmit={handleCreate} />
    </>
  );
}
