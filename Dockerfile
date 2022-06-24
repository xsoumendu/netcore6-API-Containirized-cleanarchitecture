FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# copy all the layers' csproj files into respective folders
COPY ["./ContainerApp.Contracts/ContainerApp.Contracts.csproj", "src/ContainerApp.Contracts/"]
COPY ["./ContainerApp.Migrations/ContainerApp.Migrations.csproj", "src/ContainerApp.Migrations/"]
COPY ["./ContainerApp.Infrastructure/ContainerApp.Infrastructure.csproj", "src/ContainerApp.Infrastructure/"]
COPY ["./ContainerApp.Core/ContainerApp.Core.csproj", "src/ContainerApp.Core/"]
COPY ["./ContainerApp.API/ContainerApp.API.csproj", "src/ContainerApp.API/"]

# run restore over API project - this pulls restore over the dependent projects as well
RUN dotnet restore "src/ContainerApp.API/ContainerApp.API.csproj"

COPY . .

# run build over the API project
WORKDIR "/src/ContainerApp.API/"
RUN dotnet build -c Release -o /app/build

# run publish over the API project
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .
RUN ls -l
ENTRYPOINT [ "dotnet", "ContainerApp.API.dll" ]