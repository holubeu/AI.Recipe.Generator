import { useState } from "react";
import { Navigate, Route, Routes } from "react-router-dom";
import AdminLayout from "./layouts/AdminLayout";
import UserLayout from "./layouts/UserLayout";
import AdminHome from "./pages/AdminHome";
import ApiKeysPage from "./pages/ApiKeysPage";
import GenerateRecipePage from "./pages/GenerateRecipePage";
import IngredientCategoriesPage from "./pages/IngredientCategoriesPage";
import IngredientsPage from "./pages/IngredientsPage";
import SavedRecipesPage from "./pages/SavedRecipesPage";
import StartPage from "./pages/StartPage";
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
        <Route index element={<GenerateRecipePage />} />
        <Route path="generate" element={<GenerateRecipePage />} />
        <Route path="saved" element={<SavedRecipesPage />} />
      </Route>

      <Route
        path="/admin"
        element={role === "admin" ? <AdminLayout /> : <Navigate to="/" replace />}
      >
        <Route index element={<AdminHome />} />
        <Route path="ingredients" element={<IngredientsPage />} />
        <Route path="ingredient-categories" element={<IngredientCategoriesPage />} />
        <Route path="api-keys" element={<ApiKeysPage />} />
      </Route>

      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}
