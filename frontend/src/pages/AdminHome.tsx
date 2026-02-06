import { apiBaseUrl } from "../lib/apiClient";
import { getRole } from "../lib/roleStorage";

export default function AdminHome() {
  const role = getRole();

  return (
    <section className="card">
      <h2>Welcome, Admin</h2>
      <p>This is the default admin page.</p>
      <p className="muted">Role: {role ?? "unknown"}</p>
      <p className="muted">API Base URL: {apiBaseUrl}</p>
    </section>
  );
}
