# 🌟 EduWay API

## 📋 Table of Contents

- [🌟 EduWay API](#-eduway-api)
  - [📋 Table of Contents](#-table-of-contents)
  - [🚀 Getting Started](#-getting-started)
    - [🔧 Prerequisites](#-prerequisites)
    - [📦 Installation](#-installation)
    - [🖥️ Running the Server](#️-running-the-server)
    - [🧪 Running Tests](#-running-tests)
    - [☁️ Deployment](#️-deployment)
  - [📚 API Documentation](#-api-documentation)
    - [👥 Administration Endpoints](#-administration-endpoints)
    - [🎓 Students Endpoints](#-students-endpoints)
    - [👩‍🏫 Teachers Endpoints](#-teachers-endpoints)
    - [👨‍🏫 Teacher Assistants Endpoints](#-teacher-assistants-endpoints)
    - [📊 Control Members Endpoints](#-control-members-endpoints)
    - [👥 Staff Endpoints](#-staff-endpoints)
  - [🗂️ Project Structure](#-project-structure)
  - [🤝 Contributing](#-contributing)

## 🚀 Getting Started

### 🔧 Prerequisites

Ensure you have the following installed:

- .NET 6
- MSSQL Server

### 📦 Installation

1. Clone the repository:
   ```bash
   git clone (https://github.com/Abdallahbestawy/GraduationProject)
   cd eduway-api
   ```

2. Set up the database connection in `appsettings.json`.

3. Restore .NET dependencies:
   ```bash
   dotnet restore
   ```

### 🖥️ Running the Server

1. Apply the database migrations:
   ```bash
   dotnet ef database update
   ```

2. Run the server:
   ```bash
   dotnet run
   ```

3. The server should now be running at `http://localhost:5000`.

### 🧪 Running Tests

To run the test suite:

```bash
dotnet test
```

### ☁️ Deployment

Deploy the application to your preferred hosting platform. Make sure to update the connection strings and environment variables as needed.

## 📚 API Documentation

### 👥 Administration Endpoints

- **Endpoint:** `/admin`
- **Method:** Various (GET, POST, PUT, DELETE)
- **Description:** Manage faculties, students, courses, and staff.

### 🎓 Students Endpoints

- **Endpoint:** `/students`
- **Method:** Various (GET, POST, PUT, DELETE)
- **Description:** Manage student data, view timetables, tuition fees, exam schedules, grades, portfolios, and progress.

### 👩‍🏫 Teachers Endpoints

- **Endpoint:** `/teachers`
- **Method:** Various (GET, POST, PUT, DELETE)
- **Description:** Manage courses, submit grades, and add daily notes about faculty and students.

### 👨‍🏫 Teacher Assistants Endpoints

- **Endpoint:** `/teacher-assistants`
- **Method:** Various (GET, POST, PUT, DELETE)
- **Description:** Manage courses, submit grades, and add daily notes about faculty and students.

### 📊 Control Members Endpoints

- **Endpoint:** `/control-members`
- **Method:** Various (GET, POST, PUT, DELETE)
- **Description:** Manage courses, submit final grades, and extract reports.

### 👥 Staff Endpoints

- **Endpoint:** `/staff`
- **Method:** Various (GET, POST, PUT, DELETE)
- **Description:** Manage student admissions and other administrative tasks.

## 🗂️ Project Structure

```
project-root/
├── graduationProject.Domain/
│   ├── Entities/
│   ├── Interfaces/
│   ├── Models/
├── graduationProject.Data/
│   ├── Contexts/
│   ├── Migrations/
│   ├── Seed/
├── graduationProject.Infrastructure/
│   ├── EntityFramework/
│   │   ├── Configurations/
│   │   ├── Repositories/
│   ├── graduationProject.Repository/
├── graduationProject.Service/
│   ├── Services/
│   ├── Interfaces/
│   ├── DTOs/
├── graduationProject.Web/
│   ├── Controllers/
│   ├── Middlewares/
│   ├── Filters/
├── graduationProject.Api/
│   ├── Controllers/
├── graduationProject.Settings/
│   ├── Identity/
│   ├── ResponseHandler/
│   ├── LogHandler/
│   ├── ExcelFileGenerator/
│   ├── Mails/
│   ├── Shared/

🌟 Explanation of the Structure
📜 graduationProject.Domain: Contains graduationProject.Data
📜 graduationProject.Data: Manages database contexts, migrations, and seeding data.
📜 graduationProject.Infrastructure: Includes the repository pattern implementation, configurations for EntityFramework, and other data access logic.
📜 graduationProject.Service: Contains business logic and service layer implementations, including DTOs and service interfaces.
📜 graduationProject.Web: Contain graduationProject.Api , Manages web-specific components like controllers, middleware, and filters.
📜 graduationProject.Api: Focused on API endpoints, including controllers, and the main application setup (Startup.cs and Program.cs).
📜 graduationProject.Settings: Includes various configurations and additional services like identity management, response handling, logging, Excel file generation, email handling, and shared utilities.
```

## 🤝 Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

1. Fork the repository
2. Create a new branch (`git checkout -b feature-branch`)
3. Make your changes
4. Commit your changes (`git commit -m 'Add some feature'`)
5. Push to the branch (`git push origin feature-branch`)
6. Open a pull request

