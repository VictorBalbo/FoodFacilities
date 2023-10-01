FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/FoodFacilities.API/FoodFacilities.API.csproj", "src/FoodFacilities.API/"]
COPY ["src/FoodFacilities.Models/FoodFacilities.Models.csproj", "src/FoodFacilities.Models/"]
COPY ["src/FoodFacilities.Data/FoodFacilities.Data.csproj", "src/FoodFacilities.Data/"]
RUN dotnet restore "src/FoodFacilities.API/FoodFacilities.API.csproj"

COPY . .
WORKDIR "/src/src/FoodFacilities.API"
RUN dotnet build "FoodFacilities.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FoodFacilities.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FoodFacilities.API.dll"]
