import type { MaintenanceRecord, MaintenanceStatus } from "../../types/maintenance";
import { StatusBadge } from "../common/StatusBadge";

interface Props {
  records: MaintenanceRecord[];
  onAdvance: (record: MaintenanceRecord) => void;
}

const NEXT_STATUS: Record<MaintenanceStatus, MaintenanceStatus | null> = {
  Scheduled: "InProgress",
  InProgress: "Completed",
  Completed: null,
};

function formatDate(iso: string): string {
  return new Date(iso).toLocaleDateString("en-CA", {
    year: "numeric",
    month: "short",
    day: "numeric",
  });
}

function money(n: number): string {
  return n.toLocaleString("en-CA", { style: "currency", currency: "CAD" });
}

export function MaintenanceList({ records, onAdvance }: Props) {
  return (
    <div>
      {records.map((r) => {
        const canAdvance = NEXT_STATUS[r.status] !== null;
        return (
          <div key={r.id} className="mrow">
            <div>
              <p className="desc">{r.description}</p>
              <p className="when">
                {formatDate(r.serviceDate)} · {r.mileageAtService.toLocaleString("en-CA")} km
              </p>
            </div>
            <span className="cost">{money(r.cost)}</span>
            <StatusBadge status={r.status} onClick={canAdvance ? () => onAdvance(r) : undefined} />
          </div>
        );
      })}
    </div>
  );
}
