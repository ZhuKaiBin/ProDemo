# 使用 ASP.NET Core 运行时镜像
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# 设置工作目录
WORKDIR /app

# 暴露应用程序的端口
EXPOSE 8080
EXPOSE 8081

# 将所有文件复制到容器中
COPY . .

# 设置应用的入口点
ENTRYPOINT ["dotnet", "WebApplication1.dll"]
