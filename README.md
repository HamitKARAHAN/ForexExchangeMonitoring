# ForexExchangeMonitoring

 ### ⚡️ Quickstart - Summary

  This is a simple project for creating a WebApp with ASP.NET Core with the principles of Clean Architecture. 
In project, we are taking Forex Exchange Rates for chosen currencies with the help of some free API's. Firstly, We have a Worker running in Background once every 30 minutes between 9 am and 6 pm on weekdays. Our Worker sending request to API provider, taking our Exchange datas and recording our local database with the help of our DbContext. In the other hand, We have a ASP.NET Core MVC Project taking this datas from our DbContext and routing to our endpoints.

## Technologies & Architectures

- [ASP.NET Core 5.0](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-5.0)

- [Clean Architecture](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures#clean-architecture)

## Getting Started

- Clone Project to from Visual Stuido or VS Code
- Open the project folder. Then open `~ForexExchange/appsettings.json` Change your Sql Connection String from here and `~ForexExchange.Worker/appsettings.json` here (if you want to use Visual Studio local mssqlDb, don't do anything)
- Set `ForexExchange` to your Startup Project
- Migrate the database using command  `Add-Migration "MigrationName" -Context ForexCurrencyModelDbContext`
- Update the databse using command `Update-Database -Context ForexCurrencyModelDbContext`
- Right Click solution. Open Properties tab. Choose Multiple Startup Projects. Choose ForexExchange and ForexExchange.Worker
- Save and Run

## Overview

### Project Architecture

![dddd](https://user-images.githubusercontent.com/73249548/142181292-f3d4d656-b8c2-49b6-ad39-4530635b367e.png)



### Domain

This will contain all entities, interfaces, types and logic specific to the domain layer. Here is our db model relationships. 

![diagram2](https://user-images.githubusercontent.com/73249548/142172677-39f3c261-111a-4ae8-89a9-5430e228e195.png)


### Application

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a repository, a new interface would be added to application and an implementation would be created within infrastructure layer.

### Infrastructure

This layer contains classes for accessing external resources such as file systems, in our case just our Database inside Repository with help of our DbContext. Repository comminucate with Application Services with the help of Domain Layer.

### Web UI

This layer is a single page application based on ASP.NET Core 5. This layer depends on both the Application and Infrastructure layers. Dependency Injection set up with this two layers and our DbContext.

## Support

If you have If you are having problems, please let me know [Github](https://github.com/HamitKARAHAN)


