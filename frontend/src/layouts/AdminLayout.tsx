import { Link, Outlet } from "react-router-dom";
import NavigationMenu from "../components/NavigationMenu";

const adminMenuItems = [
  { label: "Ingredients", to: "/admin/ingredients" },
  { label: "Ingredient Categories", to: "/admin/ingredient-categories" },
  { label: "Api Keys", to: "/admin/api-keys" },
];

export default function AdminLayout() {
  return (
    <div className="page">
      <header className="layout-header">
        <h1>Admin Area</h1>
        <Link className="link" to="/">
          Change role
        </Link>
      </header>
      <NavigationMenu items={adminMenuItems} ariaLabel="Admin navigation" />
      <Outlet />
    </div>
  );
}
