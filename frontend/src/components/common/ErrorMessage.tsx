export function ErrorMessage({ message }: { message: string }) {
  return (
    <div className="state-box">
      <i className="bi bi-exclamation-triangle" />
      <div>{message}</div>
    </div>
  );
}
