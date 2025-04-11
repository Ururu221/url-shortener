# 🔗 UrlShortener — Minimal URL Shortening API using ASP.NET Core + Docker + PostgreSQL

**UrlShortener** is a lightweight and scalable REST API for creating short links from long URLs. It's built with **ASP.NET Core (.NET 8)** following clean architecture principles like **SOLID**, **Dependency Injection**, and uses **Entity Framework Core** with a **PostgreSQL** database. The project is fully containerized using **Docker Compose**, making it easy to run anywhere.

---

## 🚀 Features

- ✅ Shortens any valid long URL to a short code
- 🔁 Redirects users using a short link back to the original URL
- 💾 Stores all data in PostgreSQL using EF Core
- 🔧 Built with `Minimal API` pattern (lightweight)
- 🧱 Architecture follows SOLID principles and clean separation of concerns
- 📦 Full Docker support for easy deployment and testing

---

## 🛠️ Technologies Used

| Technology            | Purpose                              |
|-----------------------|---------------------------------------|
| ASP.NET Core (.NET 8) | Main backend framework (Minimal API) |
| Entity Framework Core | Data access and migrations            |
| PostgreSQL            | Persistent data storage               |
| Docker & Docker Compose | Containerization                     |
| C#                    | Business logic, DTOs, DI, and models |

---

## 📁 Project Structure

```
Solution/
│
├── Program.cs               # Entry point + route config
├── Entities/                # Database entities
├── Models/                  # Input models (DTOs)
├── Services/                # Business logic
├── Extensions/              # App extensions (e.g., migrations)
├── appsettings.json         # App configuration
├── Dockerfile               # Docker instructions for API
├── docker-compose.yml       # Combined API + Postgres containers
```

---

## ⚙️ Run with Docker

### ✅ Prerequisites:
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- Git (optional)

### 📥 Clone the project:

```bash
git clone https://github.com/your-username/urlshortener.git
cd urlshortener
```

### 🏗️ Build and run:

```bash
docker-compose build --no-cache
docker-compose up
```

After a few seconds:

- API will be running at:  
  `http://localhost:5000`

- PostgreSQL credentials:  
  host: `localhost`, port: `1234`,  
  user: `postgres`, password: `2413050505`,  
  database: `url_shortener`

---

## 🧪 How to Test the API (using curl)

### 1. Basic check:

```bash
curl http://localhost:5000/
# Response: "Hello world!"
```

---

### 2. Create a short URL:

```bash
curl -X POST -H "Content-Type: application/json" \
     -d "{\"Url\": \"https://example.com\"}" \
     http://localhost:5000/url/shorten
```

- This will return a short URL like:  
  `http://localhost:5000/url/abc123`

---

### 3. Use the short URL:

```bash
curl -L http://localhost:5000/url/abc123
```

- This will **redirect** you to the original long URL.

---

## 🔄 Endpoints Summary

| Method | Endpoint              | Description                             |
|--------|-----------------------|-----------------------------------------|
| GET    | `/`                   | Returns a "Hello world!" test response  |
| POST   | `/url/shorten`        | Accepts `{ "Url": "..." }` and returns a short link |
| GET    | `/url/{code}`         | Redirects to the original long URL      |

---

## 💡 Why This Project is a Good

- ✅ Realistic microservice with persistent storage
- ✅ Uses **SOLID principles** and **clean architecture**
- ✅ Well-organized using **DI**, DTOs, and Minimal APIs
- ✅ Fully containerized with Docker and PostgreSQL
- ✅ Easy to test and extend (Swagger, logging, CI/CD ready)
- ✅ Production-friendly project layout

---

## 👤 Author

**Marenych Fedia**  

Junior .NET Backend Developer

---
```
