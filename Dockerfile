FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar os arquivos de projeto para restaurar dependências
COPY ["ContactZone.Domain/ContactZone.Domain.csproj", "ContactZone.Domain/"]
COPY ["ContactZone.Application/ContactZone.Application.csproj", "ContactZone.Application/"]
COPY ["ContactZone.Infrastructure/ContactZone.Infrastructure.csproj", "ContactZone.Infrastructure/"]
COPY ["ContactZone.Functions/GetContacts/GetContacts.csproj", "ContactZone.Functions/GetContacts/"]

# Restaurar as dependências
RUN dotnet restore "ContactZone.Functions/GetContacts/GetContacts.csproj"

# Copiar o resto do código fonte
COPY ["ContactZone.Domain/", "ContactZone.Domain/"]
COPY ["ContactZone.Application/", "ContactZone.Application/"]
COPY ["ContactZone.Infrastructure/", "ContactZone.Infrastructure/"]
COPY ["ContactZone.Functions/GetContacts/", "ContactZone.Functions/GetContacts/"]

# Publicar a aplicação
WORKDIR "/src/ContactZone.Functions/GetContacts"
RUN dotnet publish "GetContacts.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build da imagem final
FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0
WORKDIR /home/site/wwwroot
COPY --from=build /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true
