#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Merp.Web.App/Merp.Web.App.csproj", "src/Merp.Web.App/"]
COPY ["src/Merp.Web/Merp.Web.csproj", "src/Merp.Web/"]
RUN dotnet restore "src/Merp.Web.App/Merp.Web.App.csproj"
COPY . .
WORKDIR "/src/src/Merp.Web.App"
RUN dotnet build "Merp.Web.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Merp.Web.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Merp.Web.App.dll"]