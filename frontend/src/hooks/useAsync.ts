import { useCallback, useEffect, useState } from "react";
import { ApiError } from "../api/client";

interface AsyncState<T> {
  data: T | null;
  loading: boolean;
  error: string | null;
}

export function useAsync<T>(fetcher: () => Promise<T>, deps: unknown[] = []) {
  const [state, setState] = useState<AsyncState<T>>({
    data: null,
    loading: true,
    error: null,
  });

  const run = useCallback(() => {
    setState({ data: null, loading: true, error: null });
    fetcher()
      .then((data) => setState({ data, loading: false, error: null }))
      .catch((err) => {
        const message =
          err instanceof ApiError ? err.message : "Something went wrong. Please try again.";
        setState({ data: null, loading: false, error: message });
      });
      // eslint-disable-next-line react-hooks/exhaustive-deps, react-hooks/use-memo
  }, deps);

  useEffect(() => {
    run();
  }, [run]);

  return { ...state, reload: run };
}
