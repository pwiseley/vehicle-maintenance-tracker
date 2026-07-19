import { useNavigate } from "react-router-dom";
import type { Vehicle } from "../../types/vehicle";
import { VEHICLE_TYPE_LABEL } from "../../types/vehicle";

export function VehicleList({ vehicles }: { vehicles: Vehicle[] }) {
  const navigate = useNavigate();

  return (
    <div>
      {vehicles.map((v) => (
        <div key={v.id} className="vrow" onClick={() => navigate(`/vehicles/${v.id}`)}>
          <div>
            <div className="plate">{v.plateNumber}</div>
            <div className="sub">
              {v.make} {v.model}
            </div>
          </div>
          <div className="meta">
            <span className="vtype">{VEHICLE_TYPE_LABEL[v.type]}</span>
            <br />
            {v.maintenanceCount} record{v.maintenanceCount === 1 ? "" : "s"}
          </div>
        </div>
      ))}
    </div>
  );
}
