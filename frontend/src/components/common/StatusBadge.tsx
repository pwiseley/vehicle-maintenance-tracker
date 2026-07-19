import type { MaintenanceStatus } from "../../types/maintenance";
import { STATUS_LABEL } from "../../types/maintenance";

interface Props {
  status: MaintenanceStatus;
  onClick?: () => void;
}

export function StatusBadge({ status, onClick }: Props) {
  return (
    <button
      className={`badge-st b-${status}`}
      onClick={onClick}
      style={{ cursor: onClick ? "pointer" : "default" }}
      title={onClick ? "Click to advance status" : undefined}
    >
      {STATUS_LABEL[status]}
    </button>
  );
}
