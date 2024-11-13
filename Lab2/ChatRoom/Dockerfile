
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ChatRoom.sln . 
COPY ChatRoom/ChatRoom.csproj ChatRoom/ 


RUN dotnet restore ChatRoom/ChatRoom.csproj


COPY . .
RUN dotnet publish ChatRoom/ChatRoom.csproj -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ChatRoom.dll"]