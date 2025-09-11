FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AttrectoTest.Web/AttrectoTest.Web.csproj", "AttrectoTest.Web/"]
RUN dotnet restore "AttrectoTest.Web/AttrectoTest.Web.csproj"
COPY . .
WORKDIR "/src/AttrectoTest.Web"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AttrectoTest.Web.dll"]
