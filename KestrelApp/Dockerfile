FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 2309
EXPOSE 44303

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY KestrelApp/KestrelApp.csproj KestrelApp/
RUN dotnet restore KestrelApp/KestrelApp.csproj
COPY . .
WORKDIR /src/KestrelApp
RUN dotnet build KestrelApp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish KestrelApp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "KestrelApp.dll"]
