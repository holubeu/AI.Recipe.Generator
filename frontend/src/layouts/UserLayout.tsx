import { Link, Outlet } from "react-router-dom";

export default function UserLayout() {
  return (
    <div className="page">
      <header className="layout-header">
        <h1>User Area</h1>
        <Link className="link" to="/">
          Change role
        </Link>
      </header>
      <Outlet />
    </div>
  );
}
