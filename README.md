# MLM Generation Plan Web Application

A complete Multi-Level Marketing (MLM) Generation Plan web application built with ASP.NET Core MVC, C#, SQL Server, HTML, CSS, and Bootstrap.

## Features

### User Features
- **User Registration**: Register with Full Name, Email, Mobile Number, Password, and optional Sponsor ID
- **Auto-Generated User ID**: Unique User IDs (e.g., REG1001, REG1002) are automatically generated
- **Login System**: Secure authentication using Email and Password
- **Dashboard**: View comprehensive user statistics and generation tree
- **Generation Income Report**: 
  - Level 1: ₹100 per direct referral
  - Level 2: ₹50 per member
  - Level 3: ₹25 per member
- **Generation Tree Visualization**: Visual representation of the MLM hierarchy up to 3 levels
- **Team Statistics**: View total direct referrals, team members, and total income

### Admin Features
- **User Management**: View all registered users
- **User Status Control**: Activate/Deactivate users
- **Generation Tree Viewer**: View generation tree for any user
- **User Details**: View detailed information and statistics for any user

## Technical Stack

- **Backend**: ASP.NET Core MVC 8.0 (C#)
- **Frontend**: HTML5, CSS3, Bootstrap 5, jQuery
- **Database**: SQL Server (LocalDB)
- **Authentication**: Cookie-based authentication
- **Architecture**: MVC Pattern with Service Layer

## Project Structure

```
MLMApp/
├── Controllers/
│   ├── AccountController.cs      # Registration and Login
│   ├── DashboardController.cs    # User Dashboard
│   └── AdminController.cs        # Admin Panel
├── Models/
│   └── User.cs                   # User models and ViewModels
├── Services/
│   └── UserService.cs           # Business logic for user operations
├── Data/
│   └── ApplicationDbContext.cs   # Entity Framework DbContext
├── Views/
│   ├── Account/                 # Registration and Login views
│   ├── Dashboard/               # User dashboard view
│   ├── Admin/                   # Admin panel views
│   └── Shared/                  # Layout and shared views
├── wwwroot/
│   ├── css/
│   │   └── site.css            # Custom styles
│   └── js/
│       └── site.js             # Custom JavaScript
├── Database/
│   └── MLMDb.sql               # Database creation script
├── Program.cs                  # Application startup
└── appsettings.json           # Configuration
```

## Quick Start Guide

### Option 1: Using Visual Studio (Recommended)

1. Open the project folder in Visual Studio 2022
2. Execute the SQL script `Database/MLMDb.sql` in SQL Server Management Studio
3. Update connection string in `appsettings.json` if needed
4. Press `F5` to run the application
5. Login with admin credentials: `admin@mlm.com` / `Admin@123`

### Option 2: Using Command Line

```bash
# Restore packages
dotnet restore

# Run database script (using sqlcmd)
sqlcmd -S (localdb)\mssqllocaldb -i Database/MLMDb.sql

# Run the application
dotnet run
```

## Setup Instructions

### Prerequisites

1. **Visual Studio 2022** or **Visual Studio Code** with .NET 8.0 SDK
2. **SQL Server** (LocalDB recommended) or SQL Server Express
3. **Internet connection** (for NuGet package restoration)

### Installation Steps

#### 1. Database Setup

1. Open SQL Server Management Studio (SSMS) or use `sqlcmd`
2. Execute the SQL script located at `Database/MLMDb.sql`
   - This will create the database, tables, indexes, and sample data
   - Or run: `sqlcmd -S (localdb)\mssqllocaldb -i Database/MLMDb.sql`

#### 2. Update Connection String

If you're using a different SQL Server instance, update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=MLMDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

#### 3. Restore NuGet Packages

Open PowerShell or Command Prompt in the project directory and run:

```bash
dotnet restore
```

#### 4. Run Database Migrations (Optional)

If Entity Framework migrations are needed:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

#### 5. Run the Application

```bash
dotnet run
```

Or press `F5` in Visual Studio.

The application will start at `https://localhost:5001` or `http://localhost:5000`

## Default Credentials

### Admin Account
- **Email**: `admin@mlm.com`
- **Password**: `Admin@123`
- **User ID**: `REG1000`

### Sample User Account
- **Email**: `john.doe@example.com`
- **Password**: `Test@123`
- **User ID**: `REG1001`

**Note**: All sample users have the password: `Test@123`

## Sample Data

The database includes:
- 1 Admin user (REG1000)
- 1 Root user (REG1001)
- 3 Level 1 users (REG1002-1004)
- 5 Level 2 users (REG1005-1009)
- 6 Level 3 users (REG1010-1015)

## Generation Income Logic

The income calculation follows this structure:

- **Level 1 (Direct Referrals)**: ₹100 per member
- **Level 2**: ₹50 per member
- **Level 3**: ₹25 per member

**Example Calculation for REG1001:**
- 3 direct referrals (Level 1) = 3 × ₹100 = ₹300
- 5 members in Level 2 = 5 × ₹50 = ₹250
- 6 members in Level 3 = 6 × ₹25 = ₹150
- **Total Income**: ₹700

## Usage Guide

### For Users

1. **Registration**:
   - Navigate to Register page
   - Fill in all required fields
   - Optionally enter a Sponsor ID if you have one
   - Upon successful registration, you'll receive your unique User ID

2. **Login**:
   - Use your Email and Password to login
   - You'll be redirected to your Dashboard

3. **Dashboard**:
   - View your statistics (Direct Referrals, Team Members, Total Income)
   - View Generation Income Report by levels
   - Explore your Generation Tree visualization
   - View members at each level

### For Admins

1. **Login** as admin using admin credentials
2. **Admin Panel**:
   - View all registered users
   - View user details and statistics
   - View generation tree for any user
   - Activate/Deactivate users

## Key Features Implementation

### User ID Generation
- Auto-generates unique User IDs starting from REG1001
- Increments sequentially for each new registration
- Format: REG#### (e.g., REG1001, REG1002)

### Sponsor Validation
- Validates Sponsor ID exists in database
- Ensures sponsor is active before allowing registration
- Client-side format validation (REG#### pattern)
- Server-side existence and active status validation

### Generation Tree Calculation
- Uses recursive logic to traverse up to 3 levels
- Efficiently calculates team size and income
- Visual tree representation with JavaScript
- Supports both user dashboard and admin panel views

### Security
- Password hashing (Base64 for demo - use BCrypt in production)
- Cookie-based authentication with persistent sessions
- Role-based authorization (Admin/User)
- Server-side and client-side validation
- CSRF protection enabled
- Input sanitization and validation

### User Interface
- Modern, responsive Bootstrap 5 design
- Smooth animations and transitions
- Mobile-friendly responsive layout
- Custom CSS styling with gradient effects
- Font Awesome icons throughout
- Interactive generation tree visualization

## Important Notes

⚠️ **Security Warning**: This is a demo application. In production:
- Use proper password hashing (BCrypt, Argon2, etc.)
- Implement HTTPS
- Add CSRF protection (already included)
- Add rate limiting
- Implement proper logging and error handling
- Use secure session management

## Troubleshooting

### Database Connection Issues
- Ensure SQL Server is running
- Verify connection string in `appsettings.json`
- Check if LocalDB is installed: `sqllocaldb info MSSQLLocalDB`

### Package Restore Issues
- Run `dotnet restore` manually
- Clear NuGet cache: `dotnet nuget locals all --clear`

### Build Errors
- Ensure .NET 8.0 SDK is installed
- Verify all NuGet packages are restored
- Check for missing dependencies

## Development

### Adding New Features
- Follow MVC pattern
- Add business logic in Services layer
- Use dependency injection for services
- Maintain separation of concerns

### Database Changes
- Update `ApplicationDbContext.cs`
- Create migration: `dotnet ef migrations add MigrationName`
- Update database: `dotnet ef database update`

## 🚀 Deployment & GitHub

### Upload to GitHub

**Quick Setup:**
```bash
# Run the automated script
./push-to-github.sh

# Or manually:
git init
git add .
git commit -m "Initial commit"
git remote add origin https://github.com/YOUR_USERNAME/MLM-Generation-Plan.git
git branch -M main
git push -u origin main
```

**Detailed Instructions:** See [GITHUB_SETUP.md](GITHUB_SETUP.md)

### Free Deployment Options

This application can be deployed for FREE on:

1. **Azure App Service** (Free Tier)
   - 1GB RAM, 60 minutes compute/day
   - Auto-deploy from GitHub
   - See [DEPLOYMENT.md](DEPLOYMENT.md) for details

2. **Railway** (Free $5 credit/month)
   - Easy setup, auto-deploy from GitHub
   - Includes free PostgreSQL database
   - See [DEPLOYMENT.md](DEPLOYMENT.md) for details

3. **Render** (Free Tier)
   - Free tier available
   - Auto-deploy from GitHub
   - See [DEPLOYMENT.md](DEPLOYMENT.md) for details

**Full Deployment Guide:** See [DEPLOYMENT.md](DEPLOYMENT.md)

## License

This project is created for educational/demonstration purposes.

## Support

For issues or questions, please refer to the code comments or contact the development team.

---

**Built with ❤️ by Anant Mulay**

