FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS builder
WORKDIR /source
COPY ./ImageGallery.sln .

COPY ./ImageGallery/*.csproj ./ImageGallery/
RUN dotnet restore

COPY ./ImageGallery ./ImageGallery

RUN dotnet publish "./ImageGallery/ImageGallery.csproj" --output "../dist" --configuration Release --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=builder /source/dist .
EXPOSE 80
ENTRYPOINT ["dotnet", "ImageGallery.dll"]