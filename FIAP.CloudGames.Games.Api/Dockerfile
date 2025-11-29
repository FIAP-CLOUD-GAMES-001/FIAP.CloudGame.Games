# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
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

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FIAP.CloudGames.Games.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FIAP.CloudGames.Games.Api.dll"]




