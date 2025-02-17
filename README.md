# User Appointment Management System

This is a .NET 8-based appointment management system that allows users to create, manage, and view appointments with timezone support.

## Prerequisites

Before running the project, ensure you have the following installed:

- **.NET 8 SDK** ([Download Here](https://dotnet.microsoft.com/en-us/download/dotnet/8.0))
- **Microsoft Server SQL
- **Git** (optional, for version control)

## Setup Instructions

### 1. Clone the Repository

### 2. Configure the Database

- Update the database connection string in `appsettings.json` and `appsettings.Development.json`:

```json
"DB": {
    ...
    "dbname": "YOUR_DB_NAME",
    "username": "YOUR_DB_USERNAME",
    "password": "YOUR_DB_PASSWORD",
    ...
}
```

### 3. Run the Application

```sh
dotnet run
```

### Troubleshooting

- **Port already in use?** Run:
  ```sh
  dotnet run --urls=http://localhost:5001
  ```
- **Database issues?** Ensure the database server is running and the credentials are correct in `appsettings.json` and/or `appsettings.Development.json`.
