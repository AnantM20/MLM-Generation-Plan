# Quick SQL Server Setup for macOS

## Option 1: Using Docker (Recommended)

### Step 1: Install Docker Desktop
- Download from: https://www.docker.com/products/docker-desktop
- Install and start Docker Desktop

### Step 2: Run SQL Server Container

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd123!" \
   -p 1433:1433 --name sqlserver \
   -d mcr.microsoft.com/mssql/server:2022-latest
```

### Step 3: Wait for SQL Server to Start
```bash
sleep 10
```

### Step 4: Create Database
```bash
docker exec -it sqlserver /opt/mssql-tools/bin/sqlcmd \
   -S localhost -U sa -P "YourStrong@Passw0rd123!" \
   -Q "CREATE DATABASE MLMDb"
```

### Step 5: Update appsettings.json
Change connection string to:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=MLMDb;User Id=sa;Password=YourStrong@Passw0rd123!;TrustServerCertificate=true"
}
```

### Step 6: Run SQL Script
```bash
# Copy SQL script into container
docker cp Database/MLMDb.sql sqlserver:/tmp/MLMDb.sql

# Execute script
docker exec -it sqlserver /opt/mssql-tools/bin/sqlcmd \
   -S localhost -U sa -P "YourStrong@Passw0rd123!" \
   -d MLMDb -i /tmp/MLMDb.sql
```

## Option 2: Use SQL Server Express for Mac
- Download from Microsoft
- Install and configure
- Update connection string accordingly

