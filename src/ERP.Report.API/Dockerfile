#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ERP.Report.API/ERP.Report.API.csproj", "ERP.Report.API/"]
COPY ["ERP.Core/ERP.Core.csproj", "ERP.Core/"]
COPY ["ERP.Report.Application/ERP.Report.Application.csproj", "ERP.Report.Application/"]
COPY ["ERP.Report.Domain/ERP.Report.Domain.csproj", "ERP.Report.Domain/"]
COPY ["ERP.Report.IoC/ERP.Report.IoC.csproj", "ERP.Report.IoC/"]
COPY ["ERP.Report.Infra.Data/ERP.Report.Infra.Data.csproj", "ERP.Report.Infra.Data/"]
RUN dotnet restore "ERP.Report.API/ERP.Report.API.csproj"
COPY . .
WORKDIR "/src/ERP.Report.API"
RUN dotnet build "ERP.Report.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ERP.Report.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ERP.Report.API.dll"]