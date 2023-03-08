#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/DotNet.WebApi/DotNet.WebApi.csproj", "src/DotNet.WebApi/"]
COPY ["src/DotNet.Infrastructure/DotNet.Infrastructure.csproj", "src/DotNet.Infrastructure/"]
COPY ["src/DotNet.ApplicationCore/DotNet.ApplicationCore.csproj", "src/DotNet.ApplicationCore/"]
RUN dotnet restore "src/DotNet.WebApi/DotNet.WebApi.csproj"
COPY . .
WORKDIR "/src/src/DotNet.WebApi"
RUN dotnet build "DotNet.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DotNet.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotNet.WebApi.dll"]