#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MedOnTime-WebApp/MedOnTime-WebApp.csproj", "MedOnTime-WebApp/"]
RUN dotnet restore "MedOnTime-WebApp/MedOnTime-WebApp.csproj"
COPY . .
WORKDIR "/src/MedOnTime-WebApp"
RUN dotnet build "MedOnTime-WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MedOnTime-WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MedOnTime-WebApp.dll"]