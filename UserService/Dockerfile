FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Logging ./Logging
COPY UserService/UserService.csproj ./UserService/
COPY Logging/Logging.csproj ./Logging/

RUN dotnet restore ./UserService/UserService.csproj

COPY . .

WORKDIR /src/UserService
RUN dotnet build -c Release -o /app/build

FROM build AS publish
WORKDIR /src/UserService
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.dll"]
