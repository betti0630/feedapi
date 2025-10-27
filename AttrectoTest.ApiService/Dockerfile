FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

EXPOSE 8080
# FONTOS: ez kell, hogy ne [::]-on fusson (IPv6 helyett IPv4)
ENV ASPNETCORE_URLS=http://feed_apiservice:8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Directory.Build.props ./
COPY Directory.Packages.props ./

COPY ["AttrectoTest.ApiService/AttrectoTest.ApiService.csproj", "AttrectoTest.ApiService/"]
COPY ["AttrectoTest.Common.Grpc/AttrectoTest.Common.Grpc.csproj", "AttrectoTest.Common.Grpc/"]
COPY ["AttrectoTest.Infrastructure/AttrectoTest.Infrastructure.csproj", "AttrectoTest.Infrastructure/"]
COPY ["AttrectoTest.Persistence/AttrectoTest.Persistence.csproj", "AttrectoTest.Persistence/"]
COPY ["AttrectoTest.Application/AttrectoTest.Application.csproj", "AttrectoTest.Application/"]
COPY ["AttrectoTest.Domain/AttrectoTest.Domain.csproj", "AttrectoTest.Domain/"]
COPY ["AttrectoTest.Application.Shared/AttrectoTest.Application.Shared.csproj", "AttrectoTest.Application.Shared/"]

RUN dotnet restore "AttrectoTest.ApiService/AttrectoTest.ApiService.csproj"

COPY . .
WORKDIR "/src/AttrectoTest.ApiService"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AttrectoTest.ApiService.dll"]

