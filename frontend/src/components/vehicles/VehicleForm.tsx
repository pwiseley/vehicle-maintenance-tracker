import { useState } from "react";
import { Modal } from "react-bootstrap";
import type { CreateVehicleRequest, VehicleType } from "../../types/vehicle";
import { VEHICLE_TYPES, VEHICLE_TYPE_LABEL } from "../../types/vehicle";

interface Props {
  show: boolean;
  onClose: () => void;
  onSubmit: (request: CreateVehicleRequest) => Promise<void>;
}

const EMPTY: CreateVehicleRequest = {
  plateNumber: "",
  make: "",
  model: "",
  year: new Date().getFullYear(),
  mileage: 0,
  type: "Car",
};

export function VehicleForm({ show, onClose, onSubmit }: Props) {
  const [form, setForm] = useState<CreateVehicleRequest>(EMPTY);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const update = <K extends keyof CreateVehicleRequest>(key: K, value: CreateVehicleRequest[K]) =>
    setForm((f) => ({ ...f, [key]: value }));

  const handleSubmit = async () => {
    setSaving(true);
    setError(null);
    try {
      await onSubmit(form);
      setForm(EMPTY);
      onClose();
    } catch (err) {
      setError(err instanceof Error ? err.message : "Could not save vehicle.");
    } finally {
      setSaving(false);
    }
  };

  return (
    <Modal show={show} onHide={onClose} centered dialogClassName="vmt-modal">
      <Modal.Body style={{ padding: "22px" }}>
        <h3 style={{ fontSize: "19px", fontWeight: 600, margin: "0 0 16px" }}>Add vehicle</h3>

        {error && (
          <div style={{ color: "#8A3A2E", fontSize: "13px", marginBottom: "12px" }}>{error}</div>
        )}

        <div style={{ marginBottom: "13px" }}>
          <div className="field-label">Plate number</div>
          <input
            className="form-control"
            placeholder="ABC123"
            value={form.plateNumber}
            onChange={(e) => update("plateNumber", e.target.value)}
          />
        </div>

        <div style={{ display: "flex", gap: "10px" }}>
          <div style={{ flex: 1, marginBottom: "13px" }}>
            <div className="field-label">Make</div>
            <input
              className="form-control"
              placeholder="Ford"
              value={form.make}
              onChange={(e) => update("make", e.target.value)}
            />
          </div>
          <div style={{ flex: 1, marginBottom: "13px" }}>
            <div className="field-label">Model</div>
            <input
              className="form-control"
              placeholder="F-150"
              value={form.model}
              onChange={(e) => update("model", e.target.value)}
            />
          </div>
        </div>

        <div style={{ display: "flex", gap: "10px" }}>
          <div style={{ flex: 1, marginBottom: "13px" }}>
            <div className="field-label">Year</div>
            <input
              className="form-control"
              type="number"
              value={form.year}
              onChange={(e) => update("year", Number(e.target.value))}
            />
          </div>
          <div style={{ flex: 1, marginBottom: "13px" }}>
            <div className="field-label">Mileage</div>
            <input
              className="form-control"
              type="number"
              value={form.mileage}
              onChange={(e) => update("mileage", Number(e.target.value))}
            />
          </div>
        </div>

        <div style={{ marginBottom: "13px" }}>
          <div className="field-label">Type</div>
          <select
            className="form-select"
            value={form.type}
            onChange={(e) => update("type", e.target.value as VehicleType)}
          >
            {VEHICLE_TYPES.map((t) => (
              <option key={t} value={t}>
                {VEHICLE_TYPE_LABEL[t]}
              </option>
            ))}
          </select>
        </div>

        <div style={{ display: "flex", justifyContent: "flex-end", gap: "9px", marginTop: "18px" }}>
          <button className="btn-ghost" onClick={onClose} disabled={saving}>
            Cancel
          </button>
          <button className="btn-accent" onClick={handleSubmit} disabled={saving}>
            {saving ? "Saving..." : "Add vehicle"}
          </button>
        </div>
      </Modal.Body>
    </Modal>
  );
}
