# LegalOfficeManager

**LegalOfficeManager** is a modern web application designed to manage the daily operations of a legal office. Built with **ASP.NET Core 8** and integrated with **Microsoft Azure**, it provides secure and scalable functionality for client, case, and document management. The system automates legal workflows, reporting, and notification processes using modern .NET and cloud technologies.

## Features

- Client and case management (CRUD)
- Lawyer assignment and scheduling
- Internal comment threads for case history
- Document uploads and PDF generation
- Automated email notifications (e.g., new case updates)
- Daily background reports sent via email
- Role-based authentication (Admin, Lawyer, Secretary)
- Filtering and reporting on cases and clients
- Logging and performance profiling

## Core Functionality

| **Module**        | **Requirements**                                                                 |
|-------------------|----------------------------------------------------------------------------------|
| Authentication    | Registration, login (ASP.NET Identity), roles: `Admin`, `Lawyer`, `Receptionist` |
| Clients           | Client CRUD, search, assign cases                                                |
| Cases             | Case CRUD, statuses, assign to lawyer, deadlines                                 |
| Calendar          | Calendar view, appointment reminders                                             |
| Documents         | Upload and manage documents (PDF/DOC) per case, version control                  |
| Services          | List of law office services + pricing                                            |
| Notes             | Internal notes for cases (with edit history)                                     |
| Reports           | Case statistics by client / lawyer / date + export to PDF                        |
| Settings          | Manage users, roles, law office data (`Admin` only)                              |


## Technologies & Architecture

| Layer / Function           | Technology                                                                 |
|---------------------------|----------------------------------------------------------------------------|
| Backend                   | ASP.NET Core 8 Web API + Razor Pages                                       |
| ORM & DB                  | Entity Framework Core 8 + SQL Server (local or Azure SQL)                  |
| Authentication            | ASP.NET Identity + Role-based Authorization                                |
| Mapping                   | [Mapperly](https://github.com/mapperly/mapperly)                           |
| Logging                   | NLog (file-based) + Azure Application Insights (optional)                  |
| CI/CD                     | GitHub Actions + optional Docker image build and push                      |
| Cloud Hosting             | Azure App Service / Azure Container Apps                                   |
| File Storage              | Azure Blob Storage (for documents and uploads)                             |
| Background Processing     | .NET `BackgroundService` for automated PDF generation and email delivery   |
| Email Delivery            | SMTP or Azure Communication Services                                       |
| API Documentation         | Swagger / Swashbuckle                                                      |
| Performance Testing       | NBomber load testing                                                        |

## CI/CD – GitHub Actions

Automated CI/CD is configured via GitHub Actions:

- Build: `dotnet build`
- Tests: `dotnet test`
- Optional Docker build and publish
- Deployment to Azure (with secrets)

Workflow file: `.github/workflows/dotnet-ci.yml`

<!-- ## Cloud Integration

- **Azure Blob Storage**: stores uploaded legal documents securely
- **Azure App Service**: deploys the web application in the cloud
- **Azure SQL Database**: stores application data
- **Azure Key Vault (optional)**: manages connection strings and secrets
- **Azure Monitor (optional)**: tracks performance and logs via Application Insights -->

## Roles and Permissions

| Role       | Description                                                            |
|------------|------------------------------------------------------------------------|
| Admin      | Full access to manage all users, cases, and system settings            |
| Lawyer     | Manages assigned cases, adds comments, uploads documents               |
| Secretary  | Registers clients, schedules meetings, creates new cases               |

## Testing & Profiling

- **Unit tests**: written using xUnit
- **Load testing**: NBomber simulation on `/api/cases/active`
- **Query analysis**: SQL Server Profiler and EF Core logging
- **Index optimization**: added non-clustered indexes where needed

## Setup Instructions

1. Clone the repository
2. Configure `appsettings.json` (SMTP, Azure, DB)
3. Run database migrations: `dotnet ef database update`
4. Launch with `dotnet run` or from Visual Studio
5. Optional: deploy to Azure using GitHub Actions or Azure CLI

## License

MIT License – you are free to use and modify the code for educational and commercial purposes.

---

<!-- **Authors:**  
Piotr Sus  
Michał Siudut -->

## Development Team

Two developers. One mission: streamline legal office management.

| Name   | Role               | GitHub                                           |
|--------|--------------------|--------------------------------------------------|
| Piotr Sus  | Full Stack Developer | [@pSus365](https://github.com/pSus365) |
| Michał Siudut | Full Stack Developer | [@michalsiudut](https://github.com/michalsiudut) |


2025 – For .NET & Cloud Technologies Project - Cracow University of Technology  
