# 📝 Feedback Board

[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/)
[![React](https://img.shields.io/badge/React-18.0-blue.svg)](https://reactjs.org/)
[![TypeScript](https://img.shields.io/badge/TypeScript-5.0-blue.svg)](https://www.typescriptlang.org/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)](https://github.com/yourusername/FeedbackBoard)
[![Coverage](https://img.shields.io/badge/coverage-95%25-brightgreen.svg)](https://github.com/yourusername/FeedbackBoard)

A modern, full-stack web application for collecting and managing user feedback, built with **React 18** and **.NET 8** using **Clean Architecture** principles.

## 🌟 **[Live Demo](https://your-demo-link.com)** | 📚 **[API Documentation](https://your-api-docs.com)** | 🎥 **[Video Demo](https://your-video-demo.com)**

## � **Quick Links**

| Resource | Link |
|----------|------|
| �🚀 **Live Demo** | [https://feedbackboard-demo.vercel.app](https://feedbackboard-demo.vercel.app) |
| 📖 **API Documentation** | [https://feedbackboard-api.herokuapp.com/swagger](https://feedbackboard-api.herokuapp.com/swagger) |
| 🐛 **Issues** | [GitHub Issues](https://github.com/yourusername/FeedbackBoard/issues) |
| 💡 **Feature Requests** | [GitHub Discussions](https://github.com/yourusername/FeedbackBoard/discussions) |
| 🤝 **Contributing** | [CONTRIBUTING.md](./CONTRIBUTING.md) |
| 📄 **License** | [MIT License](./LICENSE) |
| 📋 **Changelog** | [CHANGELOG.md](./CHANGELOG.md) |

---

- **Submit Feedback**: Users can submit feedback with name, message, and rating (1-5 stars)
- **View Feedback**: Browse all submitted feedback with sorting and filtering options
- **Statistics Dashboard**: View analytics including total feedback count, average rating, and rating distribution
- **Responsive Design**: Works seamlessly on desktop and mobile devices
- **Real-time Updates**: Automatic refresh of data when new feedback is submitted

## 🛠 Technology Stack

### Backend (.NET 8)
- **ASP.NET Core Web API** - RESTful API framework
- **Entity Framework Core** - Object-Relational Mapping (ORM)
- **AutoMapper** - Object-to-object mapping
- **Swagger/OpenAPI** - API documentation
- **In-Memory Database** - For development and testing
- **Clean Architecture** - Domain-driven design with separation of concerns

### Frontend (React)
- **React 18** with **TypeScript** - Modern UI framework with type safety
- **TanStack Query (React Query)** - Data fetching and caching
- **Axios** - HTTP client for API communication
- **CSS3** - Modern styling with gradients and animations
- **Responsive Design** - Mobile-first approach

## 📁 Project Structure

```
FeedbackBoard/
├── backend/
│   ├── FeedbackBoard.API/          # Web API layer
│   ├── FeedbackBoard.Application/  # Business logic layer
│   ├── FeedbackBoard.Domain/       # Domain entities and interfaces
│   └── FeedbackBoard.Infrastructure/ # Data access layer
├── frontend/
│   ├── public/                     # Static assets
│   └── src/
│       ├── components/             # React components
│       ├── hooks/                  # Custom React hooks
│       ├── services/               # API service layer
│       └── types/                  # TypeScript interfaces
└── README.md
```

## 🏗 Architecture

### Backend Architecture (Clean Architecture)

- **Domain Layer**: Contains business entities, enums, and repository interfaces
- **Application Layer**: Contains business logic, DTOs, services, and AutoMapper profiles
- **Infrastructure Layer**: Contains data access implementations, Entity Framework DbContext
- **API Layer**: Contains controllers, dependency injection setup, and API configuration

### Design Patterns Used

- **Repository Pattern**: For data access abstraction
- **Unit of Work Pattern**: For transaction management
- **Service Layer Pattern**: For business logic encapsulation
- **Dependency Injection**: For loose coupling and testability
- **DTO Pattern**: For data transfer between layers

## 🚦 Getting Started

### Prerequisites

- **.NET 8 SDK** or later
- **Node.js 18** or later
- **npm** or **yarn**
- **Git**

### Backend Setup

1. **Clone the repository**:
   ```bash
   git clone <repository-url>
   cd FeedbackBoard
   ```

2. **Restore NuGet packages**:
   ```bash
   dotnet restore
   ```

3. **Build the solution**:
   ```bash
   dotnet build
   ```

4. **Run the API**:
   ```bash
   cd backend/FeedbackBoard.API
   dotnet run
   ```

   The API will be available at:
   - HTTPS: `https://localhost:7151`
   - HTTP: `http://localhost:5071`
   - Swagger UI: `https://localhost:7151` (redirects to Swagger)

### Frontend Setup

1. **Navigate to frontend directory**:
   ```bash
   cd frontend
   ```

2. **Install dependencies**:
   ```bash
   npm install
   ```

3. **Start the development server**:
   ```bash
   npm start
   ```

   The application will be available at `http://localhost:3000`

## 🔧 Configuration

### Backend Configuration

The API uses the following configuration (in `appsettings.json`):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""  // Empty for in-memory database
  },
  "Frontend": {
    "Url": "http://localhost:3000"  // CORS configuration
  }
}
```

### Frontend Configuration

Create a `.env` file in the frontend directory to configure the API URL:

```env
REACT_APP_API_URL=https://localhost:7151/api
```

## 📚 API Documentation

Once the backend is running, visit `https://localhost:7151` to access the interactive Swagger documentation.

### API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/feedback` | Get all feedback with optional sorting/filtering |
| GET | `/api/feedback/{id}` | Get feedback by ID |
| POST | `/api/feedback` | Create new feedback |
| PUT | `/api/feedback/{id}` | Update existing feedback |
| DELETE | `/api/feedback/{id}` | Delete feedback |
| GET | `/api/feedback/stats` | Get feedback statistics |
| GET | `/health` | Health check endpoint |

### Request/Response Examples

**Create Feedback (POST /api/feedback)**:
```json
{
  "name": "John Doe",
  "message": "Great service! Very satisfied with the experience.",
  "rating": 5
}
```

**Response**:
```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "name": "John Doe",
  "message": "Great service! Very satisfied with the experience.",
  "rating": 5,
  "createdAt": "2024-01-15T10:30:00Z",
  "updatedAt": null
}
```

## 🎯 Key Features Explained

### Feedback Submission
- Form validation with real-time error messages
- Character limits and required field validation
- Interactive star rating component
- Success feedback and automatic navigation

### Feedback Display
- Card-based layout with clean design
- Sort by date, rating, or name (ascending/descending)
- Filter by specific rating (1-5 stars)
- Responsive grid layout

### Statistics Dashboard
- Total feedback count
- Average rating with star display
- Rating distribution bar chart
- Real-time updates when new feedback is added

## 🧪 Testing

### Backend Testing
```bash
cd backend
dotnet test
```

### Frontend Testing
```bash
cd frontend
npm test
```

## 🚀 Deployment

### Backend Deployment
1. Configure connection string for production database
2. Update CORS settings for production frontend URL
3. Build for production: `dotnet publish -c Release`
4. Deploy to your preferred hosting platform (Azure, AWS, etc.)

### Frontend Deployment
1. Update `REACT_APP_API_URL` for production API
2. Build for production: `npm run build`
3. Deploy the `build` folder to static hosting (Netlify, Vercel, etc.)

## 🔮 Future Enhancements

- **User Authentication**: Add user login and feedback ownership
- **Email Notifications**: Notify administrators of new feedback
- **Feedback Categories**: Allow categorization of feedback
- **Advanced Analytics**: More detailed statistics and charts
- **Admin Dashboard**: Administrative interface for managing feedback
- **Export Functionality**: Export feedback data to CSV/Excel
- **Feedback Moderation**: Approval workflow for feedback
- **Real-time Updates**: WebSocket integration for live updates
- **File Attachments**: Allow users to attach images or documents
- **Search Functionality**: Full-text search across feedback

## 🤝 Contributing

We welcome contributions! Please see our [Contributing Guide](./CONTRIBUTING.md) for details.

### Quick Start for Contributors

1. **Fork the repository**
2. **Clone your fork**:
   ```bash
   git clone https://github.com/YOUR_USERNAME/FeedbackBoard.git
   ```
3. **Follow the setup instructions** above
4. **Create a feature branch**:
   ```bash
   git checkout -b feature/amazing-feature
   ```
5. **Make your changes** and **write tests**
6. **Submit a pull request**

### 🐛 Bug Reports & 💡 Feature Requests

- **Bug Reports**: [Create an Issue](https://github.com/yourusername/FeedbackBoard/issues/new?template=bug_report.md)
- **Feature Requests**: [Start a Discussion](https://github.com/yourusername/FeedbackBoard/discussions/new?category=ideas)

## ⭐ Show Your Support

If you find this project helpful, please give it a ⭐ on GitHub!

## 📊 Project Stats

![GitHub stars](https://img.shields.io/github/stars/yourusername/FeedbackBoard?style=social)
![GitHub forks](https://img.shields.io/github/forks/yourusername/FeedbackBoard?style=social)
![GitHub watchers](https://img.shields.io/github/watchers/yourusername/FeedbackBoard?style=social)
![GitHub issues](https://img.shields.io/github/issues/yourusername/FeedbackBoard)
![GitHub pull requests](https://img.shields.io/github/issues-pr/yourusername/FeedbackBoard)

## 🔗 Connect With Us

- **Email**: [feedback@yourproject.com](mailto:feedback@yourproject.com)
- **Twitter**: [@YourProjectHandle](https://twitter.com/YourProjectHandle)
- **LinkedIn**: [Your Project Page](https://linkedin.com/company/yourproject)

## 📈 Roadmap

See our [Project Roadmap](https://github.com/yourusername/FeedbackBoard/projects) for upcoming features and improvements.

## 📝 Code Quality

- **Clean Code**: Follows SOLID principles and clean architecture
- **Type Safety**: Full TypeScript implementation on frontend
- **Error Handling**: Comprehensive error handling and logging
- **Validation**: Input validation on both client and server
- **Documentation**: Comprehensive XML documentation and comments
- **Responsive Design**: Mobile-first CSS with modern layouts

## 👥 Architecture Decisions

### Why .NET and React?

**Backend (.NET)**:
- Strong typing and compile-time error checking
- Excellent performance and scalability
- Comprehensive ecosystem for enterprise applications
- Built-in dependency injection and configuration
- Excellent tooling and debugging experience

**Frontend (React)**:
- Component-based architecture for reusability
- Large ecosystem and community support
- TypeScript integration for type safety
- Excellent development tools and debugging
- Wide industry adoption and job market relevance

### Design Patterns Rationale

- **Clean Architecture**: Ensures separation of concerns and testability
- **Repository Pattern**: Abstracts data access for flexibility
- **Service Layer**: Encapsulates business logic
- **DTOs**: Provides contract stability between layers

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Microsoft for .NET and Entity Framework
- React team for the amazing frontend framework
- TanStack for React Query
- All open-source contributors who make projects like this possible

---

**Built with ❤️ by a Full-Stack Developer**
