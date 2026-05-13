# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj files and restore dependencies
COPY ["JPLearn.Api/JPLearn.Api.csproj", "JPLearn.Api/"]
COPY ["JPLearn.Core/JPLearn.Core.csproj", "JPLearn.Core/"]
COPY ["JPLearn.Infrastructure/JPLearn.Infrastructure.csproj", "JPLearn.Infrastructure/"]

RUN dotnet restore "JPLearn.Api/JPLearn.Api.csproj"

# Copy the rest of the code and build
COPY . .
WORKDIR "/app/JPLearn.Api"
RUN dotnet publish "JPLearn.Api.csproj" -c Release -o /app/out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Expose port 8080 (Render uses PORT env var, but we'll default to 8080)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "JPLearn.Api.dll"]
