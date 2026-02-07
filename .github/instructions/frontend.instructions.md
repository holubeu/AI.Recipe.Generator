---
applyTo: 'frontend/**/*'
---

## Frontend Instructions

- This is a small React SPA.
- Prioritize simplicity and working code over best practices.
- Avoid overengineering and unnecessary abstractions.
- Use functional components with basic React hooks (useState, useEffect).
- No global state management unless explicitly requested.
- One component per file.
- Fetch data directly from the API using fetch or a simple API client.
- Minimal but polished styling:
  - clean layout
  - consistent spacing
  - neutral colors
  - light shadows
  - rounded corners
  - subtle hover and focus effects
  - small transitions (no heavy animations)
  - use plain CSS or CSS modules
- Focus on clarity and readability.
- Do not optimize for scalability or performance unless requested.

## Backend Communication
- Frontend consumes ASP.NET Core API located in 'backend' folder.
- API base URL: https://localhost:7292
- OpenAPI available at: https://localhost:7292/openapi/v1.json
