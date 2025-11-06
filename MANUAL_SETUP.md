# 🚀 MLM Application - Manual Setup Instructions

## ⚠️ Important: You need to install .NET SDK first!

Since the automated installation requires your password, please follow these steps:

---

## Step 1: Install .NET SDK

### Option A: Using Homebrew (Recommended)

Open Terminal and run:

```bash
brew install --cask dotnet-sdk
```

Enter your password when prompted.

### Option B: Download from Microsoft

1. Visit: https://dotnet.microsoft.com/download/dotnet/8.0
2. Download the macOS installer (.pkg)
3. Run the installer
4. Follow the installation wizard

### Verify Installation:

After installation, close and reopen Terminal, then run:

```bash
dotnet --version
```

You should see: `8.0.x` or `9.0.x`

---

## Step 2: Setup Database

### Option A: Use Docker (Easiest for Mac)

```bash
# Pull SQL Server image
docker pull mcr.microsoft.com/mssql/server:2022-latest

# Run SQL Server container
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" \
   -p 1433:1433 --name sqlserver \
   -d mcr.microsoft.com/mssql/server:2022-latest

# Wait 10 seconds for SQL Server to start
sleep 10

# Create database
docker exec -it sqlserver /opt/mssql-tools/bin/sqlcmd \
   -S localhost -U sa -P "YourStrong@Passw0rd" \
   -Q "CREATE DATABASE MLMDb"
```

Then update `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=MLMDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=true"
  }
}
```

### Option B: Use SQL Server Express

1. Download SQL Server Express for Mac
2. Install and configure
3. Update connection string in `appsettings.json`
4. Run the SQL script: `Database/MLMDb.sql`

---

## Step 3: Run the Application

### Navigate to Project Folder:

```bash
cd /Users/nandinirathod/Desktop/MLM
```

### Restore Packages:

```bash
dotnet restore
```

### Build Project:

```bash
dotnet build
```

### Run Application:

```bash
dotnet run
```

You should see output like:

```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
      Now listening on: http://localhost:5000
```

### Open in Browser:

- **HTTPS:** https://localhost:5001
- **HTTP:** http://localhost:5000

**Note:** You may see a security warning - click "Advanced" → "Proceed to localhost" (this is normal for development)

---

## Step 4: Login

### Admin Account:
- Email: `admin@mlm.com`
- Password: `Admin@123`

### Test User:
- Email: `john.doe@example.com`
- Password: `Test@123`

---

## Quick Commands Reference

```bash
# Check .NET version
dotnet --version

# Restore packages
dotnet restore

# Build project
dotnet build

# Run application
dotnet run

# Run with auto-reload (recommended for development)
dotnet watch run

# Clean build artifacts
dotnet clean
```

---

## Troubleshooting

### If "dotnet: command not found":
1. Close Terminal completely
2. Reopen Terminal
3. Run: `dotnet --version`
4. If still not found, restart your Mac

### If database connection fails:
1. Check if SQL Server is running
2. Verify connection string in `appsettings.json`
3. Ensure database `MLMDb` exists
4. Check firewall settings

### If port 5000/5001 is in use:
```bash
# Find process using port
lsof -i :5001

# Kill the process
kill -9 <PID>

# Or use different port
dotnet run --urls "http://localhost:8080"
```

---

## Using the Setup Script

After installing .NET SDK, you can use the automated setup script:

```bash
cd /Users/nandinirathod/Desktop/MLM
./setup.sh
```

Or manually:

```bash
cd /Users/nandinirathod/Desktop/MLM
dotnet restore
dotnet build
dotnet run
```

---

## ✅ Success Checklist

- [ ] .NET SDK installed (`dotnet --version` works)
- [ ] SQL Server running
- [ ] Database MLMDb created
- [ ] Connection string configured
- [ ] Packages restored (`dotnet restore` succeeds)
- [ ] Project builds (`dotnet build` succeeds)
- [ ] Application runs (`dotnet run` starts server)
- [ ] Browser opens login page
- [ ] Can login with admin credentials

---

## Need Help?

1. Check `README.md` for detailed documentation
2. Review `MLM_Setup_Guide.html` for complete guide
3. See troubleshooting section above

**Ready to run!** 🚀

