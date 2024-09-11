FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

RUN apk add --no-cache nodejs npm

WORKDIR /opt/cdn

COPY *.csproj ./
COPY frontend/package*.json frontend/

RUN dotnet restore

COPY . .

RUN dotnet build -c Release

RUN cd frontend && npm install && npm run build

ENTRYPOINT ["dotnet", "bin/Release/net8.0/CDNApp.dll"]

EXPOSE 8080

CMD ["dotnet", "run", "--no-build"]

