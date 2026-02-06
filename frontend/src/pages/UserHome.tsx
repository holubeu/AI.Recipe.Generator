import { apiBaseUrl } from "../lib/apiClient";
import { getRole } from "../lib/roleStorage";

export default function UserHome() {
  const role = getRole();

  return (
    <section className="card">
      <h2>Welcome, User</h2>
      <p>This is the default user page.</p>
      <p className="muted">Role: {role ?? "unknown"}</p>
      <p className="muted">API Base URL: {apiBaseUrl}</p>
    </section>
  );
}
