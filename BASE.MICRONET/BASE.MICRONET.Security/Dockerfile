
#FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim
WORKDIR /app

COPY ./publish/ /app
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT docker
EXPOSE 8080

ENTRYPOINT ["dotnet", "/app/BASE.MICRONET.Security.dll"]