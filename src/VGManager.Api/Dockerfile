# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:6.0 as build-env
WORKDIR /src
COPY ./VGApi.Api/VGApi.Api.csproj .
RUN dotnet restore "./VGApi.Api.csproj"
RUN cd ..
COPY ./VGService/VGService.csproj .
RUN dotnet restore "./VGService.csproj"
COPY . .
RUN dotnet build "./VGApi.Api/VGApi.Api.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /app/build
COPY --from=build-env /app/build .
EXPOSE 7210
ENV ASPNETCORE_URLS=http://+:7210
ENTRYPOINT ["dotnet", "VGApi.Api.dll"]