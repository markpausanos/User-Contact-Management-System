# Use the official .NET 6 SDK image as the base image.
# You can choose a specific version tag if needed.
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory inside the container.
WORKDIR /app

COPY UCMS-api.csproj .

# Restore the dependencies.
RUN dotnet restore

# Copy the entire project files to the container.
# This step can be optimized using a .dockerignore file to exclude unnecessary files.
COPY . .

# Build the application.
RUN dotnet build -c Release --no-restore

# Publish the application.
RUN dotnet publish -c Release -o out --no-restore

# Create a runtime image using the .NET 6 runtime image.
FROM mcr.microsoft.com/dotnet/runtime:6.0

# Set the working directory inside the container.
WORKDIR /app

# Copy the published files from the build image to the runtime image.
COPY --from=build /app/out .

# Set the entry point for the application.
ENTRYPOINT ["dotnet", "UCMS-api.dll"]
