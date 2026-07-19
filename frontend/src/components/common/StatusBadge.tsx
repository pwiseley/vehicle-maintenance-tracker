import type { MaintenanceStatus } from "../../types/maintenance";
import { STATUS_LABEL } from "../../types/maintenance";

const NEXT_LABEL: Record<MaintenanceStatus, string | null> = {
    Scheduled: "In progress",
    InProgress: "Completed",
    Completed: null,
};

interface Props {
    status: MaintenanceStatus;
    onClick?: () => void;
}

export function StatusBadge({ status, onClick }: Props) {
    const next = NEXT_LABEL[status];
    const clickable = Boolean(onClick);

    return (
        <button
            className={`badge-st b-${status}${clickable ? " badge-clickable" : ""}`}
            onClick={onClick}
            disabled={!clickable}
            title={next ? `Click to mark as ${next}` : "Completed"}
        >
            {STATUS_LABEL[status]}
            {clickable && <i className="bi bi-arrow-right" style={{ marginLeft: 5, fontSize: 10 }} />}
        </button>
    );
}