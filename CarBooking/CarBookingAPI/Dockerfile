#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CarBookingAPI/CarBookingAPI.csproj", "CarBookingAPI/"]
RUN dotnet restore "CarBookingAPI/CarBookingAPI.csproj"
COPY . .
WORKDIR "/src/CarBookingAPI"
RUN dotnet build "CarBookingAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarBookingAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarBookingAPI.dll"]