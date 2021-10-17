FROM mcr.microsoft.com/dotnet/sdk:3.1 as build
WORKDIR /build
COPY . .
RUN dotnet publish --configuration Release --output ./app

FROM mcr.microsoft.com/dotnet/sdk:3.1
ENV ASPNETCORE_URLS=http://+:8000
WORKDIR /app
COPY --from=build /build/app .
ENTRYPOINT [ "dotnet", "cumin-api.dll" ]