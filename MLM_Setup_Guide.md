# MLM Generation Plan - Complete Setup and User Guide

**Version 1.0** | Built with ASP.NET Core MVC

---

## Table of Contents

1. [Overview](#overview)
2. [Prerequisites](#prerequisites)
3. [Setup Instructions](#setup-instructions)
4. [Database Setup](#database-setup)
5. [Running the Application](#running-the-application)
6. [Application Flow](#application-flow)
7. [Features Walkthrough](#features-walkthrough)
8. [Default Credentials](#default-credentials)
9. [Troubleshooting](#troubleshooting)

---

## 1. Overview

The MLM Generation Plan is a complete web application built with ASP.NET Core MVC that allows users to register, login, and view their generation tree with income reports up to 3 levels.

### Key Features:

- ✓ User Registration with auto-generated User IDs
- ✓ Secure Login System
- ✓ Dashboard with statistics and generation tree
- ✓ MLM Income Calculation (3 levels)
- ✓ Admin Panel for user management
- ✓ Responsive and modern UI

### Technology Stack:

| Component | Technology |
|-----------|-----------|
| Backend Framework | ASP.NET Core MVC 8.0 |
| Programming Language | C# |
| Database | SQL Server (LocalDB) |
| ORM | Entity Framework Core |
| Frontend | HTML5, CSS3, Bootstrap 5, jQuery |
| Authentication | Cookie-based Authentication |

---

## 2. Prerequisites

Before running the application, ensure you have the following installed:

### 2.1 Visual Studio 2022 (Recommended) or Visual Studio Code

- Download from: https://visualstudio.microsoft.com/
- Make sure to install ".NET desktop development" workload

### 2.2 .NET 8.0 SDK

- Download from: https://dotnet.microsoft.com/download
- Verify installation by running: `dotnet --version` in command prompt

### 2.3 SQL Server

- **Option A:** SQL Server LocalDB (Recommended - comes with Visual Studio)
- **Option B:** SQL Server Express (Free)
- **Option C:** SQL Server Developer Edition (Free)

### 2.4 SQL Server Management Studio (SSMS) (Optional but Recommended)

- Download from: Microsoft Download Center
- Used for executing SQL scripts and managing the database

⚠️ **Important:** Make sure all prerequisites are installed and working before proceeding to the setup steps.

---

## 3. Setup Instructions

### Step 1: Extract/Copy Project Files

Ensure all project files are in a folder (e.g., `C:\MLM` or `Desktop\MLM`)

### Step 2: Verify Project Structure

Verify that your project folder contains:

- `MLMApp.csproj` - Project file
- `Program.cs` - Application startup
- `appsettings.json` - Configuration
- `Controllers/` - Controllers folder
- `Models/` - Models folder
- `Views/` - Views folder
- `Services/` - Services folder
- `Data/` - Data access folder
- `Database/MLMDb.sql` - Database script
- `wwwroot/` - Static files folder

### Step 3: Restore NuGet Packages

Open Command Prompt or PowerShell in the project folder and run:

```bash
dotnet restore
```

This will download all required NuGet packages.

✓ **Success:** If packages restore successfully, you'll see "Restore succeeded" message.

---

## 4. Database Setup

### Method 1: Using SQL Server Management Studio (SSMS) - Recommended

#### Step 1: Open SQL Server Management Studio (SSMS)

#### Step 2: Connect to your SQL Server instance

- **Server Name:** `(localdb)\mssqllocaldb` (for LocalDB)
- Or use your SQL Server instance name
- **Authentication:** Windows Authentication

#### Step 3: Open the SQL script file

Open: `Database\MLMDb.sql`

#### Step 4: Execute the script

Click **Execute** button (F5) or press `Ctrl+E`

#### Step 5: Verify the execution

- Check the Messages tab for "Database setup completed successfully!"
- Verify database "MLMDb" is created in Object Explorer
- Verify "Users" table exists with sample data

### Method 2: Using Command Line (sqlcmd)

#### Step 1: Open Command Prompt or PowerShell

#### Step 2: Navigate to the project folder and run:

```bash
sqlcmd -S (localdb)\mssqllocaldb -i Database\MLMDb.sql
```

**Note:** If using a different SQL Server instance, replace `(localdb)\mssqllocaldb` with your server name.

ℹ️ **Database Connection:** The default connection string uses LocalDB. If you're using a different SQL Server instance, update the connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=MLMDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

✓ **Database Ready:** After successful execution, you should have:

- Database: MLMDb
- Table: Users
- 16 sample users (1 admin + 15 regular users)
- Complete 3-level generation hierarchy

---

## 5. Running the Application

### Method 1: Using Visual Studio (Recommended)

#### Step 1: Open Visual Studio 2022

#### Step 2: Open Project

Click **File → Open → Project/Solution**

#### Step 3: Select Project File

Navigate to the project folder and select `MLMApp.csproj`

#### Step 4: Wait for Loading

Wait for Visual Studio to restore packages and load the project

#### Step 5: Run Application

Press **F5** or click the **Run** button (green play icon)

#### Step 6: Browser Opens

The application will:

- Build the project
- Start the web server
- Open your default browser
- Navigate to the login page (usually `https://localhost:5001` or `http://localhost:5000`)

### Method 2: Using Command Line

#### Step 1: Open Command Prompt or PowerShell

#### Step 2: Navigate to the project folder:

```bash
cd C:\path\to\MLM
```

#### Step 3: Run the application:

```bash
dotnet run
```

#### Step 4: Look for output:

```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
      Now listening on: http://localhost:5000
```

#### Step 5: Open Browser

Open your browser and navigate to the URL shown in the output (usually `https://localhost:5001`)

ℹ️ **First Run:** On first run, you may see a security warning about the SSL certificate. Click "Advanced" and "Proceed to localhost" to continue.

✓ **Application Running:** You should now see the Login page in your browser!

---

## 6. Application Flow

### 6.1 User Registration Flow

#### Step 1: Navigate to Registration Page

- Click "Register" link on the login page
- Or navigate to: `/Account/Register`

#### Step 2: Fill Registration Form

- **Full Name:** Enter your full name (required)
- **Email:** Enter valid email address (required, must be unique)
- **Mobile Number:** Enter 10-15 digit mobile number (required, must be unique)
- **Password:** Enter password (minimum 6 characters, required)
- **Confirm Password:** Re-enter password to confirm (required)
- **Sponsor ID:** Enter sponsor's User ID (optional, format: REG####)

#### Step 3: Submit Registration

- Click "Register" button
- System validates all fields (client-side and server-side)
- If Sponsor ID provided, system validates it exists and is active
- System generates unique User ID (e.g., REG1001, REG1002)

#### Step 4: Registration Success

- Success message displayed with your User ID
- Redirected to Login page
- Save your User ID for future reference

### 6.2 Login Flow

#### Step 1: Navigate to Login Page

- Default page when application starts
- Or navigate to: `/Account/Login`

#### Step 2: Enter Credentials

- **Email:** Enter your registered email
- **Password:** Enter your password
- **Remember Me:** (Optional) Check to stay logged in

#### Step 3: Click Login

- System validates credentials
- Checks if account is active
- Creates authentication session

#### Step 4: Redirect to Dashboard

- On successful login, redirected to Dashboard
- If admin, Admin Panel link appears in navigation

### 6.3 Dashboard Flow

#### Step 1: View User Information

- User Name and User ID displayed at top
- Sponsor ID shown (if any)

#### Step 2: View Statistics Cards

- **Direct Referrals:** Count of direct referrals (Level 1)
- **Total Team Members:** Total members in all 3 levels
- **Total Income:** Calculated income from all levels
- **Sponsor:** Your sponsor's User ID

#### Step 3: View Generation Income Report

- Table showing income breakdown by level
- Level 1: ₹100 per member
- Level 2: ₹50 per member
- Level 3: ₹25 per member
- Grand Total displayed

#### Step 4: View Generation Tree

- Visual tree representation
- Shows hierarchy up to 3 levels
- Interactive and expandable

#### Step 5: View Level-wise Members

- Cards showing members at each level
- Member name, User ID, and income per member

### 6.4 Admin Panel Flow

#### Step 1: Access Admin Panel

- Login as admin user
- Click "Admin Panel" link in navigation
- Or navigate to: `/Admin`

#### Step 2: View All Users

- Table showing all registered users
- User ID, Name, Email, Mobile, Sponsor ID
- Registration Date and Status

#### Step 3: View User Details

- Click "View Details" (eye icon) for any user
- See complete user information
- View user's statistics and income report

#### Step 4: View Generation Tree

- Click "View Tree" (sitemap icon) for any user
- See complete generation tree for that user
- Shows up to 3 levels

#### Step 5: Activate/Deactivate Users

- Click "Deactivate" (ban icon) to deactivate a user
- Click "Activate" (check icon) to activate a user
- Confirmation dialog appears before action

---

## 7. Features Walkthrough

### 7.1 Income Calculation Logic

The MLM income calculation follows this structure:

| Level | Description | Income Per Member |
|-------|-------------|-------------------|
| Level 1 | Direct Referrals | ₹100 |
| Level 2 | Referrals of Level 1 members | ₹50 |
| Level 3 | Referrals of Level 2 members | ₹25 |

**Example Calculation:**

If user REG1001 has:

- 3 direct referrals (Level 1): 3 × ₹100 = ₹300
- 5 members in Level 2: 5 × ₹50 = ₹250
- 6 members in Level 3: 6 × ₹25 = ₹150
- **Total Income: ₹700**

### 7.2 User ID Generation

- Auto-generated starting from REG1001
- Increments sequentially (REG1002, REG1003, etc.)
- Format: REG followed by 4-digit number
- Unique and cannot be duplicated

### 7.3 Sponsor Validation

- Sponsor ID must be in format: REG####
- Sponsor must exist in database
- Sponsor must be active
- Validation happens both client-side and server-side

### 7.4 Security Features

- Password hashing (Base64 encoding)
- Cookie-based authentication
- CSRF protection
- Role-based authorization
- Input validation and sanitization

---

## 8. Default Credentials

### Admin Account

| Field | Value |
|-------|-------|
| Email | `admin@mlm.com` |
| Password | `Admin@123` |
| User ID | `REG1000` |
| Role | Admin |

### Sample User Accounts

| User ID | Email | Password | Sponsor ID |
|---------|-------|----------|------------|
| REG1001 | john.doe@example.com | Test@123 | None (Root) |
| REG1002 | alice.smith@example.com | Test@123 | REG1001 |
| REG1003 | bob.johnson@example.com | Test@123 | REG1001 |
| REG1004 | carol.williams@example.com | Test@123 | REG1001 |
| ... | ... | Test@123 | ... |

ℹ️ **Note:** All sample users (except admin) use the password: `Test@123`

---

## 9. Troubleshooting

### Problem: Database Connection Error

**Solution:**

- Verify SQL Server is running
- Check connection string in `appsettings.json`
- For LocalDB, verify it's installed: `sqllocaldb info MSSQLLocalDB`
- Ensure database MLMDb exists and was created successfully

### Problem: "dotnet" command not found

**Solution:**

- Install .NET 8.0 SDK from Microsoft's website
- Restart your terminal/command prompt after installation
- Verify installation: `dotnet --version`

### Problem: NuGet Package Restore Fails

**Solution:**

- Check internet connection
- Clear NuGet cache: `dotnet nuget locals all --clear`
- Try restore again: `dotnet restore`
- Check firewall settings

### Problem: Build Errors

**Solution:**

- Ensure .NET 8.0 SDK is installed
- Verify all NuGet packages are restored
- Clean solution: `dotnet clean`
- Rebuild: `dotnet build`
- Check for missing dependencies

### Problem: SSL Certificate Warning

**Solution:**

- This is normal for development
- Click "Advanced" in browser
- Click "Proceed to localhost"
- Or use HTTP instead of HTTPS: `http://localhost:5000`

### Problem: Foreign Key Constraint Error

**Solution:**

- Ensure database script executed completely
- Verify foreign key constraint exists: `FK_Users_Sponsor`
- Check that UserId column has unique constraint
- Re-run database script if needed

### Problem: Application Won't Start

**Solution:**

- Check if port 5000/5001 is already in use
- Close other applications using those ports
- Check application logs for errors
- Verify database connection string
- Ensure database exists and is accessible

---

## 10. Additional Resources

### Project Files

- **README.md** - Complete project documentation
- **PROJECT_SUMMARY.md** - Project completion summary
- **Database/MLMDb.sql** - Database creation script

### Useful Commands

```bash
# Restore packages
dotnet restore

# Build project
dotnet build

# Run application
dotnet run

# Clean solution
dotnet clean

# Watch for changes (auto-reload)
dotnet watch run
```

### Ports Used

- **HTTPS:** 5001 (default)
- **HTTP:** 5000 (default)

ℹ️ **Changing Ports:** To use different ports, edit `Properties/launchSettings.json` or use command line: `dotnet run --urls "http://localhost:8080"`

---

## 🎉 Ready to Use!

Your MLM Generation Plan application is now ready to use. Follow the steps above to get started!

**Built with ❤️ using ASP.NET Core MVC**

Version 1.0 | 2024

---

## Quick Reference Card

### Common URLs

- Login Page: `/Account/Login`
- Registration: `/Account/Register`
- Dashboard: `/Dashboard`
- Admin Panel: `/Admin`

### Default Admin Login

- Email: `admin@mlm.com`
- Password: `Admin@123`

### Database Name

- `MLMDb`

### Default Ports

- HTTPS: `5001`
- HTTP: `5000`

---

*End of Document*

