import { Link, Outlet } from "react-router-dom";

export default function AdminLayout() {
  return (
    <div className="page">
      <header className="layout-header">
        <h1>Admin Area</h1>
        <Link className="link" to="/">
          Change role
        </Link>
      </header>
      <Outlet />
    </div>
  );
}
