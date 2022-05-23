FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY . ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

ARG APP_PORT
ENV APP_PORT $APP_PORT
EXPOSE $APP_PORT

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "Roadmap.dll"]