FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY . .
RUN dotnet restore "./src/Ingestion.API/Ingestion.API.csproj"
WORKDIR "/src/Ingestion.API"
RUN dotnet build "./Ingestion.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Ingestion.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ingestion.API.dll"]
