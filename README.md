
- [Food Facilities Challenge](#food-facilities-challenge)
  - [Technologies used](#technologies-used)
  - [Recommended IDE Setup](#recommended-ide-setup)
  - [Project Setup](#project-setup)
    - [Docker](#docker)
    - [.NET CLI](#net-cli)
    - [IDE](#ide)
- [Architectural Decisions](#architectural-decisions)
- [Critique](#critique)

# Food Facilities Challenge
This project is part of RadAi Software Engineer assignment. The challenge is to create an API that performs operations and filters to the data set given.

You can access the deployed project on https://foodfacilities.azurewebsites.net/ hosted by Azure.

## Technologies used
- `.net 7` as platform
- `ASP.NET Core` for web framework
- `Entity Framework` for database connection
- `Guard` for validations
- `MSTests` for tests
- `Swagger` for documentation

## Recommended IDE Setup
[Rider](https://www.jetbrains.com/rider/) or [Visual Studio](https://visualstudio.microsoft.com/).

## Project Setup
This project can run in three ways, using `Docker`, an `IDE` or using `.NET` commands.

### Docker
To run this project using Docker you will need to have a `Docker Engine` and `Docker CLI` installed and running.
#### Build Image
```sh
docker build . -t food-facilities
```
#### Run Image as Container
```sh
docker run -p 8080:80 food-facilities
```
This will run the webapp locally though `Docker` using port 8080. The app will run on http://localhost:8080/

### .NET CLI
To run this project using .NET CLI you will need to have a `.NET 7` installed. You can check your dotnet version using the command `dotnet --version`.

#### Build and Run
Go to the folder `FoodFacilities\src\FoodFacilities.API`and run the following command:
```sh
dotnet build
cd .\src\FoodFacilities.API\
dotnet run
```
This should run the API locally using port 5260 (but you can confirm the port in the command logs). The app should run on http://localhost:5260/

### IDE

If you have [Rider](https://www.jetbrains.com/rider/) or [Visual Studio](https://visualstudio.microsoft.com/) installed on your machine, you can run the project directly from it.
Open the solution `FoodFacilities.sln`, set the project `FoodFacilities.API` as the startup project, and run the project.

The API swagger will automatically open on your default browser.

## What this API do
This API fetches data about [Mobile Food Facilities in San Francisco](https://datasf.org) and perform custom filters to it. The data is fetched on the first request and stored in a memory cache for 05 minutes.

The Endpoints present in this API are documented through `Swagger`.

## Time spent
This projects was create in approximately 7 hours.

# Architectural Decisions
## Entity Framework
As this API consumes the data from an external source, it is susceptible to external errors from its providers. Because of this, it is always a good idea to have a copy or a cache of the data.
With that in mind, I used the [Entity Framework](https://learn.microsoft.com/pt-br/ef/) to cache the data in a memory storage. Although there are other simpler cache mechanism, the idea of using Entity Framework is that the storage could be quickly be moved to a real database if the data doesn't change very often.
Entity Framework is reliable enough to be used in any project, as it maintained by Microsoft and a huge community. 

## Dependency Injection
Dependency injection is a programming technique that makes a class independent of its dependencies. It is a good practice to use Dependency Injection in projects that have potential to grow, as it makes it easier to manage the dependencies of each classes and to apply SOLID principles.

To use Dependency Injection in this project, each dependency is injected as a `Singleton` or `Scoped` inside a `Registrable` class. The idea is that each folder inside the project will have it's own Registrable class responsible for injecting the classes for a specific context (Controller, Manager, Provider, etc). 

## Controller, Manager and Providers
This projects keeps a separation of logic between Controllers, Manager and Providers.
A Controller will be responsible for just defining the endpoint and make the validations necessary to the data received by the client.
A Manager will contain the business logic for a specific context and will be called by the Controller, whenever necessary.
The Provider will be responsible to fetch data from an external source and treat it in any necessary way.

# Critique
## What could be added
### Log and Metrics System
For a production grade application it would be a good idea to have a monitoring system that can show how the application is performing and present the logs from the application. One option for this is `Grafana` that has a complete stack with metric dashboards, traces and logs.

## What could be improved
### More precise distance calculation
In this project I'm using a method of calculating the distance between two points that is based on a cartesian plane, witch means that elevation and earths curvature is not considered. For an application that will be used for the context of a city, this is not a big problem as the earth's curvature won't affect to much in the calculations. But for an application that would work in the context of a bigger area, it would be a good idea to change the calculation for considering the curvature of earth and the ground elevation.   
### Cache Mechanism
For an application that constantly requires data from an external provider is always a good idea to have some kind of cache mechanism, so a problem on the external source doesn't stop our application. In the context of this project, I used Entity Framework with an In Memory cache for the data, but other options could be considered depending on the kind and external provider.
Other cache options are using a simpler memory cache from .net (removing the need for `Entity Framework`); or if the application needs to be horizontally scaled (running multiple instances) is it possible to use an external cache database, like a `Redis` Cache.
