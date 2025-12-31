# 🎓 Medical Faculty Alumni Platform

A centralized digital platform connecting students and graduates of the Medical Faculty.

This project was developed as a modern solution for organizing the Alumni community—helping former students stay connected, follow scientific events, and share professional opportunities.

---

## 📖 About the Project

The Alumni Platform transforms how the Medical Faculty interacts with its graduates.  
Replacing outdated lists and manual workflows, the application provides a modern, interactive experience where users can:

- ✅ Create professional profiles and become **verified members** of the organization  
- 🌍 Visualize global Alumni presence via an **interactive world map**  
- 🗓️ Register online for **congresses and educational events**  
- 💼 Access an exclusive **job board** and read inspiring **Alumni Stories**  
- 💳 Manage memberships through an integrated **financial module**  
- ⚙️ Benefit from automation (**geolocation, verification**) and scalability (**data caching**)  

---

## 🛠 Tech Stack

### Backend
- **.NET 8 (ASP.NET Core Web API)** — service-oriented architecture  
- **Entity Framework Core** — Code-First approach  
- **SQL Server** — relational database  
- **Dependency Injection** + **Repository Pattern**

### Frontend
- **React** — SPA (Single Page Application)  
- **Axios** — HTTP client  
- **Leaflet / React-Leaflet** — map rendering

### Integrations
- **Nominatim (OpenStreetMap)** — location geocoding  
- **Photon API** — type-ahead city search & suggestions  

---

## 🚀 Key Challenge: Intelligent Geocoding System

One of the biggest engineering challenges was mapping users worldwide **without overloading external APIs**, while still being tolerant to typos and imperfect data entry.

To solve this, I implemented a **Hybrid Caching Strategy** with fallback logic:

1. **Check the local `GeoCache`** first  
2. If missing, request coordinates from **Nominatim API**  
3. If the API can’t resolve the city (e.g., typo), the system:
   - falls back to **Country coordinates**
   - flags the profile for **manual admin review**

### Geocoding Workflow Diagram
> *(Add diagram image here)*  
> Example:
> `docs/images/geocoding-workflow.png`

---

## 📌 Notes
- External APIs are protected from abuse via caching and fallback rules.
- Failed/uncertain locations are never silently accepted—they’re tracked for admin verification.

