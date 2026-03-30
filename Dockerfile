FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# On copie le projet et on restaure les dépendances (NuGet)
COPY ["NothingBot.csproj", "./"]
RUN dotnet restore "NothingBot.csproj"

# On copie le reste du code et on compile
COPY . .
RUN dotnet publish "NothingBot.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Étape finale (Runtime uniquement, beaucoup plus léger)
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Commande de lancement (adapte le nom du .dll si besoin)
ENTRYPOINT ["dotnet", "NothingBot.dll"]