# ---------- Build ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /src

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish

# ---------- Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

# Render sets PORT automatically
ENV ASPNETCORE_URLS=http://0.0.0.0:$PORT

ENTRYPOINT ["dotnet", "Mobilis-Real-Time-Assistant.dll"]
