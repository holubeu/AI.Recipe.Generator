export type UserRole = "user" | "admin";

const ROLE_KEY = "selectedRole";

export function setRole(role: UserRole): void {
  localStorage.setItem(ROLE_KEY, role);
}

export function getRole(): UserRole | null {
  const value = localStorage.getItem(ROLE_KEY);
  return value === "user" || value === "admin" ? value : null;
}

export function clearRole(): void {
  localStorage.removeItem(ROLE_KEY);
}
