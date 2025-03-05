FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY Directory.Build.props ./
COPY ["../src/API/FactoryTalent.API/FactoryTalent.API.csproj", "API/FactoryTalent.API/"]
COPY ["../src/Common/FactoryTalent.Common.Application/FactoryTalent.Common.Application.csproj", "Common/FactoryTalent.Common.Application/"]
COPY ["../src/Common/FactoryTalent.Common.Domain/FactoryTalent.Common.Domain.csproj", "Common/FactoryTalent.Common.Domain/"]
COPY ["../src/Common/FactoryTalent.Common.Infrastructure/FactoryTalent.Common.Infrastructure.csproj", "Common/FactoryTalent.Common.Infrastructure/"]
COPY ["../src/Common/FactoryTalent.Common.Presentation/FactoryTalent.Common.Presentation.csproj", "Common/FactoryTalent.Common.Presentation/"]
COPY ["../src/Modules/Users/FactoryTalent.Modules.Users.Application/FactoryTalent.Modules.Users.Application.csproj", "Modules/Users/FactoryTalent.Modules.Users.Application/"]
COPY ["../src/Modules/Users/FactoryTalent.Modules.Users.Domain/FactoryTalent.Modules.Users.Domain.csproj", "Modules/Users/FactoryTalent.Modules.Users.Domain/"]
COPY ["../src/Modules/Users/FactoryTalent.Modules.Users.Infrastructure/FactoryTalent.Modules.Users.Infrastructure.csproj", "Modules/Users/FactoryTalent.Modules.Users.Infrastructure/"]
COPY ["../src/Modules/Users/FactoryTalent.Modules.Users.IntegrationEvents/FactoryTalent.Modules.Users.IntegrationEvents.csproj", "Modules/Users/FactoryTalent.Modules.Users.IntegrationEvents/"]
COPY ["../src/Modules/Users/FactoryTalent.Modules.Users.Presentation/FactoryTalent.Modules.Users.Presentation.csproj", "Modules/Users/FactoryTalent.Modules.Users.Presentation/"]
RUN dotnet restore "./API/FactoryTalent.API/FactoryTalent.API.csproj"

COPY . .
COPY /src/ .

WORKDIR "/src/API/FactoryTalent.API/"

RUN dotnet build "./FactoryTalent.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FactoryTalent.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


ENTRYPOINT ["dotnet", "FactoryTalent.API.dll"]