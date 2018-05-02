FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 15656
EXPOSE 44355

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Custom/Custom.csproj Custom/
RUN dotnet restore Custom/Custom.csproj
COPY . .
WORKDIR /src/Custom
RUN dotnet build Custom.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Custom.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Custom.dll"]
