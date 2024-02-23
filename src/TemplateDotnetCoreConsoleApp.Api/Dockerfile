FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

##
# Build build project
##
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# find src* | grep '.csproj' | grep -v obj | sed "s|src/||" | sed "s|\(.*\)/\([^/]*\)\.csproj|COPY [\"\1/\2.csproj\", \"\1/\"]|"
COPY ["TemplateDotnetCoreConsoleApp.Api/TemplateDotnetCoreConsoleApp.Api.csproj", "TemplateDotnetCoreConsoleApp.Api/"]
COPY ["TemplateDotnetCoreConsoleApp.Client/TemplateDotnetCoreConsoleApp.Client.csproj", "TemplateDotnetCoreConsoleApp.Client/"]
COPY ["TemplateDotnetCoreConsoleApp.Core/TemplateDotnetCoreConsoleApp.Core.csproj", "TemplateDotnetCoreConsoleApp.Core/"]

RUN dotnet restore "TemplateDotnetCoreConsoleApp.Api/TemplateDotnetCoreConsoleApp.Api.csproj"

COPY . .
WORKDIR "/src/TemplateDotnetCoreConsoleApp.Api"
RUN dotnet build "TemplateDotnetCoreConsoleApp.Api.csproj" -c Release -o /app/build

##
# Publish
##
FROM build AS publish
ARG  VERSION=1.0.1
RUN dotnet publish "TemplateDotnetCoreConsoleApp.Api.csproj" -p:FileVersion=${VERSION} -p:VersionPrefix=${VERSION} -c Release -o /app/publish /p:UseAppHost=false

##
# Make the release
##
FROM base AS final
WORKDIR /app
ARG  VERSION=1.0.1
LABEL version="${VERSION}"
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet","TemplateDotnetCoreConsoleApp.Api.dll"]
