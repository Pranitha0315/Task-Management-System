# Task Management System

Basic .NET 8 Web API for managing users and tasks.

## Project Structure

```
Task Management System/
├── Controllers/     API endpoints
├── Services/        Business logic
├── Repositories/    Database access (ADO.NET)
├── Models/          Database entities
├── DTOs/            Request and response models
├── Program.cs       App startup
└── appsettings.json Connection string
```

## API Endpoints

### Users (`/api/User`)
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/User` | Get all users |
| GET | `/api/User/{id}` | Get user by id |
| GET | `/api/User/{id}/tasks` | Get user with tasks |
| POST | `/api/User` | Create user |
| PUT | `/api/User/{id}` | Update user |
| DELETE | `/api/User/{id}` | Delete user |

### Tasks (`/api/Task`)
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/Task` | Get all tasks |
| GET | `/api/Task/{id}` | Get task by id |
| GET | `/api/Task/search?title=` | Search tasks |
| POST | `/api/Task` | Create task |
| PUT | `/api/Task/{id}` | Update task |
| PUT | `/api/Task/{id}/status?status=` | Change task status |
| DELETE | `/api/Task/{id}` | Delete task |

## Run

1. Update connection string in `appsettings.json`
2. Run the project from Visual Studio or `dotnet run`
3. Open Swagger at `https://localhost:7008/swagger`
