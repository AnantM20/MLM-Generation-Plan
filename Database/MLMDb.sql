-- =============================================
-- MLM Generation Plan Database Script
-- Database: MLMDb
-- =============================================

-- Create Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'MLMDb')
BEGIN
    CREATE DATABASE MLMDb;
END
GO

USE MLMDb;
GO

-- =============================================
-- Drop Tables if exists
-- =============================================
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL
    DROP TABLE dbo.Users;
GO

-- =============================================
-- Create Users Table
-- =============================================
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    MobileNumber NVARCHAR(15) NOT NULL UNIQUE,
    UserId NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    SponsorId NVARCHAR(50) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    IsAdmin BIT NOT NULL DEFAULT 0,
    RegistrationDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Users_Sponsor FOREIGN KEY (SponsorId) REFERENCES Users(UserId) ON DELETE NO ACTION
);
GO

-- =============================================
-- Create Indexes
-- =============================================
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_UserId ON Users(UserId);
CREATE INDEX IX_Users_MobileNumber ON Users(MobileNumber);
CREATE INDEX IX_Users_SponsorId ON Users(SponsorId);
GO

-- =============================================
-- Insert Sample Data
-- =============================================

-- Insert Admin User
-- Password: Admin@123 (base64 encoded)
INSERT INTO Users (FullName, Email, MobileNumber, UserId, Password, SponsorId, IsActive, IsAdmin, RegistrationDate)
VALUES ('Admin User', 'admin@mlm.com', '9999999999', 'REG1000', 'QWRtaW5AMTIz', NULL, 1, 1, GETDATE());
GO

-- Insert Root User (Level 0)
-- Password: Test@123
INSERT INTO Users (FullName, Email, MobileNumber, UserId, Password, SponsorId, IsActive, IsAdmin, RegistrationDate)
VALUES ('John Doe', 'john.doe@example.com', '9876543210', 'REG1001', 'VGVzdEAxMjM=', NULL, 1, 0, GETDATE());
GO

-- Insert Level 1 Users (Direct referrals of REG1001)
INSERT INTO Users (FullName, Email, MobileNumber, UserId, Password, SponsorId, IsActive, IsAdmin, RegistrationDate)
VALUES 
    ('Alice Smith', 'alice.smith@example.com', '9876543211', 'REG1002', 'VGVzdEAxMjM=', 'REG1001', 1, 0, DATEADD(DAY, 1, GETDATE())),
    ('Bob Johnson', 'bob.johnson@example.com', '9876543212', 'REG1003', 'VGVzdEAxMjM=', 'REG1001', 1, 0, DATEADD(DAY, 2, GETDATE())),
    ('Carol Williams', 'carol.williams@example.com', '9876543213', 'REG1004', 'VGVzdEAxMjM=', 'REG1001', 1, 0, DATEADD(DAY, 3, GETDATE()));
GO

-- Insert Level 2 Users (Referrals of Level 1 users)
INSERT INTO Users (FullName, Email, MobileNumber, UserId, Password, SponsorId, IsActive, IsAdmin, RegistrationDate)
VALUES 
    ('David Brown', 'david.brown@example.com', '9876543214', 'REG1005', 'VGVzdEAxMjM=', 'REG1002', 1, 0, DATEADD(DAY, 4, GETDATE())),
    ('Emma Davis', 'emma.davis@example.com', '9876543215', 'REG1006', 'VGVzdEAxMjM=', 'REG1002', 1, 0, DATEADD(DAY, 5, GETDATE())),
    ('Frank Miller', 'frank.miller@example.com', '9876543216', 'REG1007', 'VGVzdEAxMjM=', 'REG1003', 1, 0, DATEADD(DAY, 6, GETDATE())),
    ('Grace Wilson', 'grace.wilson@example.com', '9876543217', 'REG1008', 'VGVzdEAxMjM=', 'REG1003', 1, 0, DATEADD(DAY, 7, GETDATE())),
    ('Henry Moore', 'henry.moore@example.com', '9876543218', 'REG1009', 'VGVzdEAxMjM=', 'REG1004', 1, 0, DATEADD(DAY, 8, GETDATE()));
GO

-- Insert Level 3 Users (Referrals of Level 2 users)
INSERT INTO Users (FullName, Email, MobileNumber, UserId, Password, SponsorId, IsActive, IsAdmin, RegistrationDate)
VALUES 
    ('Ivy Taylor', 'ivy.taylor@example.com', '9876543219', 'REG1010', 'VGVzdEAxMjM=', 'REG1005', 1, 0, DATEADD(DAY, 9, GETDATE())),
    ('Jack Anderson', 'jack.anderson@example.com', '9876543220', 'REG1011', 'VGVzdEAxMjM=', 'REG1005', 1, 0, DATEADD(DAY, 10, GETDATE())),
    ('Kate Thomas', 'kate.thomas@example.com', '9876543221', 'REG1012', 'VGVzdEAxMjM=', 'REG1006', 1, 0, DATEADD(DAY, 11, GETDATE())),
    ('Leo Jackson', 'leo.jackson@example.com', '9876543222', 'REG1013', 'VGVzdEAxMjM=', 'REG1007', 1, 0, DATEADD(DAY, 12, GETDATE())),
    ('Mia White', 'mia.white@example.com', '9876543223', 'REG1014', 'VGVzdEAxMjM=', 'REG1008', 1, 0, DATEADD(DAY, 13, GETDATE())),
    ('Noah Harris', 'noah.harris@example.com', '9876543224', 'REG1015', 'VGVzdEAxMjM=', 'REG1009', 1, 0, DATEADD(DAY, 14, GETDATE()));
GO

-- =============================================
-- Verification Queries
-- =============================================
-- SELECT * FROM Users ORDER BY RegistrationDate;
-- SELECT UserId, FullName, SponsorId FROM Users WHERE SponsorId IS NOT NULL ORDER BY SponsorId;
-- SELECT COUNT(*) AS TotalUsers FROM Users;
-- SELECT COUNT(*) AS ActiveUsers FROM Users WHERE IsActive = 1;
-- SELECT COUNT(*) AS AdminUsers FROM Users WHERE IsAdmin = 1;

PRINT 'Database setup completed successfully!';
PRINT 'Sample data inserted.';
PRINT '';
PRINT 'Sample Credentials:';
PRINT 'Admin Login:';
PRINT '  Email: admin@mlm.com';
PRINT '  Password: Admin@123';
PRINT '';
PRINT 'User Login:';
PRINT '  Email: john.doe@example.com';
PRINT '  Password: Test@123';
PRINT '';
PRINT 'Note: All sample users have password: Test@123';
GO

