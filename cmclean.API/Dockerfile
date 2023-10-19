#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5218

ENV ASPNETCORE_URLS=http://+:5218

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["cmclean.API/cmclean.API.csproj", "cmclean.API/"]
COPY ["cmclean.Application/cmclean.Application.csproj", "cmclean.Application/"]
COPY ["cmclean.Domain/cmclean.Domain.csproj", "cmclean.Domain/"]
COPY ["cmclean.Infrastructure/cmclean.Infrastructure.csproj", "cmclean.Infrastructure/"]
RUN dotnet restore "cmclean.API/cmclean.API.csproj"
COPY . .
WORKDIR "/src/cmclean.API"
RUN dotnet build "cmclean.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "cmclean.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "cmclean.API.dll"]