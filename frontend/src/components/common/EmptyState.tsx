export function EmptyState({ icon, message }: { icon: string; message: string }) {
  return (
    <div className="state-box">
      <i className={`bi ${icon}`} />
      <div>{message}</div>
    </div>
  );
}
