# Step 1: Use the .NET SDK to build the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy the project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application and build it
COPY . ./
RUN dotnet publish -c Release -o /publish

# Step 2: Use the .NET runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

# Copy the published output, including static files (wwwroot)
COPY --from=build /publish .

# Expose the application port
EXPOSE 80

# Set the command to run the Blazor Server app
ENTRYPOINT ["dotnet", "ChatRoom.Client.dll"]
