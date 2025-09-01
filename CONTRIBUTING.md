# Contributing to Feedback Board

Thank you for your interest in contributing to the Feedback Board project! This document provides guidelines and information for contributors.

## 🚀 Getting Started

### Prerequisites

- **.NET 8 SDK** or later
- **Node.js 18** or later
- **Git**
- **Visual Studio Code** (recommended) or **Visual Studio**

### Setup

1. **Fork the repository**
2. **Clone your fork**:
   ```bash
   git clone https://github.com/YOUR_USERNAME/FeedbackBoard.git
   cd FeedbackBoard
   ```

3. **Install backend dependencies**:
   ```bash
   dotnet restore
   ```

4. **Install frontend dependencies**:
   ```bash
   cd frontend
   npm install
   ```

## 🏗 Development Workflow

### Running the Application

1. **Start the backend**:
   ```bash
   cd backend/FeedbackBoard.API
   dotnet run
   ```

2. **Start the frontend**:
   ```bash
   cd frontend
   npm start
   ```

### Running Tests

```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --verbosity detailed

# Run specific test project
dotnet test backend/FeedbackBoard.API.Tests/
```

### Code Style

- Follow C# coding conventions for backend
- Use ESLint and Prettier for frontend
- Write meaningful commit messages
- Include XML documentation for public APIs

## 📝 Pull Request Process

1. **Create a feature branch**:
   ```bash
   git checkout -b feature/your-feature-name
   ```

2. **Make your changes** following the coding standards

3. **Write tests** for new functionality

4. **Run tests** to ensure everything works:
   ```bash
   dotnet test
   cd frontend && npm test
   ```

5. **Commit your changes**:
   ```bash
   git add .
   git commit -m "feat: add your feature description"
   ```

6. **Push to your fork**:
   ```bash
   git push origin feature/your-feature-name
   ```

7. **Create a Pull Request** with a clear description

## 🐛 Bug Reports

When filing a bug report, please include:

- **Environment details** (OS, .NET version, Node.js version)
- **Steps to reproduce** the issue
- **Expected behavior**
- **Actual behavior**
- **Screenshots** if applicable

## 💡 Feature Requests

For feature requests, please:

- **Check existing issues** to avoid duplicates
- **Describe the problem** you're trying to solve
- **Propose a solution** if you have one
- **Consider the impact** on existing functionality

## 🔧 Development Guidelines

### Backend (.NET)

- Follow **Clean Architecture** principles
- Use **dependency injection** for all services
- Write **comprehensive unit tests**
- Include **XML documentation** for controllers and services
- Use **structured logging** with meaningful messages

### Frontend (React)

- Use **TypeScript** for all new components
- Follow **React best practices**
- Write **meaningful component tests**
- Use **React Query** for data fetching
- Ensure **responsive design**

### Database

- Use **Entity Framework Core** migrations
- Follow **database naming conventions**
- Add **appropriate indexes** for performance

## 📊 Testing Strategy

### Unit Tests
- Test business logic in isolation
- Mock external dependencies
- Aim for high code coverage

### Integration Tests
- Test API endpoints end-to-end
- Use in-memory database for testing
- Verify HTTP status codes and response formats

### Frontend Tests
- Test component rendering
- Test user interactions
- Mock API calls

## 🚀 Deployment

The application uses:
- **.NET 8** for backend API
- **React 18** with TypeScript for frontend
- **Entity Framework Core** with in-memory database (development)
- **AutoMapper** for object mapping
- **Swagger/OpenAPI** for API documentation

## 🔗 Useful Links

- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [React Documentation](https://reactjs.org/docs/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [xUnit Testing](https://xunit.net/)
- [React Query](https://tanstack.com/query/)

## 📜 License

By contributing to this project, you agree that your contributions will be licensed under the same license as the project.

## 🤝 Code of Conduct

Please be respectful and professional in all interactions. We're here to build something great together!

---

Thank you for contributing to Feedback Board! 🎉
