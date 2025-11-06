# 🚀 Deployment Guide - MLM Generation Plan

This guide covers deploying your MLM application to free hosting platforms and uploading to GitHub.

## 📋 Table of Contents
1. [GitHub Setup](#github-setup)
2. [Free Deployment Options](#free-deployment-options)
3. [Azure App Service (Free Tier)](#azure-app-service-free-tier)
4. [Railway (Free Tier)](#railway-free-tier)
5. [Render (Free Tier)](#render-free-tier)
6. [Database Setup for Production](#database-setup-for-production)

---

## 🔵 GitHub Setup

### Step 1: Initialize Git Repository

```bash
# Navigate to project directory
cd /Users/nandinirathod/Desktop/MLM

# Initialize git repository
git init

# Add all files
git add .

# Create initial commit
git commit -m "Initial commit: MLM Generation Plan application"

# Add your GitHub repository (replace YOUR_USERNAME with your GitHub username)
git remote add origin https://github.com/YOUR_USERNAME/MLM-Generation-Plan.git

# Push to GitHub
git branch -M main
git push -u origin main
```

### Step 2: Create GitHub Repository

1. Go to [GitHub.com](https://github.com) and sign in
2. Click the **"+"** icon → **"New repository"**
3. Repository name: `MLM-Generation-Plan` (or your preferred name)
4. Description: "MLM Generation Plan - User Referral System with 3-Level Income Tracking"
5. Choose **Public** (for free) or **Private**
6. **DO NOT** initialize with README, .gitignore, or license (we already have these)
7. Click **"Create repository"**

### Step 3: Push Your Code

After creating the repository, GitHub will show you commands. Use these:

```bash
git remote add origin https://github.com/YOUR_USERNAME/MLM-Generation-Plan.git
git branch -M main
git push -u origin main
```

### Step 4: Verify Upload

- Go to your GitHub repository page
- You should see all your files uploaded
- Check that `.gitignore` is working (bin/, obj/ folders should not appear)

---

## 🌐 Free Deployment Options

### Option 1: Azure App Service (Free Tier) ⭐ RECOMMENDED

**Pros:**
- Free tier available (F1 - 1GB RAM)
- Easy deployment from GitHub
- Built-in CI/CD
- Supports .NET 8.0
- Free SSL certificate

**Limitations:**
- 1GB RAM
- 60 minutes compute time per day
- App sleeps after 20 minutes of inactivity

#### Setup Steps:

1. **Create Azure Account** (Free $200 credit for 30 days)
   - Go to [azure.microsoft.com](https://azure.microsoft.com/free/)
   - Sign up with your email

2. **Create App Service:**
   ```bash
   # Install Azure CLI (if not installed)
   # macOS: brew install azure-cli
   
   # Login to Azure
   az login
   
   # Create resource group
   az group create --name MLM-RG --location eastus
   
   # Create App Service Plan (Free tier)
   az appservice plan create --name MLM-Plan --resource-group MLM-RG --sku FREE
   
   # Create Web App
   az webapp create --resource-group MLM-RG --plan MLM-Plan --name mlm-generation-plan --runtime "DOTNET|8.0"
   ```

3. **Deploy from GitHub:**
   - Go to Azure Portal → Your App Service
   - Deployment Center → GitHub → Authorize
   - Select your repository and branch
   - Azure will auto-deploy on every push

4. **Configure Database Connection:**
   - Use Azure SQL Database (Free tier available) or
   - Use connection string from your existing SQL Server

---

### Option 2: Railway (Free Tier) ⭐ EASY SETUP

**Pros:**
- Very easy setup
- Free $5 credit monthly
- Auto-deploys from GitHub
- Supports .NET 8.0
- Free PostgreSQL database included

**Limitations:**
- Requires credit card (but free tier available)
- $5 credit per month (usually enough for small apps)

#### Setup Steps:

1. **Sign up at Railway:**
   - Go to [railway.app](https://railway.app)
   - Sign up with GitHub

2. **Create New Project:**
   - Click "New Project"
   - Select "Deploy from GitHub repo"
   - Choose your MLM repository

3. **Configure Build:**
   - Railway auto-detects .NET
   - Add build command: `dotnet publish -c Release -o ./publish`
   - Add start command: `dotnet ./publish/MLMApp.dll`

4. **Add Database:**
   - Click "New" → "Database" → "PostgreSQL"
   - Railway provides connection string automatically
   - Update your `appsettings.json` with the connection string

5. **Deploy:**
   - Railway automatically deploys
   - Get your app URL from the dashboard

---

### Option 3: Render (Free Tier)

**Pros:**
- Free tier available
- Auto-deploy from GitHub
- Free PostgreSQL database
- Easy setup

**Limitations:**
- App sleeps after 15 minutes of inactivity
- Slower cold starts

#### Setup Steps:

1. **Sign up at Render:**
   - Go to [render.com](https://render.com)
   - Sign up with GitHub

2. **Create New Web Service:**
   - Click "New" → "Web Service"
   - Connect your GitHub repository
   - Name: `mlm-generation-plan`
   - Environment: `.NET Core`
   - Build Command: `dotnet publish -c Release -o ./publish`
   - Start Command: `dotnet ./publish/MLMApp.dll`

3. **Add PostgreSQL Database:**
   - Click "New" → "PostgreSQL"
   - Copy the connection string
   - Add as environment variable in Web Service

4. **Deploy:**
   - Render auto-deploys
   - Get your app URL

---

## 🗄️ Database Setup for Production

### Option A: Azure SQL Database (Free Tier)

1. **Create Azure SQL Database:**
   ```bash
   az sql server create --name mlm-sql-server --resource-group MLM-RG --location eastus --admin-user sqladmin --admin-password YourStrong@Passw0rd123!
   
   az sql db create --resource-group MLM-RG --server mlm-sql-server --name MLMDb --service-objective Free
   ```

2. **Update Connection String:**
   - Get connection string from Azure Portal
   - Update in App Service → Configuration → Connection Strings

### Option B: Use Existing SQL Server

If you have SQL Server running:
- Update connection string in `appsettings.json` or environment variables
- Ensure SQL Server is accessible from the internet (for cloud hosting)

### Option C: PostgreSQL (Railway/Render)

If using Railway or Render with PostgreSQL:
- You'll need to update your code to use PostgreSQL instead of SQL Server
- Or use a SQL Server hosting service

---

## 🔧 Environment Variables Setup

For production, set these environment variables:

```bash
# Connection String
ConnectionStrings__DefaultConnection=your_production_connection_string

# Environment
ASPNETCORE_ENVIRONMENT=Production

# Allowed Hosts
ASPNETCORE_URLS=http://+:5000
```

---

## 📝 Production Checklist

- [ ] Update `appsettings.json` with production connection string
- [ ] Remove or secure sensitive data
- [ ] Set `ASPNETCORE_ENVIRONMENT=Production`
- [ ] Configure CORS if needed
- [ ] Set up SSL/HTTPS
- [ ] Configure logging
- [ ] Test deployment
- [ ] Verify database migrations
- [ ] Test user registration/login
- [ ] Test admin panel

---

## 🚀 Quick Deploy Commands

### For GitHub:

```bash
# Initial setup
git init
git add .
git commit -m "Initial commit"

# Add remote (replace YOUR_USERNAME)
git remote add origin https://github.com/YOUR_USERNAME/MLM-Generation-Plan.git
git branch -M main
git push -u origin main

# Future updates
git add .
git commit -m "Update: description of changes"
git push
```

### For Azure:

```bash
# Deploy using Azure CLI
az webapp deployment source config-zip --resource-group MLM-RG --name mlm-generation-plan --src publish.zip
```

---

## 📞 Support

If you encounter issues:
1. Check application logs in your hosting platform
2. Verify database connection string
3. Ensure .NET 8.0 runtime is available
4. Check environment variables

---

## 🎉 Success!

Once deployed, your application will be live at:
- Azure: `https://mlm-generation-plan.azurewebsites.net`
- Railway: `https://mlm-generation-plan.railway.app`
- Render: `https://mlm-generation-plan.onrender.com`

**Note:** Replace `mlm-generation-plan` with your actual app name.

