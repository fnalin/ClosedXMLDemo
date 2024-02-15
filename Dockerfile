FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DemoUploadReport.Api/DemoUploadReport.Api.csproj", "DemoUploadReport.Api/"]
RUN dotnet restore "DemoUploadReport.Api/DemoUploadReport.Api.csproj"
COPY . .
WORKDIR "/src/DemoUploadReport.Api"
RUN dotnet build "DemoUploadReport.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DemoUploadReport.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoUploadReport.Api.dll"]
