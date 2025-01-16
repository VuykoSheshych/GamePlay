FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
EXPOSE 80
COPY bin/Release/net8.0/publish/ .

COPY ["./GamePlayService.csproj", "./"]
RUN dotnet restore "./GamePlayService.csproj"

COPY . .
RUN dotnet build "./GamePlayService.csproj" -c Debug --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=build /src .

ENTRYPOINT ["dotnet", "GamePlayService.dll"]