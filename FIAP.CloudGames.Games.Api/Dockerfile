# Esta fase é usada para compilar  o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
USER root
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia a solution e os arquivos .csproj
COPY ["FIAP.CloudGames.Games.sln", "."]
COPY ["FIAP.CloudGames.Games.Api/FIAP.CloudGames.Games.Api.csproj", "FIAP.CloudGames.Games.Api/"]
COPY ["FIAP.CloudGames.Games.Domain/FIAP.CloudGames.Games.Domain.csproj", "FIAP.CloudGames.Games.Domain/"]
COPY ["FIAP.CloudGames.Games.Infrastructure/FIAP.CloudGames.Games.Infrastructure.csproj", "FIAP.CloudGames.Games.Infrastructure/"]
COPY ["FIAP.CloudGames.Games.Service/FIAP.CloudGames.Games.Service.csproj", "FIAP.CloudGames.Games.Service/"]

# Restaura as dependências
RUN dotnet restore "FIAP.CloudGames.Games.sln"

# Copia todo o código fonte
COPY . .

# Compila o projeto
WORKDIR "/src/FIAP.CloudGames.Games.Api"
RUN dotnet build "FIAP.CloudGames.Games.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publica o projeto
RUN dotnet publish "FIAP.CloudGames.Games.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
USER root

# Configurando timezone para America/Sao_Paulo
RUN apk add --no-cache tzdata icu-libs \
    && cp /usr/share/zoneinfo/America/Sao_Paulo /etc/localtime \
    && echo "America/Sao_Paulo" > /etc/timezone \
    && apk del tzdata

ENV TZ=America/Sao_Paulo
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

WORKDIR /app
COPY --from=build /app/publish .

RUN chown -R $APP_UID /app

EXPOSE 8080
EXPOSE 8081

USER $APP_UID
ENTRYPOINT ["dotnet", "FIAP.CloudGames.Games.Api.dll"]
