FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["src/Roadmap/Roadmap.csproj", "src/Roadmap/"]
RUN dotnet restore "src/Roadmap/Roadmap.csproj"

COPY . .

WORKDIR "/src/Roadmap/"

RUN dotnet build "merchandise-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "merchandise-service.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

WORKDIR /app

ARG APP_PORT
ENV APP_PORT $APP_PORT
EXPOSE $APP_PORT

FROM runtime AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "merchandise-service.dll"]