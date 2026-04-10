FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["thuydung484.csproj", "./"]
RUN dotnet restore "./thuydung484.csproj"

COPY . .
RUN dotnet build "./thuydung484.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./thuydung484.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "thuydung484.dll"]