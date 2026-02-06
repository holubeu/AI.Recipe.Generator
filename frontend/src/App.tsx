import { useState } from "react";
import { Navigate, Route, Routes } from "react-router-dom";
import AdminLayout from "./layouts/AdminLayout";
import UserLayout from "./layouts/UserLayout";
import AdminHome from "./pages/AdminHome";
import StartPage from "./pages/StartPage";
import UserHome from "./pages/UserHome";
import { getRole, setRole, type UserRole } from "./lib/roleStorage";

export default function App() {
  const [role, setCurrentRole] = useState<UserRole | null>(getRole());

  const handleRoleSelect = (selectedRole: UserRole) => {
    setRole(selectedRole);
    setCurrentRole(selectedRole);
  };

  return (
    <Routes>
      <Route path="/" element={<StartPage onSelectRole={handleRoleSelect} />} />

      <Route
        path="/user"
        element={role === "user" ? <UserLayout /> : <Navigate to="/" replace />}
      >
        <Route index element={<UserHome />} />
      </Route>

      <Route
        path="/admin"
        element={role === "admin" ? <AdminLayout /> : <Navigate to="/" replace />}
      >
        <Route index element={<AdminHome />} />
      </Route>

      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}
