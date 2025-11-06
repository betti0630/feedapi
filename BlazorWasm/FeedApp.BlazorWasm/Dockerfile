FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Directory.Packages.props ./
COPY nuget.config ./

COPY ["FeedApp.BlazorWasm/FeedApp.BlazorWasm.csproj", "FeedApp.BlazorWasm/"]
COPY ["FeedApp.Blazor.Common/FeedApp.Blazor.Common.csproj" ,"FeedApp.Blazor.Common/"]

RUN dotnet restore "FeedApp.BlazorWasm/FeedApp.BlazorWasm.csproj"

COPY . .
WORKDIR "/src/FeedApp.BlazorWasm"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Serve with nginx
FROM nginx:alpine AS final
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot/appsettings.Docker.json /usr/share/nginx/html/appsettings.json
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
