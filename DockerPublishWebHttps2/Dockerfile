#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#第一阶段：设置基础镜像，你的摩天大楼是从哪个地基起来的
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8010
EXPOSE 8011

#第二阶段：设置环境，你的大楼需要哪些设备：挖机，水泥泵，砖头，钢筋
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DockerPublishWebHttps2.csproj", "DockerPublishWebHttps2/"]
RUN dotnet restore "./DockerPublishWebHttps2/DockerPublishWebHttps2.csproj"
COPY . ./DockerPublishWebHttps2

#第三阶段：RUN dotnet build:实际上就是将代码编译成可执行的程序文件。
WORKDIR "/src/DockerPublishWebHttps2"
RUN dotnet build "DockerPublishWebHttps2.csproj" -c $BUILD_CONFIGURATION -o /app/build

#第四阶段:
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DockerPublishWebHttps2.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

#第五阶段
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DockerPublishWebHttps2.dll"]