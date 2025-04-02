FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Logging ./Logging
COPY SalesService/SalesService.csproj ./SalesService/
COPY Logging/Logging.csproj ./Logging/

RUN dotnet restore ./SalesService/SalesService.csproj

COPY . .

WORKDIR /src/SalesService
RUN dotnet build -c Release -o /app/build

FROM build AS publish
WORKDIR /src/SalesService
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SalesService.dll"]
