#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Merp.Registry.Web/Merp.Registry.Web.csproj", "src/Merp.Registry.Web/"]
COPY ["src/Merp.Registry.Web.Core/Merp.Registry.Web.Core.csproj", "src/Merp.Registry.Web.Core/"]
COPY ["src/Merp.Registry.QueryStack/Merp.Registry.QueryStack.csproj", "src/Merp.Registry.QueryStack/"]
COPY ["src/Merp.Registry.CommandStack/Merp.Registry.CommandStack.csproj", "src/Merp.Registry.CommandStack/"]
COPY ["src/Merp.Domain/Merp.Domain.csproj", "src/Merp.Domain/"]
COPY ["src/Merp.Web/Merp.Web.csproj", "src/Merp.Web/"]
COPY ["src/Acl.RegistryResolutionServices/Acl.RegistryResolutionServices.csproj", "src/Acl.RegistryResolutionServices/"]
RUN dotnet restore "src/Merp.Registry.Web/Merp.Registry.Web.csproj"
COPY . .
WORKDIR "/src/src/Merp.Registry.Web"
RUN dotnet build "Merp.Registry.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Merp.Registry.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Merp.Registry.Web.dll"]