#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["WorkforceManagementAPI.WEB/WorkforceManagementAPI.WEB.csproj", "WorkforceManagementAPI.WEB/"]
COPY ["WorkforceManagementAPI.DAL/WorkforceManagementAPI.DAL.csproj", "WorkforceManagementAPI.DAL/"]
COPY ["WorkforceManagementAPI.DTO/WorkforceManagementAPI.DTO.csproj", "WorkforceManagementAPI.DTO/"]
COPY ["WorkforceManagementAPI.BLL/WorkforceManagementAPI.BLL.csproj", "WorkforceManagementAPI.BLL/"]
COPY ["WorkforceManagementAPI.Common/WorkforceManagementAPI.Common.csproj", "WorkforceManagementAPI.Common/"]
RUN dotnet restore "WorkforceManagementAPI.WEB/WorkforceManagementAPI.WEB.csproj"
COPY . .
WORKDIR "/src/WorkforceManagementAPI.WEB"
RUN dotnet build "WorkforceManagementAPI.WEB.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorkforceManagementAPI.WEB.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkforceManagementAPI.WEB.dll"]