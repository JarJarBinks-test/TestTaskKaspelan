#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./TestTaskKaspelan.Notification/TestTaskKaspelan.Notification.csproj", "TestTaskKaspelan.Notification/"]
COPY ["./TestTaskKaspelan.Notification.Services/TestTaskKaspelan.Notification.Services.csproj", "TestTaskKaspelan.Notification.Services/"]
COPY ["./TestTaskKaspelan.Common/TestTaskKaspelan.Common.csproj", "TestTaskKaspelan.Common/"]
RUN dotnet restore "./TestTaskKaspelan.Notification/TestTaskKaspelan.Notification.csproj"
COPY . .
WORKDIR "/src/TestTaskKaspelan.Notification"
RUN dotnet build "./TestTaskKaspelan.Notification.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TestTaskKaspelan.Notification.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestTaskKaspelan.Notification.dll"]
