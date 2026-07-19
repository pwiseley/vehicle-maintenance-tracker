const BASE_URL = "/api";

interface ProblemDetails {
  title?: string;
  detail?: string;
  status?: number;
  errors?: Record<string, string[]>;
}

export class ApiError extends Error {
  status: number;

  constructor(message: string, status: number) {
    super(message);
    this.name = "ApiError";
    this.status = status;
  }
}

async function parseError(response: Response): Promise<never> {
  let message = `Request failed (${response.status})`;

  try {
    const problem: ProblemDetails = await response.json();

    if (problem.errors) {
      const first = Object.values(problem.errors)[0];
      if (first && first.length > 0) {
        message = first[0];
      }
    } else if (problem.detail) {
      message = problem.detail;
    } else if (problem.title) {
      message = problem.title;
    }
  } catch {
    // response body was not JSON, keep the default message
  }

  throw new ApiError(message, response.status);
}

async function request<T>(path: string, options?: RequestInit): Promise<T> {
  const response = await fetch(`${BASE_URL}${path}`, {
    headers: { "Content-Type": "application/json" },
    ...options,
  });

  if (!response.ok) {
    return parseError(response);
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return response.json() as Promise<T>;
}

export const api = {
  get: <T>(path: string) => request<T>(path),
  post: <T>(path: string, body: unknown) =>
    request<T>(path, { method: "POST", body: JSON.stringify(body) }),
  patch: <T>(path: string, body: unknown) =>
    request<T>(path, { method: "PATCH", body: JSON.stringify(body) }),
};
