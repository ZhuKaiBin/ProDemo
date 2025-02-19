##See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER app
#WORKDIR /app
#EXPOSE 8060
#EXPOSE 8061
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["BasicSolution/DockerPublishWebHttps/DockerPublishWebHttps.csproj", "BasicSolution/DockerPublishWebHttps/"]
#RUN dotnet restore "./BasicSolution/DockerPublishWebHttps/DockerPublishWebHttps.csproj"
#COPY . .
#WORKDIR "/src/BasicSolution/DockerPublishWebHttps"
#RUN dotnet build "./DockerPublishWebHttps.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./DockerPublishWebHttps.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "DockerPublishWebHttps.dll"]


#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8060
EXPOSE 8061


# 将所有文件复制到容器中
COPY . .

# 假设证书文件名为 developer-cert.pfx

#COPY ["developer-cert.pfx", "/app/certs/developer-cert.pfx"]
#ENV ASPNETCORE_Kestrel__Certificates__Default__Path="/app/certs/developer-cert.pfx"
#ENV ASPNETCORE_Kestrel__Certificates__Default__Password="prozkb123456789"


ENTRYPOINT ["dotnet", "DockerPublishWebHttps.dll"]