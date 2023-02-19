#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["BlazorAppPeliculasNET5/Server/BlazorAppPeliculasNET5.Server.csproj", "BlazorAppPeliculasNET5/Server/"]
COPY ["BlazorAppPeliculasNET5/Client/BlazorAppPeliculasNET5.Client.csproj", "BlazorAppPeliculasNET5/Client/"]
COPY ["BlazorAppPeliculasNET5/Shared/BlazorAppPeliculasNET5.Shared.csproj", "BlazorAppPeliculasNET5/Shared/"]
RUN dotnet restore "BlazorAppPeliculasNET5/Server/BlazorAppPeliculasNET5.Server.csproj"
COPY . .
WORKDIR "/src/BlazorAppPeliculasNET5/Server"
RUN dotnet build "BlazorAppPeliculasNET5.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorAppPeliculasNET5.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorAppPeliculasNET5.Server.dll"]