FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["GamePlayService/GamePlayService.csproj", "GamePlayService/"]
RUN dotnet restore "GamePlayService/GamePlayService.csproj"

COPY . .
WORKDIR "/src/GamePlayService"
RUN dotnet build "GamePlayService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "GamePlayService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "GamePlayService.dll"]