import { API_BASE_URL } from "./config";

const buildUrl = (path) => {
  if (!path.startsWith("/")) return `${API_BASE_URL}/${path}`;
  return `${API_BASE_URL}${path}`;
};

const readResponse = async (response) => {
  const contentType = response.headers.get("content-type") || "";
  if (contentType.includes("application/json")) {
    return response.json();
  }
  return response.text();
};

const apiFetch = async (path, options = {}) => {
  const response = await fetch(buildUrl(path), options);
  const payload = await readResponse(response);

  if (!response.ok) {
    const message =
      (typeof payload === "string" && payload.trim()) ||
      (payload && payload.message) ||
      response.statusText ||
      "Request failed";
    throw new Error(message);
  }

  return payload;
};

export const api = {
  get: (path) => apiFetch(path),
  post: (path, body) =>
    apiFetch(path, {
      method: "POST",
      headers: body ? { "Content-Type": "application/json" } : undefined,
      body: body ? JSON.stringify(body) : undefined,
    }),
  put: (path, body) =>
    apiFetch(path, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(body),
    }),
  del: (path) =>
    apiFetch(path, {
      method: "DELETE",
    }),
};
