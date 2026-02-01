# GitHub Copilot General Instructions

# Application Overview
The application provides two roles: Admin and User.  
On the initial page, the user selects a role and is redirected to the default layout for that role.  
Users can search and select ingredients from a database using autocomplete or ingredient cards.  
The backend API, powered by an LLM, generates recipes based on selected ingredients and returns structured results to the frontend.  
Recipes can be saved, searched by name, and filtered by cooking time.  
Admins can manage the ingredient database by adding new items, renaming existing ones, and editing categories.

This repository contains two main parts:
- Backend: ASP.NET Core solution with multiple projects.
- Frontend: React application.

Copilot should follow these general rules when generating code for this repository.

# General Guidelines
- Generate clean and maintainable code.
- Prefer readable variable and method names over short or cryptic ones.
- Add comments only when logic is non-trivial or requires explanation.
- Ensure consistency between backend and frontend (naming conventions, API endpoints).
- Avoid unnecessary complexity; prefer simple and direct solutions.