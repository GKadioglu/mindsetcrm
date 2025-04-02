FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Logging ./Logging
COPY CustomerService/CustomerService.csproj ./CustomerService/
COPY Logging/Logging.csproj ./Logging/

RUN dotnet restore ./CustomerService/CustomerService.csproj

COPY . .

WORKDIR /src/CustomerService
RUN dotnet build -c Release -o /app/build

FROM build AS publish
WORKDIR /src/CustomerService
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerService.dll"]
