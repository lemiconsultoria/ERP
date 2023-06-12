#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ERP.Identity.API/ERP.Identity.API.csproj", "ERP.Identity.API/"]
COPY ["ERP.Core/ERP.Core.csproj", "ERP.Core/"]
COPY ["ERP.Identity.Domain/ERP.Identity.Domain.csproj", "ERP.Identity.Domain/"]
COPY ["ERP.Identity.IoC/ERP.Identity.IoC.csproj", "ERP.Identity.IoC/"]
COPY ["ERP.Identity.Application/ERP.Identity.Application.csproj", "ERP.Identity.Application/"]
COPY ["ERP.Identity.Infra.Util/ERP.Identity.Infra.Util.csproj", "ERP.Identity.Infra.Util/"]
COPY ["ERP.Identity.Infra.Data/ERP.Identity.Infra.Data.csproj", "ERP.Identity.Infra.Data/"]
RUN dotnet restore "ERP.Identity.API/ERP.Identity.API.csproj"
COPY . .
WORKDIR "/src/ERP.Identity.API"
RUN dotnet build "ERP.Identity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ERP.Identity.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ERP.Identity.API.dll"]