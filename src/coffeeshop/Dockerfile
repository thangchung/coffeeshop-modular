#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
# https://www.docker.com/blog/9-tips-for-containerizing-your-net-application/ => Optimize for Startup Performance

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
HEALTHCHECK CMD curl --fail http://localhost || exit 1

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/coffeeshop/CoffeeShop.csproj", "src/coffeeshop/"]
COPY ["src/Libs/N8T.Core/N8T.Core.csproj", "src/Libs/N8T.Core/"]
COPY ["src/Libs/N8T.Infrastructure.EfCore/N8T.Infrastructure.EfCore.csproj", "src/Libs/N8T.Infrastructure.EfCore/"]
COPY ["src/Libs/N8T.Infrastructure/N8T.Infrastructure.csproj", "src/Libs/N8T.Infrastructure/"]
RUN dotnet restore "src/coffeeshop/CoffeeShop.csproj"
COPY . .
WORKDIR "/src/src/coffeeshop"
RUN dotnet build "CoffeeShop.csproj" -c Release -o /app/build -r linux-x64 /p:PublishReadyToRun=true

FROM build AS publish
RUN dotnet publish "CoffeeShop.csproj" -c Release -o /app/publish -r linux-x64 --self-contained true --no-restore /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishSingleFile=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

CMD ["./CoffeeShop"]