#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DevPath.ImageUploadS3/DevPath.ImageUploadS3.csproj", "DevPath.ImageUploadS3/"]
RUN dotnet restore "DevPath.ImageUploadS3/DevPath.ImageUploadS3.csproj"
COPY . .
WORKDIR "/src/DevPath.ImageUploadS3"
RUN dotnet build "DevPath.ImageUploadS3.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DevPath.ImageUploadS3.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevPath.ImageUploadS3.dll"]