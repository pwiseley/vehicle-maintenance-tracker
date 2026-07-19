export function Loading({ label = "Loading" }: { label?: string }) {
  return (
    <div className="state-box">
      <i className="bi bi-arrow-repeat" />
      <div>{label}...</div>
    </div>
  );
}
