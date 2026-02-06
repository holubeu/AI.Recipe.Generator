import { getRole } from "./roleStorage";

const API_BASE_URL = "https://localhost:7292";

export const apiBaseUrl = API_BASE_URL;

export async function getJson<T>(path: string): Promise<T> {
  const role = getRole();
  const headers: HeadersInit = role ? { "X-User-Role": role } : {};
  const response = await fetch(`${API_BASE_URL}${path}`, { headers });
  if (!response.ok) {
    throw new Error(`Request failed: ${response.status}`);
  }

  return response.json() as Promise<T>;
}
