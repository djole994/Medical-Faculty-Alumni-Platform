🎓 Medical Faculty Alumni Platform
A centralized digital platform connecting students and graduates of the Medical Faculty.

This project was developed as a modern solution for organizing the Alumni community, enabling former students to stay connected, track scientific events, and share professional opportunities.


📖 About the Project
The Alumni Platform transforms how the Medical Faculty interacts with its graduates. Replacing outdated lists, the application offers an interactive experience where users can:

Create professional profiles and become verified members of the organization.

Visualize global presence via an interactive world map.

Register online for congresses and educational events.

Access an exclusive job board and read successful "Alumni Stories".

Manage memberships through an integrated financial module.

The system focuses on automation (geolocation, verification) and scalability (data caching).

🛠 Tech Stack
Backend:

.NET 8 (ASP.NET Core Web API) - Service-oriented architecture.

Entity Framework Core - Code-first database approach.

SQL Server - Relational database management.

Dependency Injection & Repository Pattern.

Frontend:

React - SPA (Single Page Application).

Axios - HTTP client.

Leaflet / React-Leaflet - For map rendering.

Integrations:

Nominatim (OpenStreetMap) - For location geocoding.

Photon API - For type-ahead city search and suggestions.

🚀 Key Challenge: Intelligent Geocoding System
One of the significant engineering challenges was mapping users without overloading external APIs while maintaining tolerance for typos and data entry errors.

I implemented a "Hybrid Caching Strategy" with Fallback logic:

The system first checks the local GeoCache.

If the location is missing, it consults the Nominatim API.

If the API fails to recognize the city (e.g., due to a typo), the system automatically uses Country coordinates as a fallback mechanism and flags the entry for manual admin review.

Geocoding Workflow Diagram
