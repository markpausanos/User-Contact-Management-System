# Use the official .NET 6 SDK image as the build environment.
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory inside the container.
WORKDIR /app

# Copy the project file(s) to the container.
COPY UCMS-api.csproj .

# Restore the NuGet packages.
RUN dotnet restore

# Copy the entire project files to the container.
# This step can be optimized using a .dockerignore file to exclude unnecessary files.
COPY . .

# Build the application.
RUN dotnet build -c Release --no-restore

# Publish the application as a self-contained app.
RUN dotnet publish -c Release -o out --no-restore -r linux-x64

# Create a runtime image using the .NET 6 runtime image.
FROM mcr.microsoft.com/dotnet/runtime:6.0

# Set the working directory inside the container.
WORKDIR /app

# Copy the published files from the build image to the runtime image.
COPY --from=build /app/out .

# Set the entry point for the application.
ENTRYPOINT ["./UCMS-api"]
