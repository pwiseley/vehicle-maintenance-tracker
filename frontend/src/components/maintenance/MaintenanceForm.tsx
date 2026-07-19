import { useState } from "react";
import { Modal } from "react-bootstrap";
import type { CreateMaintenanceRequest } from "../../types/maintenance";

interface Props {
  show: boolean;
  onClose: () => void;
  onSubmit: (request: CreateMaintenanceRequest) => Promise<void>;
}

function emptyForm(): CreateMaintenanceRequest {
  return {
    description: "",
    serviceDate: new Date().toISOString().slice(0, 10),
    cost: 0,
    mileageAtService: 0,
  };
}

export function MaintenanceForm({ show, onClose, onSubmit }: Props) {
  const [form, setForm] = useState<CreateMaintenanceRequest>(emptyForm());
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const update = <K extends keyof CreateMaintenanceRequest>(
    key: K,
    value: CreateMaintenanceRequest[K],
  ) => setForm((f) => ({ ...f, [key]: value }));

  const handleSubmit = async () => {
    setSaving(true);
    setError(null);
    try {
      const payload: CreateMaintenanceRequest = {
        ...form,
        serviceDate: new Date(form.serviceDate).toISOString(),
      };
      await onSubmit(payload);
      setForm(emptyForm());
      onClose();
    } catch (err) {
      setError(err instanceof Error ? err.message : "Could not save record.");
    } finally {
      setSaving(false);
    }
  };

  return (
    <Modal show={show} onHide={onClose} centered dialogClassName="vmt-modal">
      <Modal.Body style={{ padding: "22px" }}>
        <h3 style={{ fontSize: "19px", fontWeight: 600, margin: "0 0 16px" }}>Log maintenance</h3>

        {error && (
          <div style={{ color: "#8A3A2E", fontSize: "13px", marginBottom: "12px" }}>{error}</div>
        )}

        <div style={{ marginBottom: "13px" }}>
          <div className="field-label">Description</div>
          <input
            className="form-control"
            placeholder="Oil change"
            value={form.description}
            onChange={(e) => update("description", e.target.value)}
          />
        </div>

        <div style={{ display: "flex", gap: "10px" }}>
          <div style={{ flex: 1, marginBottom: "13px" }}>
            <div className="field-label">Service date</div>
            <input
              className="form-control"
              type="date"
              value={form.serviceDate}
              onChange={(e) => update("serviceDate", e.target.value)}
            />
          </div>
          <div style={{ flex: 1, marginBottom: "13px" }}>
            <div className="field-label">Cost</div>
            <input
              className="form-control"
              type="number"
              placeholder="89.99"
              value={form.cost}
              onChange={(e) => update("cost", Number(e.target.value))}
            />
          </div>
        </div>

        <div style={{ marginBottom: "13px" }}>
          <div className="field-label">Mileage at service</div>
          <input
            className="form-control"
            type="number"
            placeholder="45000"
            value={form.mileageAtService}
            onChange={(e) => update("mileageAtService", Number(e.target.value))}
          />
        </div>

        <div style={{ display: "flex", justifyContent: "flex-end", gap: "9px", marginTop: "18px" }}>
          <button className="btn-ghost" onClick={onClose} disabled={saving}>
            Cancel
          </button>
          <button className="btn-accent" onClick={handleSubmit} disabled={saving}>
            {saving ? "Saving..." : "Save"}
          </button>
        </div>
      </Modal.Body>
    </Modal>
  );
}
