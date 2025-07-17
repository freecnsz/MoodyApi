# MoodyApi

[![NuGet](https://img.shields.io/nuget/v/MoodyApi.svg)](https://www.nuget.org/packages/MoodyApi)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Build Status](https://img.shields.io/github/actions/workflow/status/freecnsz/MoodyApi/ci.yml?branch=main)](https://github.com/freecnsz/MoodyApi/actions)

---

MoodyApi is a modern, extensible .NET library designed to generate context-aware, mood-based messages for your applications and APIs. With seamless Dependency Injection support and ASP.NET Core middleware integration, MoodyApi helps you deliver engaging, dynamic responses tailored to user sentiment, system state, or business logic.

---

## Table of Contents
- [Features](#features)
- [Installation](#installation)
- [Getting Started](#getting-started)
- [ASP.NET Core Middleware](#aspnet-core-middleware)
- [Configuration](#configuration)
- [Advanced Usage: Custom Providers](#advanced-usage-custom-providers)
- [License](#license)
- [Support & Contributing](#support--contributing)

---

## Features
- **Multiple built-in moods:** Neutral, Motivational, Sarcastic, Karma-Based, Time-Based, Error-Based
- **Extensible provider model:** Easily add your own custom moods and message logic
- **User-based karma tracking:** Personalize responses based on user activity
- **Flexible configuration:** Fine-tune behavior via DI or appsettings
- **ASP.NET Core middleware:** Effortlessly wrap API responses with mood data
- **Comprehensive test coverage**

---

## Installation

Install from NuGet:

```shell
Install-Package MoodyApi
```

Or using the .NET CLI:

```shell
dotnet add package MoodyApi
```

---

## Getting Started

### 1. Register MoodyApi Services

```csharp
using MoodyApi.Extensions;

var services = new ServiceCollection();
services.AddMoodyApi(options =>
{
    options.Mode = MoodType.Motivational;
    options.EnableSarcasm = false;
});
var serviceProvider = services.BuildServiceProvider();

// Initialize the static API
MoodyApi.Mood.Initialize(serviceProvider);
```

### 2. Generate Mood-Based Messages

```csharp
using MoodyApi.Models;

// Get a random message for a specific mood
global::MoodyApi.Models.MoodType mood = MoodType.Sarcastic;
string message = MoodyApi.Mood.Get(mood);

// Get a detailed mood response (with optional user tracking)
var response = MoodyApi.Mood.GetResponse(mood, userId: "user-123");
```

---

## ASP.NET Core Middleware

Seamlessly wrap your API responses with mood data:

```csharp
using MoodyApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMoodyApi();
var app = builder.Build();

MoodyApi.Mood.Initialize(app.Services);
app.UseMoodyApi();

app.MapGet("/hello", () => new { message = "Hello World" });
app.Run();
```

The middleware will automatically inject mood information into every HTTP response.

---

## Configuration

Configure MoodyApi via DI:

```csharp
services.AddMoodyApi(options =>
{
    options.Mode = MoodType.KarmaBased;
    options.KarmaThreshold = 10;
    options.EnableMotivation = true;
});
```

Or bind from `appsettings.json` using your own configuration logic.

---

## Advanced Usage: Custom Providers

You can extend MoodyApi by implementing your own message provider:

```csharp
using MoodyApi.Providers.Interfaces;
using MoodyApi.Models;

public class CustomExcitedProvider : IMessageProvider
{
    public string GetMessage() => "This is so exciting!";
}

// Register your provider in DI
services.AddSingleton<IMessageProvider, CustomExcitedProvider>();
services.AddSingleton<Dictionary<MoodType, IMessageProvider>>(provider =>
{
    var dict = new Dictionary<MoodType, IMessageProvider>
    {
        [MoodType.Neutral] = new NeutralProvider(),
        // ... other built-in providers
        [MoodType.ErrorBased] = new ErrorBasedProvider(),
        // Add your custom provider
        [MoodType.Custom] = provider.GetRequiredService<CustomExcitedProvider>()
    };
    return dict;
});
```

---

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

## Support & Contributing

We welcome issues, feature requests, and contributions! Please open an issue or submit a pull request on [GitHub](https://github.com/freecnsz/MoodyApi).

For questions or support, contact the maintainer via GitHub or open a discussion. 