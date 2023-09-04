#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CustomerBase.Adapter.WebAPI/CustomerBase.Adapter.WebAPI.csproj", "CustomerBase.Adapter.WebAPI/"]
RUN dotnet restore "CustomerBase.Adapter.WebAPI/CustomerBase.Adapter.WebAPI.csproj"
COPY . .
WORKDIR "/src/CustomerBase.Adapter.WebAPI"
RUN dotnet build "CustomerBase.Adapter.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CustomerBase.Adapter.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerBase.Adapter.WebAPI.dll"]