# SolarWatch 🌤️

Welcome to **SolarWatch**, a web application that provides users with weather-related features, including sunrise and sunset timings, today's weather forecast, and a history of their searches. This project demonstrates a full-stack web application built using modern technologies.

## 🎥 Demo Video 

https://github.com/user-attachments/assets/510a4226-a5df-4ac9-81a5-815dd6bf29c9

## 🚀 Features

1. **User Registration & Login**:
    - Secure user registration and authentication using ASP.NET Identity.
    - Logged-in users can access their search history.

2. **Sunrise & Sunset Information**:
    - Search for sunrise and sunset times for any city by name.
    - Select a specific date for detailed sunrise and sunset information.

3. **Today's Weather Forecast**:
    - View the current day's weather forecast for the searched city.

4. **Search History**:
    - Logged-in users can view their past searches for easy reference.

## 🛠️ Technologies Used

### Backend
- **C# ASP.NET Core**: Handles API requests and user authentication.
- **Entity Framework Core**: ORM for SQL database operations.

### Frontend
- **React**: For building an interactive and responsive user interface.
- **CSS**: Basic styling for a clean and functional design.

### Database
- **SQL Server**: Stores user data, search history, and other relevant data.

### Docker
- Dockerized application for streamlined deployment and local setup.

## 🛠️ Installation and Setup

### Prerequisites
- [Docker](https://www.docker.com/) installed on your machine.
- Git for cloning the repository.

### Steps
1. **Clone the Repository**:
```bash
git clone https://github.com/pixhy/solarwatch-api-csharp
```
2. **Build and Start the Application (using Docker Compose)**:
   - Add docker to your services
   - Run docker-compose up in the `docker-compose.yml` file.

3. **Configuration settings**: Ensure the following settings are configured in `appsettings.json`:

- `DefaultConnection`: Connection string for your SQL Server.
- `OpenWeatherMapAPIKey`: API key for OpenWeatherMapAPI for getting geolocations.
- `WeatherAPIKey`: API key for the weather forecast.

An empty configuration file `appsettings_empty.json` is provided in the project.

4. **Frontend setup**:
- Navigate to the frontend folder:
  ```bash
   cd \Solarwatch\Frontend
  ```
- Install dependencies:
  ```bash
  npm install
  ```
- Start the React development server:
  ```bash
  npm run dev
  ```

## 📂 Project Structure

```bash
Solarwatch/
│
├── Backend/            # ASP.NET Core project
│   ├── Controllers/    # API controllers
│   ├── Models/         # Entity Framework models
│   ├── Authentication/ # Service for Authentication
│   ├── Contracts/      # Contracts for AuthController
│   ├── Data/           # Docker Migration & list of countries
│   ├── DbContext/      # Database Context
│   ├── Services/       # Services&Interfaces used by Controllers
│   └── Dockerfile      # Dockerfile for backend

│
├── Frontend/           # React project
│   └── src/
│       ├── components/ # Reusable React components
│       └── assets/     # Pictures for website
│
├── docker-compose.yml  # Docker Compose configuration
└── README.md           # Project documentation

```

## 🗂️ Database Schema
**Tables**
- AspNetUsers
- AspNetUserRoles
- Cities
- SunriseAndSunsets
- UserHistoryEntries

## 👥 Creator
- [GitHub](https://github.com/pixhy)
- [LinkedIn](https://www.linkedin.com/in/tunde-bak)
