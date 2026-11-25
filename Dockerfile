FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS dev

# Base Development Packages
RUN apk update \
  && apk upgrade \
  && apk add ca-certificates wget && update-ca-certificates \
  && apk add --no-cache --update \
  git \
  curl \
  wget \
  bash \
  make \
  rsync \
  nano \
  zsh \
  zsh-vcs \
  docker-cli \
  docker-cli-compose \
  openssh \
  && git config --global --add safe.directory /template-dotnet-core-console-app


# zsh
RUN sh -c "$(curl -fsSL https://raw.githubusercontent.com/ohmyzsh/ohmyzsh/master/tools/install.sh)" "" --unattended && \
  git clone --depth 1 https://github.com/zsh-users/zsh-autosuggestions.git /root/.oh-my-zsh/custom/plugins/zsh-autosuggestions && \
  git clone --depth 1 https://github.com/memark/zsh-dotnet-completion.git /root/.oh-my-zsh/custom/plugins/zsh-dotnet-completion && \
  mkdir -p /cache

# Working Folder
WORKDIR /template-dotnet-core-console-app
COPY ["src/TemplateDotnetCoreConsoleApp.Api/TemplateDotnetCoreConsoleApp.Api.csproj", "src/TemplateDotnetCoreConsoleApp.Api/"]
COPY ["src/TemplateDotnetCoreConsoleApp.Client/TemplateDotnetCoreConsoleApp.Client.csproj", "src/TemplateDotnetCoreConsoleApp.Client/"]
COPY ["src/TemplateDotnetCoreConsoleApp.Core/TemplateDotnetCoreConsoleApp.Core.csproj", "src/TemplateDotnetCoreConsoleApp.Core/"]
COPY ["tests/TemplateDotnetCoreConsoleApp.Api.Tests/TemplateDotnetCoreConsoleApp.Api.Tests.csproj", "tests/TemplateDotnetCoreConsoleApp.Api.Tests/"]
COPY ["tests/TemplateDotnetCoreConsoleApp.Core.Tests/TemplateDotnetCoreConsoleApp.Core.Tests.csproj", "tests/TemplateDotnetCoreConsoleApp.Core.Tests/"]

COPY Directory.Build.props TemplateDotnetCoreConsoleApp.sln ./
RUN dotnet restore

ENV PATH="/root/.dotnet/tools:${PATH}"
ENV TERM=xterm-256color
RUN printf 'export PS1="\[\e[0;34;0;33m\][DCKR]\[\e[0m\] \\t \[\e[40;38;5;28m\][\w]\[\e[0m\] \$ "' >> ~/.bashrc

COPY zshrc /root/.zshrc

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY Directory.Build.props ./
COPY src/TemplateDotnetCoreConsoleApp.Api/TemplateDotnetCoreConsoleApp.Api.csproj src/TemplateDotnetCoreConsoleApp.Api/
COPY src/TemplateDotnetCoreConsoleApp.Client/TemplateDotnetCoreConsoleApp.Client.csproj src/TemplateDotnetCoreConsoleApp.Client/
COPY src/TemplateDotnetCoreConsoleApp.Core/TemplateDotnetCoreConsoleApp.Core.csproj src/TemplateDotnetCoreConsoleApp.Core/
RUN dotnet restore src/TemplateDotnetCoreConsoleApp.Api/TemplateDotnetCoreConsoleApp.Api.csproj

COPY src/ src/
ARG VERSION=1.0.1
RUN dotnet publish src/TemplateDotnetCoreConsoleApp.Api/TemplateDotnetCoreConsoleApp.Api.csproj \
    --no-restore -c Release -o /app/publish \
    -p:FileVersion="$VERSION" -p:VersionPrefix="$VERSION" -p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
ARG VERSION=1.0.1
LABEL version="$VERSION"
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TemplateDotnetCoreConsoleApp.Api.dll"]
