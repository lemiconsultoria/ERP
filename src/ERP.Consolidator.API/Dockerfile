#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ERP.Consolidator.API/ERP.Consolidator.API.csproj", "ERP.Consolidator.API/"]
COPY ["ERP.Consolidator.IoC/ERP.Consolidator.IoC.csproj", "ERP.Consolidator.IoC/"]
COPY ["ERP.Consolidator.Application/ERP.Consolidator.Application.csproj", "ERP.Consolidator.Application/"]
COPY ["ERP.Consolidator.Domain/ERP.Consolidator.Domain.csproj", "ERP.Consolidator.Domain/"]
COPY ["ERP.Core/ERP.Core.csproj", "ERP.Core/"]
COPY ["ERP.Domain/ERP.Domain.csproj", "ERP.Domain/"]
COPY ["ERP.Consolidator.Infra.Data/ERP.Consolidator.Infra.Data.csproj", "ERP.Consolidator.Infra.Data/"]
COPY ["ERP.Consolidator.Infra.Jobs/ERP.Consolidator.Infra.Jobs.csproj", "ERP.Consolidator.Infra.Jobs/"]
COPY ["ERP.Consolidator.Infra.Messaging/ERP.Consolidator.Infra.Messaging.csproj", "ERP.Consolidator.Infra.Messaging/"]
RUN dotnet restore "ERP.Consolidator.API/ERP.Consolidator.API.csproj"
COPY . .
WORKDIR "/src/ERP.Consolidator.API"
RUN dotnet build "ERP.Consolidator.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ERP.Consolidator.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ERP.Consolidator.API.dll"]