FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Ingestion.API/Ingestion.API.csproj", "Ingestion.API/"]
RUN dotnet restore "Ingestion.API/Ingestion.API.csproj"
COPY . .
WORKDIR "/src/Ingestion.API"
RUN dotnet build "Ingestion.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ingestion.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ingestion.API.dll"]
