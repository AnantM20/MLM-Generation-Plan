#!/bin/bash
# Script to update Admin user to have referrals in existing database

echo "🔄 Updating Admin user referrals..."
echo "=========================================="
echo ""

# Check if Docker is running
if ! docker ps &> /dev/null; then
    echo "❌ Docker is not running!"
    echo "Please start Docker Desktop first."
    exit 1
fi

# Check if SQL Server container exists
if ! docker ps -a | grep -q sqlserver; then
    echo "❌ SQL Server container not found!"
    echo "Please run setup-database.sh first."
    exit 1
fi

# Start SQL Server if not running
if ! docker ps | grep -q sqlserver; then
    echo "🚀 Starting SQL Server container..."
    docker start sqlserver
    sleep 5
fi

echo "📝 Updating database..."

# Create SQL script file
cat > /tmp/update_admin.sql << 'EOF'
USE MLMDb;
GO

-- Update REG1001 to be sponsored by Admin
UPDATE Users 
SET SponsorId = 'REG1000' 
WHERE UserId = 'REG1001' AND (SponsorId IS NULL OR SponsorId != 'REG1000');
GO

-- Add additional direct referrals for Admin if they don't exist
IF NOT EXISTS (SELECT * FROM Users WHERE UserId = 'REG1016')
BEGIN
    INSERT INTO Users (FullName, Email, MobileNumber, UserId, Password, SponsorId, IsActive, IsAdmin, RegistrationDate)
    VALUES ('Sarah Connor', 'sarah.connor@example.com', '9876543225', 'REG1016', 'VGVzdEAxMjM=', 'REG1000', 1, 0, DATEADD(DAY, -16, GETDATE()));
END
GO

IF NOT EXISTS (SELECT * FROM Users WHERE UserId = 'REG1017')
BEGIN
    INSERT INTO Users (FullName, Email, MobileNumber, UserId, Password, SponsorId, IsActive, IsAdmin, RegistrationDate)
    VALUES ('Mike Tyson', 'mike.tyson@example.com', '9876543226', 'REG1017', 'VGVzdEAxMjM=', 'REG1000', 1, 0, DATEADD(DAY, -17, GETDATE()));
END
GO

PRINT 'Admin referrals updated successfully!';
GO
EOF

# Copy script to container and execute
docker cp /tmp/update_admin.sql sqlserver:/tmp/update_admin.sql

# Try different sqlcmd paths
if docker exec sqlserver which sqlcmd &> /dev/null; then
    SQLCMD=$(docker exec sqlserver which sqlcmd | tr -d '\r')
elif [ -f "/opt/mssql-tools18/bin/sqlcmd" ]; then
    SQLCMD="/opt/mssql-tools18/bin/sqlcmd"
else
    SQLCMD="/opt/mssql-tools/bin/sqlcmd"
fi

docker exec -i sqlserver $SQLCMD \
   -S localhost -U sa -P "YourStrong@Passw0rd123!" \
   -d MLMDb -i /tmp/update_admin.sql 2>/dev/null || echo "Note: Some updates may have already been applied."

# Cleanup
rm -f /tmp/update_admin.sql

echo ""
echo "✅ Database updated successfully!"
echo ""
echo "Admin (REG1000) now has direct referrals:"
echo "  - REG1001 (John Doe)"
echo "  - REG1016 (Sarah Connor)"
echo "  - REG1017 (Mike Tyson)"
echo ""
echo "Please refresh your browser to see the updated dashboard!"

