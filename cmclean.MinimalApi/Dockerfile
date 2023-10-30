#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["cmclean.MinimalApi/cmclean.MinimalApi.csproj", "cmclean.MinimalApi/"]
COPY ["cmclean.Infrastructure/cmclean.Infrastructure.csproj", "cmclean.Infrastructure/"]
COPY ["cmclean.Application/cmclean.Application.csproj", "cmclean.Application/"]
COPY ["cmclean.Domain/cmclean.Domain.csproj", "cmclean.Domain/"]
COPY ["cmclean.Persistence/cmclean.Persistence.csproj", "cmclean.Persistence/"]
COPY ["Common.Logging/Common.Logging.csproj", "Common.Logging/"]
COPY ["Common.OpenTelemetry/Common.OpenTelemetry.csproj", "Common.OpenTelemetry/"]
RUN dotnet restore "cmclean.MinimalApi/cmclean.MinimalApi.csproj"
COPY . .
WORKDIR "/src/cmclean.MinimalApi"
RUN dotnet build "cmclean.MinimalApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "cmclean.MinimalApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "cmclean.MinimalApi.dll"]