#!/bin/bash
# MLM Application Setup Script
# Run this script to install prerequisites and run the application

echo "🌳 MLM Generation Plan - Setup Script"
echo "========================================"
echo ""

# Check if dotnet is installed
if command -v dotnet &> /dev/null; then
    echo "✅ .NET SDK is already installed"
    dotnet --version
else
    echo "❌ .NET SDK is not installed"
    echo ""
    echo "📦 Installing .NET SDK..."
    echo "Please enter your password when prompted:"
    brew install --cask dotnet-sdk
    
    if [ $? -eq 0 ]; then
        echo "✅ .NET SDK installed successfully"
        # Reload shell environment
        export PATH="/usr/local/share/dotnet:$PATH"
    else
        echo "❌ Failed to install .NET SDK"
        echo "Please install manually: brew install --cask dotnet-sdk"
        exit 1
    fi
fi

echo ""
echo "🔍 Checking project structure..."
if [ ! -f "MLMApp.csproj" ]; then
    echo "❌ Error: MLMApp.csproj not found"
    echo "Please run this script from the project root directory"
    exit 1
fi

echo "✅ Project structure looks good"
echo ""

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

echo "⚠️  Database Setup Required:"
echo "Before running the application, make sure you have:"
echo "1. SQL Server installed and running"
echo "2. Database MLMDb created (run Database/MLMDb.sql)"
echo "3. Connection string configured in appsettings.json"
echo ""

read -p "Do you want to run the application now? (y/n) " -n 1 -r
echo ""
if [[ $REPLY =~ ^[Yy]$ ]]; then
    echo ""
    echo "🚀 Starting application..."
    echo "The application will open at: http://localhost:5000 or https://localhost:5001"
    echo "Press Ctrl+C to stop the server"
    echo ""
    dotnet run
else
    echo "Setup complete! Run 'dotnet run' when ready to start the application."
fi

