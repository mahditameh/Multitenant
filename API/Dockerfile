# مرحله build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# کپی solution
COPY ../OrderManagement.sln ./

# کپی پروژه‌ها
COPY ../API ./API
COPY ../Application ./Application
COPY ../Domain ./Domain
COPY ../Infrastructure ./Infrastructure

# restore
RUN dotnet restore API/API.csproj

# build + publish
RUN dotnet publish API/API.csproj -c Release -o /app/publish

# مرحله runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "API.dll"]