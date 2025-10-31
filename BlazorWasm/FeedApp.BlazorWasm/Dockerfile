FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Directory.Packages.props ./

COPY ["AttrectoTest.BlazorWasm/AttrectoTest.BlazorWasm.csproj", "AttrectoTest.BlazorWasm/"]
COPY ["AttrectoTest.Blazor.Shared/AttrectoTest.Blazor.Common.csproj" ,"AttrectoTest.Blazor.Shared/"]

RUN dotnet restore "AttrectoTest.BlazorWasm/AttrectoTest.BlazorWasm.csproj"

COPY . .
WORKDIR "/src/AttrectoTest.BlazorWasm"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Serve with nginx
FROM nginx:alpine AS final
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
