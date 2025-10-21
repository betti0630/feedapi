FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# FONTOS: ez kell, hogy ne [::]-on fusson
ENV ASPNETCORE_URLS=http://feed_apiservice:8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AttrectoTest.ApiService/AttrectoTest.ApiService.csproj", "AttrectoTest.ApiService/"]
RUN dotnet restore "AttrectoTest.ApiService/AttrectoTest.ApiService.csproj"
COPY . .
WORKDIR "/src/AttrectoTest.ApiService"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AttrectoTest.ApiService.dll"]

