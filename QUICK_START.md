# 🚀 Quick Start Guide - Running MLM Application

## ⚠️ Current Status

**Note:** .NET SDK is not currently installed on your system. Follow the steps below to install it and run the application.

---

## Step 1: Install .NET 8.0 SDK

### For macOS:

1. **Download .NET SDK 8.0:**
   - Visit: https://dotnet.microsoft.com/download/dotnet/8.0
   - Download the macOS installer (.pkg file)
   - Or use Homebrew:
     ```bash
     brew install --cask dotnet-sdk
     ```

2. **Verify Installation:**
   ```bash
   dotnet --version
   ```
   Should show: `8.0.x` or higher

3. **Verify SDK:**
   ```bash
   dotnet --list-sdks
   ```

---

## Step 2: Install SQL Server (if not already installed)

### Option A: SQL Server LocalDB (Recommended)
- Comes with Visual Studio for Mac (if installed)
- Or download SQL Server Express

### Option B: Use Docker (Easiest for Mac)
```bash
# Pull SQL Server image
docker pull mcr.microsoft.com/mssql/server:2022-latest

# Run SQL Server container
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" \
   -p 1433:1433 --name sqlserver \
   -d mcr.microsoft.com/mssql/server:2022-latest
```

Then update `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=MLMDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=true"
}
```

---

## Step 3: Setup Database

### Using Docker SQL Server:
```bash
# Connect to SQL Server
docker exec -it sqlserver /opt/mssql-tools/bin/sqlcmd \
   -S localhost -U sa -P "YourStrong@Passw0rd" \
   -Q "CREATE DATABASE MLMDb"
```

### Or use Azure Data Studio / SQL Server Management Studio:
- Download Azure Data Studio: https://aka.ms/azuredatastudio
- Connect to your SQL Server
- Execute `Database/MLMDb.sql` script

---

## Step 4: Run the Application

### Navigate to Project Folder:
```bash
cd /Users/nandinirathod/Desktop/MLM
```

### Restore NuGet Packages:
```bash
dotnet restore
```

### Build the Project:
```bash
dotnet build
```

### Run the Application:
```bash
dotnet run
```

### Expected Output:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

### Open in Browser:
- Navigate to: `https://localhost:5001` or `http://localhost:5000`
- Accept SSL certificate warning (development only)

---

## Step 5: Login

### Admin Account:
- **Email:** `admin@mlm.com`
- **Password:** `Admin@123`

### Test User Account:
- **Email:** `john.doe@example.com`
- **Password:** `Test@123`

---

## Troubleshooting

### If `dotnet` command not found:
1. Close and reopen your terminal
2. Check PATH: `echo $PATH`
3. Restart your Mac (sometimes needed after installation)

### If database connection fails:
1. Verify SQL Server is running
2. Check connection string in `appsettings.json`
3. Ensure database `MLMDb` exists
4. Try connection test:
   ```bash
   dotnet ef database update
   ```

### If port already in use:
```bash
# Find process using port 5001
lsof -i :5001

# Kill the process
kill -9 <PID>

# Or use different port
dotnet run --urls "http://localhost:8080"
```

---

## Alternative: Use Visual Studio Code

1. Install Visual Studio Code
2. Install C# extension
3. Open project folder in VS Code
4. Press `F5` to run and debug

---

## Quick Test Commands

```bash
# Check .NET installation
dotnet --version

# Check project structure
ls -la

# Restore packages
dotnet restore

# Build project
dotnet build

# Run application
dotnet run

# Run with watch (auto-reload on changes)
dotnet watch run
```

---

## ✅ Success Indicators

When everything is working, you should see:

1. ✅ `.NET SDK installed` → `dotnet --version` shows version
2. ✅ `Database created` → MLMDb database exists
3. ✅ `Packages restored` → No errors from `dotnet restore`
4. ✅ `Build successful` → No errors from `dotnet build`
5. ✅ `Application running` → Browser shows login page
6. ✅ `Login works` → Can login with admin credentials

---

## Need Help?

If you encounter issues:

1. Check the full `README.md` file
2. Review `MLM_Setup_Guide.html` for detailed instructions
3. Check troubleshooting section in the guide
4. Verify all prerequisites are installed

---

**Ready to run!** Follow the steps above to get your MLM application running. 🚀

