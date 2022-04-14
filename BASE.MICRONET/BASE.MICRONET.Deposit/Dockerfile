ARG PROJECT_NAME=BASE.MICRONET.Deposit

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
ARG PROJECT_NAME
ENV PROJECT=$PROJECT_NAME
WORKDIR /app
COPY . .
RUN dotnet publish ${PROJECT}.csproj -c release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim
ARG PROJECT_NAME
ENV PROJECT=$PROJECT_NAME
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_ENVIRONMENT docker
EXPOSE 80
ENTRYPOINT dotnet ${PROJECT}.dll