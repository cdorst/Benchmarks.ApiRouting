FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 15647
EXPOSE 44387

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Default/Default.csproj Default/
RUN dotnet restore Default/Default.csproj
COPY . .
WORKDIR /src/Default
RUN dotnet build Default.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Default.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Default.dll"]
