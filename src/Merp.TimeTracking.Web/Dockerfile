#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Merp.TimeTracking.Web/Merp.TimeTracking.Web.csproj", "src/Merp.TimeTracking.Web/"]
COPY ["src/Merp.TimeTracking.Web.Core/Merp.TimeTracking.Web.Core.csproj", "src/Merp.TimeTracking.Web.Core/"]
COPY ["src/Merp.Web/Merp.Web.csproj", "src/Merp.Web/"]
COPY ["src/Merp.TimeTracking.TaskManagement.CommandStack/Merp.TimeTracking.TaskManagement.CommandStack.csproj", "src/Merp.TimeTracking.TaskManagement.CommandStack/"]
COPY ["src/Merp.Domain/Merp.Domain.csproj", "src/Merp.Domain/"]
COPY ["src/Merp.TimeTracking.TaskManagement.QueryStack/Merp.TimeTracking.TaskManagement.QueryStack.csproj", "src/Merp.TimeTracking.TaskManagement.QueryStack/"]
RUN dotnet restore "src/Merp.TimeTracking.Web/Merp.TimeTracking.Web.csproj"
COPY . .
WORKDIR "/src/src/Merp.TimeTracking.Web"
RUN dotnet build "Merp.TimeTracking.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Merp.TimeTracking.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Merp.TimeTracking.Web.dll"]