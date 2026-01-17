# Police Bharati 2026 – Recruitment Management System

Police Bharati 2026 is a full-stack web application designed to manage candidate registration, verification, and physical standard tests for police recruitment.

The system displays all registered candidates in a table format and allows officers to view complete candidate details using the Application Number without manual entry.

Candidate data is fetched from a centralized Master database using REST APIs built with ASP.NET Core.

The application includes a webcam-based photo capture feature where photos are captured only after entering a valid Application Number and are stored directly in the database.

Physical Standard Tests (PST) such as height, weight, and chest measurements are verified independently by different officers.

A bulk Excel upload module is provided for uploading large volumes of candidate data.

The frontend is built using React (Vite) with a multi-step read-only verification form.

The backend is developed using ASP.NET Core Web API with SQL Server.

Swagger is used for API testing and validation.

---

## Installation & Setup

### Backend (ASP.NET Core)
```bash
cd policebharati2026
dotnet restore
dotnet run
```

### Frontend (React + Vite)
```
cd frontend
npm install
npm run dev
