# MoodyApi 🎭

[![NuGet](https://img.shields.io/nuget/v/MoodyApi.svg)](https://www.nuget.org/packages/MoodyApi)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-6.0%2B-blue.svg)](https://dotnet.microsoft.com/download)

> **Tired of boring, robotic API responses?**  
**MoodyApi** adds personality to your .NET applications by wrapping your responses with mood-based messages — tailored to user behavior, system context, and even time of day.

---

## Why MoodyApi? 🤔

In a world full of lifeless JSON, *MoodyApi* brings your APIs to life with responses that can be:

- 🎯 **Personalized** — Based on user activity and karma tracking  
- ⏰ **Context-aware** — Adjusts mood based on time of day or system state  
- 😄 **Engaging** — Injects humor, motivation, or sarcasm depending on your settings  
- 🛡️ **Graceful under pressure** — Handles errors with style  
- 🔧 **Lightweight and pluggable** — Easy to drop into any ASP.NET Core pipeline  

---

## About This Project 🧪

This is my **first NuGet package** — created as a simple, developer-focused library that adds fun and context to your API responses.

It’s lightweight, easy to integrate, and open to extension.  
Think of it as *middleware with a personality*.


_Ready to give your APIs some attitude? Let's go!_


---

## Table of Contents
- [Features](#features)
- [Installation](#installation)
- [Quick Start](#quick-start)
- [Usage Examples](#usage-examples)
- [ASP.NET Core Middleware](#aspnet-core-middleware)
- [Configuration](#configuration)
- [Mood Types](#mood-types)
- [Advanced Usage](#advanced-usage)
- [Best Practices](#best-practices)
- [Contributing](#contributing)
- [License](#license)

---

## Features ✨

- **🎭 6 Built-in Mood Types**: Neutral, Motivational, Sarcastic, Karma-Based, Time-Based, Error-Based
- **🏗️ Extensible Architecture**: Easily add custom mood providers
- **👤 User Karma Tracking**: Personalize responses based on user activity
- **⚡ High Performance**: Optimized for production use with automatic cleanup
- **🔧 Flexible Configuration**: Configure via DI, appsettings.json, or runtime
- **🌐 ASP.NET Core Integration**: Seamless middleware for automatic response wrapping
- **📝 Comprehensive Logging**: Full observability with Microsoft.Extensions.Logging
- **🛡️ Robust Error Handling**: Graceful fallbacks for all scenarios
- **🧵 Thread-Safe**: Built for concurrent applications
- **📦 Zero Dependencies**: Only uses standard .NET libraries

---

## Installation 📦

Install from NuGet Package Manager:

```shell
Install-Package MoodyApi
```

Or using the .NET CLI:

```shell
dotnet add package MoodyApi
```

**Requirements**: .NET 6.0 or higher

---

## Quick Start 🚀

### 1. Basic Setup

```csharp
using MoodyApi.Extensions;
using MoodyApi.Models;

// Register services
var services = new ServiceCollection();
services.AddLogging();
services.AddMoodyApi();

var serviceProvider = services.BuildServiceProvider();

// Initialize the static API
MoodyApi.Mood.Initialize(serviceProvider);

// Generate your first mood message
string message = MoodyApi.Mood.Get(MoodType.Motivational);
Console.WriteLine(message);
// Output: "You are capable of amazing things."
```

### 2. ASP.NET Core Setup

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add MoodyApi services
builder.Services.AddMoodyApi(options =>
{
    options.Mode = MoodType.Sarcastic;
    options.KarmaThreshold = 5;
});

var app = builder.Build();

// Initialize and use middleware
MoodyApi.Mood.Initialize(app.Services);
app.UseMoodyApi();

app.MapGet("/hello", () => new { message = "Hello World!" });
app.Run();
```

---

## Usage Examples 💡

### Basic Message Generation

```csharp
using MoodyApi.Models;

// Get a simple message
string motivational = MoodyApi.Mood.Get(MoodType.Motivational);
string sarcastic = MoodyApi.Mood.Get(MoodType.Sarcastic);
string timeBased = MoodyApi.Mood.Get(MoodType.TimeBased);

Console.WriteLine(motivational);
// Output: "Dream big. Work hard. Stay focused."
```

### Detailed Mood Responses

```csharp
// Get a complete mood response with metadata
var response = MoodyApi.Mood.GetResponse(MoodType.KarmaBased, userId: "user-123");

Console.WriteLine($"Message: {response.Message}");
Console.WriteLine($"Mood: {response.Mood}");
Console.WriteLine($"Timestamp: {response.Timestamp}");
Console.WriteLine($"Karma Score: {response.KarmaScore}");

// Output:
// Message: "What you seek is also seeking you."
// Mood: KarmaBased
// Timestamp: 2024-01-15 10:30:45
// Karma Score: 3
```

### Time-Based Messages

```csharp
// Messages automatically adapt to time of day
string morning = MoodyApi.Mood.Get(MoodType.TimeBased);
// Morning (6-12): "Good morning! Ready to start fresh?"
// Afternoon (12-17): "Afternoon productivity in full swing!"
// Evening (17-21): "Evening requests have a special charm."
// Night (21-6): "Burning the midnight oil?"
```

### Error Handling

```csharp
// MoodyApi gracefully handles errors
try
{
    var response = MoodyApi.Mood.GetResponse(MoodType.ErrorBased);
    Console.WriteLine(response.Message);
    // Output: "Oops! Something went sideways."
}
catch (Exception ex)
{
    // MoodyApi handles exceptions internally
    // and returns fallback messages
}
```

---

## ASP.NET Core Middleware 🌐

The middleware automatically wraps your API responses with mood data:

### Setup

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMoodyApi(options =>
{
    options.Mode = MoodType.KarmaBased;
    options.EnableSarcasm = true;
});

var app = builder.Build();

MoodyApi.Mood.Initialize(app.Services);
app.UseMoodyApi(); // Add this line

app.MapGet("/users", () => new { users = new[] { "Alice", "Bob" } });
app.Run();
```

### Response Format

```json
{
  "success": true,
  "statusCode": 200,
  "data": {
    "users": ["Alice", "Bob"]
  },
  "mood": {
    "message": "Your digital footprint creates ripples in the cosmic code.",
    "mood": "KarmaBased",
    "timestamp": "2024-01-15T10:30:45.123Z",
    "karmaScore": 7
  }
}
```

### Middleware Configuration

```csharp
// The middleware automatically skips certain paths
// - Swagger/OpenAPI endpoints
// - Static files (CSS, JS, images)
// - Health check endpoints
// - Non-JSON responses

// You can customize user identification
// Default: Uses X-User-ID header or IP address
```

---

## Configuration ⚙️

### Via Dependency Injection

```csharp
services.AddMoodyApi(options =>
{
    options.Mode = MoodType.Motivational;          // Default mood
    options.EnableSarcasm = true;                  // Enable sarcastic responses
    options.EnableMotivation = true;               // Enable motivational messages
    options.EnableKarma = true;                    // Enable karma tracking
    options.EnableTimeBased = true;                // Enable time-based messages
    options.KarmaThreshold = 5;                    // Karma threshold for special messages
});
```

### Via appsettings.json

```json
{
  "MoodOptions": {
    "Mode": "Sarcastic",
    "EnableSarcasm": true,
    "EnableMotivation": true,
    "EnableKarma": true,
    "EnableTimeBased": true,
    "KarmaThreshold": 10
  }
}
```

```csharp
// Bind from configuration
var moodOptions = builder.Configuration.GetSection("MoodOptions").Get<MoodOptions>();
builder.Services.AddMoodyApi(options =>
{
    options.Mode = moodOptions.Mode;
    options.EnableSarcasm = moodOptions.EnableSarcasm;
    // ... other properties
});
```

---

## Mood Types 🎭

| Mood Type | Description | Example Message |
|-----------|-------------|-----------------|
| **Neutral** | Standard, professional responses | "Request processed successfully." |
| **Motivational** | Uplifting, encouraging messages | "You are capable of amazing things." |
| **Sarcastic** | Witty, humorous responses | "Oh great, another API call. I'm thrilled. 🙄" |
| **KarmaBased** | Philosophical, karma-influenced messages | "What you seek is also seeking you." |
| **TimeBased** | Context-aware based on time of day | "Good morning! Ready to start fresh?" |
| **ErrorBased** | Graceful error handling with personality | "Houston, we have a problem... but also a solution." |

---

## Advanced Usage 🔧

### Custom Message Providers

```csharp
using MoodyApi.Providers;
using MoodyApi.Providers.Interfaces;
using Microsoft.Extensions.Logging;

public class CustomExcitedProvider : BaseProvider
{
    private static readonly string[] Messages = new[]
    {
        "This is AMAZING! 🎉",
        "WOW! Absolutely fantastic!",
        "I'm so excited I could explode! 💥"
    };

    public CustomExcitedProvider(ILogger? logger = null) : base(logger) { }

    protected override string GetMessageInternal()
    {
        var index = Random.Shared.Next(Messages.Length);
        return Messages[index];
    }
}
```

### Register Custom Providers

```csharp
services.AddMoodyApi();

// Override the provider dictionary
services.AddSingleton<Dictionary<MoodType, IMessageProvider>>(provider =>
{
    var logger = provider.GetService<ILogger<MoodEngine>>();
    return new Dictionary<MoodType, IMessageProvider>
    {
        [MoodType.Neutral] = new NeutralProvider(logger),
        [MoodType.Motivational] = new CustomExcitedProvider(logger), // Custom provider
        [MoodType.Sarcastic] = new SarcasmProvider(logger),
        [MoodType.KarmaBased] = new KarmaProvider(logger),
        [MoodType.TimeBased] = new TimeBasedProvider(logger),
        [MoodType.ErrorBased] = new ErrorBasedProvider(logger)
    };
});
```

### Manual Engine Usage

```csharp
// If you prefer not to use the static API
var moodEngine = serviceProvider.GetRequiredService<MoodEngine>();
var response = moodEngine.GetMoodResponse(MoodType.Motivational, "user-123");
```

---

## Best Practices 📝

### 1. User Identification
```csharp
// Use consistent user IDs for karma tracking
var response = MoodyApi.Mood.GetResponse(MoodType.KarmaBased, userId: user.Id);
```

### 2. Error Handling
```csharp
// MoodyApi handles exceptions gracefully, but you can add extra logging
try
{
    var message = MoodyApi.Mood.Get(MoodType.Sarcastic);
}
catch (InvalidOperationException ex)
{
    // This happens if MoodyApi is not initialized
    logger.LogError(ex, "MoodyApi not initialized");
}
```

### 3. Performance
```csharp
// Initialize once at startup
MoodyApi.Mood.Initialize(serviceProvider);

// Use throughout application lifecycle
// The library is thread-safe and optimized for concurrent use
```

### 4. Middleware Placement
```csharp
// Place UseMoodyApi() after routing but before endpoints
app.UseRouting();
app.UseMoodyApi();
app.MapControllers();
```

---

## Performance & Scaling 📈

- **Thread-Safe**: All operations are thread-safe
- **Memory Efficient**: Automatic cleanup of expired user data
- **Zero Allocation**: Message arrays are pre-allocated
- **Concurrent**: Uses `ConcurrentDictionary` for user tracking
- **Lightweight**: Minimal overhead per request

---

## Contributing 🤝

We welcome contributions! Here's how to get started:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Development Setup

```bash
git clone https://github.com/freecnsz/MoodyApi.git
cd MoodyApi
dotnet restore
dotnet build
dotnet test
```

---

## License 📄

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

## Support 💬

- **Issues**: [GitHub Issues](https://github.com/freecnsz/MoodyApi/issues)
- **Discussions**: [GitHub Discussions](https://github.com/freecnsz/MoodyApi/discussions)
- **Documentation**: [Wiki](https://github.com/freecnsz/MoodyApi/wiki)

---

## Acknowledgments 🙏

- Built with ❤️ for the .NET community
- Inspired by the need for more engaging API experiences
- Thanks to all contributors and users

---

**Made with 🎭 by [freecnsz](https://github.com/freecnsz)**