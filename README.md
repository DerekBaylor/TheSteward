# 🏠 The Steward

> A collaborative home management system for budgeting, task management, and meal planning.  
> 🌐 **Live Demo:** https://thesteward-1039866872649.us-east1.run.app/

---

## Overview

The Steward is a multi-user household management application designed to help households stay organized and financially aware. It brings together budget tracking, task management, and meal planning into a single shared workspace — with secure, role-based access so every member of a household sees the right information.

---

## ✨ Features

- **Finance Manager** — Track income, investments, credit & debt, and expenses with category and subcategory grouping and live tax estimates
- **Task Manager** — Create, assign, and track recurring household tasks with priority levels and due dates
- **Household Management** — Create and manage households, invite members, and assign roles and permissions
- **Meal Planning** *(coming soon)* — Plan weekly meals collaboratively across your household

---

## 📸 Screenshots

### Landing Page
![Home Page](docs/screenshots/home_page.png)

---

### Login Page
![Login Page](docs/screenshots/login_page.png)

---

### User Dashboard
![User Dashboard](docs/screenshots/user_dashboard.png)

---

### Budget Dashboard
![Budget Dashboard](docs/screenshots/budget_dashboard.png)

---

### Household Management
![Household Management](docs/screenshots/household_dashboard.png)

---

## 🛠 Tech Stack

| Layer | Technology |
|---|---|
| Frontend | Blazor Server (.NET 10) |
| Backend | ASP.NET Core / .NET 10 |
| ORM | Entity Framework Core |
| Database | PostgreSQL |
| UI Components | MudBlazor |
| Authentication | ASP.NET Core Identity |
| Object Mapping | AutoMapper |
| Containerization | Docker |
| Hosting | Google Cloud Run |

---

## 🏗 Architecture

The Steward follows a clean **layered architecture** with clear separation of concerns:

```
┌─────────────────────────────────────┐
│         TheSteward.Web              │  Blazor pages, routing, app entry point
├─────────────────────────────────────┤
│         TheSteward.Shared           │  Reusable Blazor components, state management, form DTOs
├─────────────────────────────────────┤
│         TheSteward.Core             │  Domain models, interfaces, DTOs, AutoMapper profiles
├─────────────────────────────────────┤
│      TheSteward.Infrastructure      │  EF Core DbContext, repositories, service implementations
└─────────────────────────────────────┘
```

- **Core** has zero dependencies on Infrastructure — all cross-layer communication goes through interfaces
- **Repositories** abstract all data access so services stay testable
- **Services** own all business logic and are injected via DI throughout the app

---

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)

### Setup

**1. Clone the repository**
```bash
git clone https://github.com/your-username/the-steward.git
cd the-steward
```

**2. Configure the database connection**

Copy the example settings and fill in your PostgreSQL credentials:
```bash
cp appsettings.Example.json appsettings.Development.json
```

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=thesteward;Username=your_user;Password=your_password"
  }
}
```

**3. Apply database migrations**
```bash
dotnet ef database update
```

**4. Run the application**
```bash
dotnet run
```

The app will be available at `https://localhost:5001`.

---

## 📁 Project Structure

```
TheSteward/
├── TheSteward.Core/              # Domain layer
│   ├── Dtos/                     # Data transfer objects
│   ├── IRepositories/            # Repository interfaces
│   ├── IServices/                # Service interfaces
│   ├── Models/                   # Domain entities
│   └── Profiles/                 # AutoMapper profiles
├── TheSteward.Infrastructure/    # Data & service implementations
│   ├── Data/                     # EF Core DbContext & migrations
│   ├── Repositories/             # Repository implementations
│   └── Services/                 # Service implementations
├── TheSteward.Shared/            # Shared frontend layer
│   ├── Components/               # Reusable Blazor components
│   ├── Dtos/                     # Form/UI-specific DTOs
│   └── State/                    # Application state management
├── TheSteward.Tests/             # Test projects
└── TheSteward.Web/               # Web application entry point
    └── Pages/                    # Blazor pages & routing
```

---

## 🔐 Authentication & Permissions

The Steward uses **ASP.NET Core Identity** for authentication. Within each household, members are assigned roles that control read and write access across features:

| Role | Finance Manager | Task Manager | Household Settings |
|---|---|---|---|
| Owner | Read & Write | Read & Write | Full Control |
| Member (Write) | Read & Write | Read & Write | Read Only |
| Member (Read) | Read Only | Read Only | Read Only |

---

## 🗺 Roadmap

- [x] Household creation and member management
- [x] Finance Manager — Income, Investments, Credit, Expenses with category & subcategory support
- [x] Task Manager — Task creation, assignment, recurrence, and tracking
- [x] Deployment — Hosted on Google Cloud Run
- [ ] Meal planning module
- [ ] Mobile-responsive improvements

---

## 🤝 Contributing

This project is currently in active development. If you'd like to contribute, feel free to open an issue or submit a pull request.

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).


