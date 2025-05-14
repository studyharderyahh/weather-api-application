# 🌤️ Weather API Application (C#)

Developed a C#(.NET) weather app using RESTful APIs (e.g., NASA) to display real-time data and forecasts. Employed MVC pattern and tested with NUnit for reliability. 

## 📦 Features

- Search weather by city name
- Fetch current weather conditions (temperature, humidity, etc.)
- Clean service-based architecture
- Easy-to-configure API integration
- Extendable and maintainable codebase

## 🛠️ Tech Stack

- **C# / .NET Framework or .NET Core**
- **Visual Studio 2019+**

## 🚀 Getting Started

### Prerequisites

- Visual Studio 2019 or newer
- .NET SDK (Core or Framework, depending on target)
- Internet access (to query the weather API)

### Setup Steps

1. Clone or extract the repository.
2. Open `WeatherApplication.sln` in Visual Studio.
3. Restore NuGet packages if prompted.
4. Insert your weather API key in the configuration file (e.g., `Config/ApiConfig.cs` or `appsettings.json`).
5. Build and run the application.

### API Key Configuration

If using [OpenWeatherMap](https://openweathermap.org/api) or similar:

```csharp
public static class ApiConfig
{
    public static string ApiKey = "your_api_key_here";
    public static string BaseUrl = "https://api.openweathermap.org/data/2.5/";
}
```

🧪 Testing
The solution may include a TestWeatherProject project with unit tests. Run the tests using the built-in Visual Studio Test Explorer.

📌 Future Enhancements
Multi-day weather forecast support

Unit conversion (Celsius/Fahrenheit)

Advanced error handling

UI enhancements

👨‍💻 Author
Developed by Elisa Wu as a academic learning project.

📄 License
This project is licensed under the MIT License.
