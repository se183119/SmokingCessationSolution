# Smoking Cessation Platform - WPF Application

## Overview
This is a comprehensive WPF application for a smoking cessation platform built using .NET 8, Entity Framework Core, and Material Design. The application follows MVVM pattern with proper separation of concerns and dependency injection.

## Project Structure

### SmokingCessation.Common
- **Enums**: Application-wide enumerations (UserRole, QuitPlanStatus, MessageStatus, etc.)
- **Constants**: Application constants and configuration values
- **Extensions**: Utility extension methods for common operations

### SmokingCessation.Data
- **Entities**: Complete data models representing the database schema
- **Context**: Entity Framework DbContext with proper configuration
- **Repositories**: Repository pattern implementation with Unit of Work
- **Migrations**: Database migration support

### SmokingCessation.Business
- **DTOs**: Data Transfer Objects for API and service layers
- **Interfaces**: Service interfaces defining business operations
- **Services**: Complete business logic implementation including:
  - Authentication Service with JWT token support
  - User Management Service
  - Quit Plan Management Service
  - Smoke Log Tracking Service
  - Achievement System Service
  - Messaging Service
  - Coach Management Service
  - Feedback Management Service

### SmokingCessation.WPF
- **Views**: WPF windows and user controls
- **ViewModels**: MVVM ViewModels with CommunityToolkit.Mvvm
- **Services**: Application services (Navigation, Dialog, CurrentUser)
- **Helpers**: Converters and utility classes
- **Styles**: Material Design styling and custom themes

## Key Features Implemented

### 1. Authentication & User Management
- User registration and login with JWT tokens
- Password hashing and validation
- Role-based access control (Member, Coach, Admin)
- Profile management

### 2. Database Design
- Complete entity relationships
- Proper foreign key constraints
- Enum conversion handling
- Seed data for initial setup

### 3. Business Logic
- Smoke tracking and statistics calculation
- Quit plan creation and milestone tracking
- Achievement system with automatic awarding
- Money saved and health benefit calculations
- Message system for coach-member communication

### 4. WPF Application Architecture
- Dependency injection with Microsoft.Extensions.DependencyInjection
- MVVM pattern with proper data binding
- Material Design UI components
- Custom converters and styles
- Navigation service for window management

### 5. Data Access Layer
- Repository pattern with generic base repository
- Unit of Work pattern for transaction management
- Entity Framework Core with proper configuration
- Async/await throughout the data layer

## Technologies Used

- **.NET 8**: Latest .NET framework
- **WPF**: Windows Presentation Foundation for UI
- **Entity Framework Core**: ORM for database access
- **AutoMapper**: Object-to-object mapping
- **Material Design Themes**: Modern UI styling
- **CommunityToolkit.Mvvm**: MVVM helpers and commands
- **JWT**: JSON Web Tokens for authentication
- **FluentValidation**: Data validation (configured but not fully implemented)

## Database Schema

The application supports a comprehensive database schema including:
- Users with role management
- Quit plans with milestones
- Smoke logs for tracking
- Achievement system
- Coach-member relationships
- Messaging system
- Feedback management
- Template system

## Future Enhancements

1. **Complete UI Implementation**: All remaining views and ViewModels
2. **Real-time Notifications**: SignalR integration
3. **Data Validation**: Complete FluentValidation rules
4. **Testing**: Unit tests and integration tests
5. **Reporting**: Charts and analytics using LiveCharts
6. **Mobile App**: Xamarin or MAUI companion app
7. **API Layer**: REST API for mobile and web clients

## Getting Started

1. **Prerequisites**:
   - Windows OS
   - .NET 8 SDK
   - SQL Server (LocalDB or full instance)

2. **Database Setup**:
   - Update connection string in App.xaml.cs
   - Run the application - EF will create the database automatically
   - Default admin user: admin@smokingcessation.com / admin123

3. **Running the Application**:
   - Build the solution
   - Run SmokingCessation.WPF project
   - Login with default credentials or register a new user

## Architecture Benefits

- **Scalable**: Clean separation of concerns allows for easy maintenance
- **Testable**: Dependency injection enables unit testing
- **Extensible**: Repository pattern allows for different data sources
- **Modern**: Uses latest .NET features and best practices
- **Maintainable**: SOLID principles and clean architecture

This implementation provides a solid foundation for a comprehensive smoking cessation platform with room for future enhancements and scaling.