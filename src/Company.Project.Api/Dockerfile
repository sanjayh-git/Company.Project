# BUILD STAGE
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# copy csproj and restore
COPY src/Company.Project.Data/*.csproj ./Company.Project.Data/
COPY src/Company.Project.Application/*.csproj ./Company.Project.Application/
COPY src/Company.Project.Infrastructure/*.csproj ./Company.Project.Infrastructure/
COPY src/Company.Project.Api/*.csproj ./Company.Project.Api/

RUN dotnet restore ./Company.Project.Api/Company.Project.Api.csproj

# copy the rest of the source
COPY src/. ./

WORKDIR /src/Company.Project.Api
RUN dotnet publish Company.Project.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# RUNTIME STAGE
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish ./

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "Company.Project.Api.dll"]
