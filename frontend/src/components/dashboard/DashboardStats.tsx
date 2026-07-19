import type { DashboardResponse } from "../../types/dashboard";
import { STATUS_LABEL, MAINTENANCE_STATUSES } from "../../types/maintenance";

function money(n: number): string {
  return n.toLocaleString("en-CA", {
    style: "currency",
    currency: "CAD",
    maximumFractionDigits: 0,
  });
}

export function DashboardStats({ data }: { data: DashboardResponse }) {
  const total = MAINTENANCE_STATUSES.reduce(
    (sum, s) => sum + (data.maintenanceByStatus[s] ?? 0),
    0,
  );

  const pct = (s: string) =>
    total === 0 ? 0 : ((data.maintenanceByStatus[s] ?? 0) / total) * 100;

  const colors: Record<string, string> = {
    Scheduled: "var(--sched)",
    InProgress: "var(--prog)",
    Completed: "var(--done)",
  };

  return (
    <>
      <div className="stats">
        <div className="stat">
          <p className="lbl">Vehicles</p>
          <p className="val">{data.totalVehicles}</p>
        </div>
        <div className="stat">
          <p className="lbl">Total cost</p>
          <p className="val">{money(data.totalMaintenanceCost)}</p>
        </div>
        <div className="stat">
          <p className="lbl">Upcoming</p>
          <p className="val">{data.upcomingMaintenanceCount}</p>
        </div>
        <div className="stat">
          <p className="lbl">Completed</p>
          <p className="val">{data.maintenanceByStatus.Completed ?? 0}</p>
        </div>
      </div>

      <div className="panel-card">
        <h3>Maintenance by status</h3>
        <div className="statusbar">
          {MAINTENANCE_STATUSES.map((s) => (
            <div key={s} style={{ width: `${pct(s)}%`, background: colors[s] }} />
          ))}
        </div>
        <div className="legend">
          {MAINTENANCE_STATUSES.map((s) => (
            <span key={s}>
              <span className="dot" style={{ background: colors[s] }} />
              {STATUS_LABEL[s]} · {data.maintenanceByStatus[s] ?? 0}
            </span>
          ))}
        </div>
      </div>
    </>
  );
}
