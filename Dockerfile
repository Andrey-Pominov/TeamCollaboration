FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["TeamCollaboration.sln", "./"]
COPY ["TeamCollaboration.Server/TeamCollaboration.Server.csproj", "TeamCollaboration.Server/"]
COPY ["TeamCollaboration.Application/TaeamCollaboration.Application.csproj", "TeamCollaboration.Application/"]
COPY ["TeamCollaboration.Infrastructure/TeamCollaboration.Infrastructure.csproj", "TeamCollaboration.Infrastructure/"]
COPY ["TeamCollaboration.Domain/TeamCollaboration.Domain.csproj", "TeamCollaboration.Domain/"]

RUN dotnet restore "TeamCollaboration.sln"

COPY . .
WORKDIR "/src/TeamCollaboration.Server"
RUN dotnet build "TeamCollaboration.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TeamCollaboration.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeamCollaboration.Server.dll"]
