FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

EXPOSE 8080
# FONTOS: ez kell, hogy ne [::]-on fusson (IPv6 helyett IPv4)
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Directory.Build.props ./
COPY Directory.Packages.props ./

COPY ["AttrectoTest.BlazorServer/AttrectoTest.BlazorServer.csproj", "AttrectoTest.BlazorServer/"]
COPY ["AttrectoTest.Application/AttrectoTest.Application.csproj", "AttrectoTest.Application/"]
COPY ["AttrectoTest.Blazor.Shared/AttrectoTest.Blazor.Common.csproj", "AttrectoTest.Blazor.Shared/"]
COPY ["AttrectoTest.Common.Grpc/AttrectoTest.Common.Grpc.csproj", "AttrectoTest.Common.Grpc/"]
COPY ["AttrectoTest.Infrastructure/AttrectoTest.Infrastructure.csproj", "AttrectoTest.Infrastructure/"]
COPY ["AttrectoTest.Persistence/AttrectoTest.Persistence.csproj", "AttrectoTest.Persistence/"]


RUN dotnet restore "AttrectoTest.BlazorServer/AttrectoTest.BlazorServer.csproj"

COPY . .
WORKDIR "/src/AttrectoTest.BlazorServer"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AttrectoTest.BlazorServer.dll"]

