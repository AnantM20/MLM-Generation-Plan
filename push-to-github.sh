#!/bin/bash
# Quick script to push code to GitHub

echo "🚀 MLM Generation Plan - GitHub Upload Script"
echo "=============================================="
echo ""

# Check if git is installed
if ! command -v git &> /dev/null; then
    echo "❌ Git is not installed!"
    echo "Please install Git first:"
    echo "  macOS: brew install git"
    echo "  Or download from: https://git-scm.com/downloads"
    exit 1
fi

echo "✅ Git found: $(git --version)"
echo ""

# Get GitHub username
read -p "Enter your GitHub username: " GITHUB_USERNAME

if [ -z "$GITHUB_USERNAME" ]; then
    echo "❌ GitHub username is required!"
    exit 1
fi

# Get repository name
read -p "Enter repository name (default: MLM-Generation-Plan): " REPO_NAME
REPO_NAME=${REPO_NAME:-MLM-Generation-Plan}

echo ""
echo "📦 Repository: https://github.com/$GITHUB_USERNAME/$REPO_NAME"
echo ""

# Check if repository already exists
read -p "Have you created this repository on GitHub? (y/n): " REPO_EXISTS

if [ "$REPO_EXISTS" != "y" ] && [ "$REPO_EXISTS" != "Y" ]; then
    echo ""
    echo "📝 Please create the repository on GitHub first:"
    echo "   1. Go to https://github.com/new"
    echo "   2. Repository name: $REPO_NAME"
    echo "   3. Choose Public or Private"
    echo "   4. DO NOT initialize with README"
    echo "   5. Click 'Create repository'"
    echo ""
    read -p "Press Enter after creating the repository..."
fi

# Initialize git if not already done
if [ ! -d ".git" ]; then
    echo "🔧 Initializing git repository..."
    git init
fi

# Add all files
echo "📝 Adding files to git..."
git add .

# Check if there are changes to commit
if git diff --staged --quiet; then
    echo "⚠️  No changes to commit. Everything is up to date!"
    exit 0
fi

# Create commit
echo "💾 Creating commit..."
git commit -m "Initial commit: MLM Generation Plan application"

# Add remote if not exists
if ! git remote get-url origin &> /dev/null; then
    echo "🔗 Adding GitHub remote..."
    git remote add origin "https://github.com/$GITHUB_USERNAME/$REPO_NAME.git"
else
    echo "🔄 Updating remote URL..."
    git remote set-url origin "https://github.com/$GITHUB_USERNAME/$REPO_NAME.git"
fi

# Rename branch to main
git branch -M main

# Push to GitHub
echo ""
echo "🚀 Pushing to GitHub..."
echo ""

if git push -u origin main; then
    echo ""
    echo "✅ Successfully pushed to GitHub!"
    echo ""
    echo "🌐 Your repository is now available at:"
    echo "   https://github.com/$GITHUB_USERNAME/$REPO_NAME"
    echo ""
    echo "📚 Next steps:"
    echo "   1. View your code on GitHub"
    echo "   2. Check DEPLOYMENT.md for free hosting options"
    echo "   3. Set up automatic deployments"
    echo ""
else
    echo ""
    echo "❌ Failed to push to GitHub!"
    echo ""
    echo "💡 Troubleshooting:"
    echo "   1. Make sure the repository exists on GitHub"
    echo "   2. Check your GitHub credentials"
    echo "   3. You may need to use a Personal Access Token"
    echo "      (GitHub → Settings → Developer settings → Personal access tokens)"
    echo ""
    echo "   Or use GitHub CLI:"
    echo "   brew install gh"
    echo "   gh auth login"
    echo "   git push -u origin main"
    echo ""
fi

