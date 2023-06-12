#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ERP.Crud.API/ERP.Crud.API.csproj", "ERP.Crud.API/"]
COPY ["ERP.Core/ERP.Core.csproj", "ERP.Core/"]
COPY ["ERP.Crud.IoC/ERP.Crud.IoC.csproj", "ERP.Crud.IoC/"]
COPY ["ERP.Crud.Application/ERP.Crud.Application.csproj", "ERP.Crud.Application/"]
COPY ["ERP.Crud.Domain/ERP.Crud.Domain.csproj", "ERP.Crud.Domain/"]
COPY ["ERP.Domain/ERP.Domain.csproj", "ERP.Domain/"]
COPY ["ERP.Crud.Infra.Data/ERP.Crud.Infra.Data.csproj", "ERP.Crud.Infra.Data/"]
COPY ["ERP.Crud.Infra.Messaging/ERP.Crud.Infra.Messaging.csproj", "ERP.Crud.Infra.Messaging/"]
RUN dotnet restore "ERP.Crud.API/ERP.Crud.API.csproj"
COPY . .
WORKDIR "/src/ERP.Crud.API"
RUN dotnet build "ERP.Crud.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ERP.Crud.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ERP.Crud.API.dll"]