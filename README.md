# OtChaim

OtChaim is a modern emergency event management system designed to facilitate rapid response, user subscription management, and real-time notifications. Built with .NET and MAUI for cross-platform support, it leverages a clean architecture for maintainability and scalability.

## 🚀 Features
- Emergency event creation, tracking, and resolution
- User subscription and approval workflow
- Real-time notifications and status updates
- Modular architecture with clear separation of concerns
- Cross-platform UI with MAUI

## 🏗️ Project Structure
- `OtChaim.Domain/` - Core domain models and business logic
- `OtChaim.Application/` - Application layer, CQRS handlers, and commands
- `OtChaim.Persistence/` - Data access and repository implementations
- `OtChaim.Presentation.MAUI/` - Cross-platform UI (MAUI)
- `*.Tests/` - Unit and integration tests for each layer

## 🛠️ Getting Started
### Prerequisites
- [.NET 7+ SDK](https://dotnet.microsoft.com/download)
- (Optional) [MAUI workload](https://learn.microsoft.com/en-us/dotnet/maui/get-started/installation)

### Build & Run
```sh
# Restore dependencies
 dotnet restore
# Build all projects
 dotnet build
# Run MAUI app (example for Windows)
 dotnet run --project OtChaim.Presentation.MAUI
```

### Running Tests
```sh
dotnet test
```

## 🤝 Contributing
1. Fork the repository
2. Create a feature branch (`git checkout -b feature/your-feature`)
3. Commit your changes
4. Open a Pull Request

Please follow the code style and add tests for new features.

## 📄 License
This project is licensed under the GNU GPLv3. See [LICENSE](LICENSE) for details. 