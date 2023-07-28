FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["UCMS-api.csproj", "./"]
RUN dotnet restore "UCMS-api.csproj"
COPY . .
RUN dotnet build "UCMS-api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "UCMS-api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "UCMS-api.dll"]