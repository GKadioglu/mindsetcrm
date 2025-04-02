FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ./Logging ./Logging

COPY ./APIGateway/APIGateway.csproj ./APIGateway/
COPY ./APIGateway/appsettings.json ./APIGateway/
COPY ./APIGateway/ocelot.json ./APIGateway/

WORKDIR /src/APIGateway

RUN dotnet restore "APIGateway.csproj"
COPY ./APIGateway/. ./
RUN dotnet build -c Release -o /app/build

FROM build AS publish
WORKDIR /src/APIGateway
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "APIGateway.dll"]
