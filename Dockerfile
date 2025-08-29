# ===== build =====
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY TaskManagementSystem.TaskBoardService.sln ./
COPY TaskManagementSystem.TaskBoardService/TaskManagementSystem.TaskBoardService.csproj TaskManagementSystem.TaskBoardService/
COPY TaskManagementSystem.SharedLib/TaskManagementSystem.SharedLib.csproj TaskManagementSystem.SharedLib/
COPY TaskManagementSystem.GrpcLib/TaskManagementSystem.GrpcLib.csproj TaskManagementSystem.GrpcLib/

RUN dotnet restore TaskManagementSystem.TaskBoardService/TaskManagementSystem.TaskBoardService.csproj

COPY . ./

RUN dotnet publish TaskManagementSystem.TaskBoardService/TaskManagementSystem.TaskBoardService.csproj \
    -c Release -o /app/out --no-restore

# ===== runtime =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://0.0.0.0:8080 \
    ASPNETCORE_Kestrel__EndpointDefaults__Protocols=Http2

COPY --from=build /app/out ./

EXPOSE 8080
ENTRYPOINT ["dotnet", "TaskManagementSystem.TaskBoardService.dll"]
