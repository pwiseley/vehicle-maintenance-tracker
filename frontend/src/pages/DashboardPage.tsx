import { useEffect } from "react";
import { dashboardApi } from "../api/dashboard";
import { useAsync } from "../hooks/useAsync";
import { DashboardStats } from "../components/dashboard/DashboardStats";
import { Loading } from "../components/common/Loading";
import { ErrorMessage } from "../components/common/ErrorMessage";

export function DashboardPage() {
  const { data, loading, error } = useAsync(() => dashboardApi.get());

  useEffect(() => {
    document.title = "Dashboard · Vehicle Maintenance Tracker";
  }, []);

  return (
    <>
      <div className="page-head">
        <h1>Fleet overview</h1>
        <p>Maintenance status across all vehicles.</p>
      </div>

      {loading && <Loading />}
      {error && <ErrorMessage message={error} />}
      {data && <DashboardStats data={data} />}
    </>
  );
}
