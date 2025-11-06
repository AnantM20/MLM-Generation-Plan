#!/bin/bash
# Quick run script for MLM Application

echo "🌳 MLM Generation Plan - Starting Application..."
echo "================================================"
echo ""

# Check if dotnet is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ ERROR: .NET SDK is not installed!"
    echo ""
    echo "Please install .NET SDK first:"
    echo "  brew install --cask dotnet-sdk"
    echo ""
    echo "Then close and reopen Terminal, and run this script again."
    exit 1
fi

echo "✅ .NET SDK found: $(dotnet --version)"
echo ""

# Navigate to project directory
cd "$(dirname "$0")"

echo "📦 Restoring NuGet packages..."
dotnet restore

if [ $? -ne 0 ]; then
    echo "❌ Failed to restore packages"
    exit 1
fi

echo "✅ Packages restored"
echo ""

echo "🔨 Building project..."
dotnet build

if [ $? -ne 0 ]; then
    echo "❌ Build failed"
    exit 1
fi

echo "✅ Build successful"
echo ""

echo "⚠️  IMPORTANT: Make sure SQL Server is running and database is set up!"
echo ""
echo "🚀 Starting application..."
echo "================================================"
echo "Application will be available at:"
echo "  - HTTPS: https://localhost:5001"
echo "  - HTTP:  http://localhost:5000"
echo ""
echo "Login credentials:"
echo "  Admin: admin@mlm.com / Admin@123"
echo "  User:  john.doe@example.com / Test@123"
echo ""
echo "Press Ctrl+C to stop the server"
echo "================================================"
echo ""

dotnet run

