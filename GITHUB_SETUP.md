# 📦 GitHub Setup Guide - Quick Start

## Step-by-Step Instructions

### 1. Create GitHub Account (if you don't have one)
- Go to [github.com](https://github.com) and sign up (it's free!)

### 2. Create New Repository on GitHub

1. Click the **"+"** icon in the top right corner
2. Select **"New repository"**
3. Fill in:
   - **Repository name**: `MLM-Generation-Plan` (or any name you like)
   - **Description**: "MLM Generation Plan - User Referral System with 3-Level Income Tracking"
   - **Visibility**: Choose **Public** (free) or **Private**
   - **DO NOT** check "Initialize with README" (we already have files)
4. Click **"Create repository"**

### 3. Upload Your Code to GitHub

Open Terminal in your project folder and run these commands:

```bash
# Navigate to your project
cd /Users/nandinirathod/Desktop/MLM

# Initialize git (if not already done)
git init

# Add all files
git add .

# Create your first commit
git commit -m "Initial commit: MLM Generation Plan application"

# Add GitHub repository (REPLACE YOUR_USERNAME with your actual GitHub username)
git remote add origin https://github.com/YOUR_USERNAME/MLM-Generation-Plan.git

# Rename branch to main
git branch -M main

# Push to GitHub
git push -u origin main
```

**Important:** Replace `YOUR_USERNAME` with your actual GitHub username!

### 4. Verify Upload

- Go to your GitHub repository page
- You should see all your project files
- Check that sensitive files (like `bin/`, `obj/`) are NOT visible (thanks to .gitignore)

### 5. Future Updates

When you make changes to your code:

```bash
# Add changed files
git add .

# Commit changes
git commit -m "Description of what you changed"

# Push to GitHub
git push
```

---

## 🔐 Authentication

If GitHub asks for authentication:

### Option 1: Personal Access Token (Recommended)
1. Go to GitHub → Settings → Developer settings → Personal access tokens → Tokens (classic)
2. Click "Generate new token"
3. Select scopes: `repo` (full control)
4. Copy the token
5. Use it as password when pushing

### Option 2: GitHub CLI
```bash
# Install GitHub CLI
brew install gh

# Login
gh auth login

# Then push normally
git push
```

---

## ✅ What Gets Uploaded

✅ **Will be uploaded:**
- Source code (.cs files)
- Views (.cshtml files)
- Configuration files (appsettings.json, Program.cs)
- Database scripts
- Documentation (README.md, etc.)
- .gitignore file

❌ **Will NOT be uploaded** (thanks to .gitignore):
- Compiled files (bin/, obj/)
- User-specific files
- Sensitive data
- Database files

---

## 🎯 Next Steps

After uploading to GitHub, you can:
1. Share your code with others
2. Deploy to free hosting (see DEPLOYMENT.md)
3. Set up CI/CD for automatic deployments
4. Collaborate with team members

---

## 💡 Tips

- **Commit often**: Make small, frequent commits with clear messages
- **Use branches**: Create branches for new features
- **Write good commit messages**: Describe what changed and why
- **Keep .gitignore updated**: Don't commit sensitive files

---

## 🆘 Troubleshooting

### "Repository not found" error
- Check your GitHub username is correct
- Verify repository name matches
- Ensure repository exists on GitHub

### "Authentication failed" error
- Use Personal Access Token instead of password
- Or use GitHub CLI for easier authentication

### "Large file" error
- Check .gitignore is working
- Remove large files from git: `git rm --cached large-file.zip`

---

## 📚 Learn More

- [GitHub Docs](https://docs.github.com)
- [Git Basics](https://git-scm.com/book)
- [.NET Gitignore](https://github.com/github/gitignore/blob/main/VisualStudio.gitignore)

