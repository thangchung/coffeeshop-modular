#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/DataGen/DataGen.csproj", "src/DataGen/"]
RUN dotnet restore "src/DataGen/DataGen.csproj"
COPY . .
WORKDIR "/src/src/DataGen"
RUN dotnet build "DataGen.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataGen.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataGen.dll"]