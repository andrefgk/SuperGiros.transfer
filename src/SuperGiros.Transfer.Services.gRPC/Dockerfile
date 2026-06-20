# ---- STAGE 1: Build ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/SuperGiros.Transfer.Services.gRPC/SuperGiros.Transfer.Services.gRPC.csproj",               "src/SuperGiros.Transfer.Services.gRPC/"]
COPY ["src/SuperGiros.Transfer.Application.UseCases/SuperGiros.Transfer.Application.UseCases.csproj", "src/SuperGiros.Transfer.Application.UseCases/"]
COPY ["src/SuperGiros.Transfer.Application.Interfaces/SuperGiros.Transfer.Application.Interfaces.csproj", "src/SuperGiros.Transfer.Application.Interfaces/"]
COPY ["src/SuperGiros.Transfer.Domain/SuperGiros.Transfer.Domain.csproj",                             "src/SuperGiros.Transfer.Domain/"]
COPY ["src/SuperGiros.Transfer.Persistence/SuperGiros.Transfer.Persistence.csproj",                   "src/SuperGiros.Transfer.Persistence/"]
COPY ["src/SuperGiros.Transfer.Messaging/SuperGiros.Transfer.Messaging.csproj",                       "src/SuperGiros.Transfer.Messaging/"]
COPY ["src/SuperGiros.Transfer.Infrastructure/SuperGiros.Transfer.Infrastructure.csproj",             "src/SuperGiros.Transfer.Infrastructure/"]

RUN dotnet restore "src/SuperGiros.Transfer.Services.gRPC/SuperGiros.Transfer.Services.gRPC.csproj"

COPY . .
WORKDIR "/src/src/SuperGiros.Transfer.Services.gRPC"
RUN dotnet build "SuperGiros.Transfer.Services.gRPC.csproj" -c Release -o /app/build

# ---- STAGE 2: Publish ----
FROM build AS publish
RUN dotnet publish "SuperGiros.Transfer.Services.gRPC.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ---- STAGE 3: Runtime ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app


COPY --from=publish /app/publish .

EXPOSE 5220
EXPOSE 5221

ENTRYPOINT ["dotnet", "SuperGiros.Transfer.Services.gRPC.dll"]
