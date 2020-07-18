FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS runtime
WORKDIR /app
EXPOSE 80


# copy and publish app and libraries
COPY . ./

RUN dotnet restore
RUN dotnet build --no-restore -c Release



RUN dotnet publish hacka-zeenvia.sln -c Release -o published


CMD ASPNETCORE_URLS=http://*:$PORT dotnet published/hacka-zeenvia.dll

