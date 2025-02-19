#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8070
EXPOSE 8071


# 将所有文件复制到容器中
COPY . .

# 假设证书文件名为 developer-cert.pfx

#COPY ["developer-cert.pfx", "/app/certs/developer-cert.pfx"]
#ENV ASPNETCORE_Kestrel__Certificates__Default__Path="/app/certs/developer-cert.pfx"
#ENV ASPNETCORE_Kestrel__Certificates__Default__Password="prozkb123456789"


ENTRYPOINT ["dotnet", "dockerPublish.dll"]