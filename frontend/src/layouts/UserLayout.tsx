import { Link, Outlet } from "react-router-dom";
import NavigationMenu from "../components/NavigationMenu";

const userMenuItems = [
  { label: "Generate Recipe", to: "/user/generate" },
  { label: "Saved Recipes", to: "/user/saved" },
];

export default function UserLayout() {
  return (
    <div className="page">
      <header className="layout-header">
        <h1>User Area</h1>
        <Link className="link" to="/">
          Change role
        </Link>
      </header>
      <NavigationMenu items={userMenuItems} ariaLabel="User navigation" />
      <Outlet />
    </div>
  );
}
