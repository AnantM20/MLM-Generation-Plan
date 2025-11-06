#!/bin/bash
# Quick SQL Server Setup Script for macOS

echo "🐳 Setting up SQL Server using Docker..."
echo "=========================================="
echo ""

# Check if Docker is installed
if ! command -v docker &> /dev/null; then
    echo "❌ Docker is not installed!"
    echo ""
    echo "Please install Docker Desktop from:"
    echo "https://www.docker.com/products/docker-desktop"
    echo ""
    exit 1
fi

echo "✅ Docker found"
echo ""

# Check if SQL Server container already exists
if docker ps -a | grep -q sqlserver; then
    echo "📦 SQL Server container found"
    if docker ps | grep -q sqlserver; then
        echo "✅ SQL Server container is already running"
    else
        echo "🚀 Starting SQL Server container..."
        docker start sqlserver
        sleep 5
    fi
else
    echo "📦 Creating new SQL Server container..."
    docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd123!" \
       -p 1433:1433 --name sqlserver \
       -d mcr.microsoft.com/mssql/server:2022-latest
    
    echo "⏳ Waiting for SQL Server to start (15 seconds)..."
    sleep 15
fi

echo ""
echo "🔍 Checking SQL Server connection..."
docker exec -it sqlserver /opt/mssql-tools/bin/sqlcmd \
   -S localhost -U sa -P "YourStrong@Passw0rd123!" \
   -Q "SELECT @@VERSION" 2>/dev/null

if [ $? -eq 0 ]; then
    echo "✅ SQL Server is running!"
else
    echo "❌ Failed to connect to SQL Server"
    echo "Waiting a bit more..."
    sleep 10
fi

echo ""
echo "📊 Creating database..."
docker exec -it sqlserver /opt/mssql-tools/bin/sqlcmd \
   -S localhost -U sa -P "YourStrong@Passw0rd123!" \
   -Q "IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'MLMDb') CREATE DATABASE MLMDb" 2>/dev/null

if [ $? -eq 0 ]; then
    echo "✅ Database MLMDb created (or already exists)"
else
    echo "⚠️  Database might already exist or there was an issue"
fi

echo ""
echo "📝 Running database setup script..."
if [ -f "Database/MLMDb.sql" ]; then
    docker cp Database/MLMDb.sql sqlserver:/tmp/MLMDb.sql
    
    docker exec -it sqlserver /opt/mssql-tools/bin/sqlcmd \
       -S localhost -U sa -P "YourStrong@Passw0rd123!" \
       -d MLMDb -i /tmp/MLMDb.sql 2>/dev/null
    
    if [ $? -eq 0 ]; then
        echo "✅ Database script executed successfully"
    else
        echo "⚠️  Database script might have errors or tables already exist"
    fi
else
    echo "⚠️  Database script not found at Database/MLMDb.sql"
fi

echo ""
echo "=========================================="
echo "✅ SQL Server setup complete!"
echo ""
echo "Connection details:"
echo "  Server: localhost,1433"
echo "  Database: MLMDb"
echo "  Username: sa"
echo "  Password: YourStrong@Passw0rd123!"
echo ""
echo "You can now run the application with:"
echo "  dotnet run"
echo ""

