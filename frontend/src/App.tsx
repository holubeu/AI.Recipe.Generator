import { useEffect, useState } from "react";
import { apiBaseUrl, getJson } from "./lib/apiClient";

type ApiStatus = {
  ok: boolean;
  message: string;
};

export default function App() {
  const [status, setStatus] = useState<ApiStatus>({
    ok: false,
    message: "Checking API..."
  });

  useEffect(() => {
    const check = async () => {
      try {
        await getJson("/openapi/v1.json");
        setStatus({ ok: true, message: "API reachable" });
      } catch {
        setStatus({ ok: false, message: "API not reachable" });
      }
    };

    check();
  }, []);

  return (
    <div className="page">
      <header className="header">
        <h1>AI Recipe Generator</h1>
        <p>Simple React SPA wired to the ASP.NET Core API.</p>
      </header>

      <section className="card">
        <h2>API Status</h2>
        <p>
          <strong>{status.message}</strong>
        </p>
        <p className="muted">Base URL: {apiBaseUrl}</p>
      </section>
    </div>
  );
}
