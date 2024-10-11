FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ChessWebApi/ChessWebApi.csproj", "src/ChessWebApi/"]
COPY ["src/Chess.Core/Chess.Core.csproj", "src/Chess.Core/"]
COPY ["src/External/ChessCoreEngine/ChessCoreEngine.csproj", "src/External/ChessCoreEngine/"]
RUN dotnet restore "src/ChessWebApi/ChessWebApi.csproj"
COPY . .
WORKDIR "/src/src/ChessWebApi"
RUN dotnet build "ChessWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ChessWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChessWebApi.dll"]
