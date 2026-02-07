import { NavLink } from "react-router-dom";

type NavigationItem = {
  label: string;
  to: string;
};

type NavigationMenuProps = {
  items: NavigationItem[];
  ariaLabel: string;
};

export default function NavigationMenu({ items, ariaLabel }: NavigationMenuProps) {
  return (
    <nav className="menu" aria-label={ariaLabel}>
      <ul className="menu-list">
        {items.map((item) => (
          <li key={item.to}>
            <NavLink
              to={item.to}
              className={({ isActive }) =>
                isActive ? "menu-link active" : "menu-link"
              }
            >
              {item.label}
            </NavLink>
          </li>
        ))}
      </ul>
    </nav>
  );
}
