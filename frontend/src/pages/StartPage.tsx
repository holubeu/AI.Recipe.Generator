import { useNavigate } from "react-router-dom";
import type { UserRole } from "../lib/roleStorage";

type StartPageProps = {
  onSelectRole: (role: UserRole) => void;
};

export default function StartPage({ onSelectRole }: StartPageProps) {
  const navigate = useNavigate();

  const handleSelect = (role: UserRole) => {
    onSelectRole(role);
    navigate(role === "user" ? "/user" : "/admin");
  };

  return (
    <div className="page">
      <header className="header">
        <h1>AI Recipe Generator</h1>
        <p>Select a role to continue.</p>
      </header>

      <section className="card">
        <h2>Choose your role</h2>
        <div className="role-buttons">
          <button className="button" onClick={() => handleSelect("user")}>
            User
          </button>
          <button className="button secondary" onClick={() => handleSelect("admin")}>
            Admin
          </button>
        </div>
      </section>
    </div>
  );
}
